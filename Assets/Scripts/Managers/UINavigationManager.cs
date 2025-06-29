using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <summary>
    /// Manages UI screen navigation based on game states.
    /// Each GameState has a corresponding UI screen that gets shown/hidden using CanvasGroup.
    /// Handles transitions between screens and integrates with the event system.
    /// </summary>
    public class UINavigationManager : MonoBehaviour
    {
        #region Singleton

        private static UINavigationManager _instance;
        public static UINavigationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UINavigationManager>();
                    
                    if (_instance == null)
                    {
                        // Auto-initialization for quick testing
                        GameObject uiNavigationManagerObject = new GameObject("UINavigationManager");
                        _instance = uiNavigationManagerObject.AddComponent<UINavigationManager>();
                        Debug.Log("UINavigationManager auto-created for quick testing");
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Serialized Fields

        [Header("UI Screen References")]
        [SerializeField] private List<UIScreenData> _uiScreens = new List<UIScreenData>();

        [Header("Navigation Settings")]
        [SerializeField] private TransitionType _defaultTransitionType = TransitionType.Fade;
        [SerializeField] private float _defaultTransitionDuration = 0.5f;
        [SerializeField] private bool _hideAllScreensOnStart = true;

        #endregion

        #region Private Fields

        private Dictionary<GameState, UIScreenData> _screenDictionary = new Dictionary<GameState, UIScreenData>();
        private GameState _currentScreenState = GameState.Loading;
        private UIScreenData _currentActiveScreen;
        private Coroutine _currentTransitionCoroutine;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            // Singleton pattern implementation
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeManager();
            }
            else if (_instance != this)
            {
                Debug.LogWarning("Duplicate UINavigationManager found. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            // Clean up singleton reference if this instance is being destroyed
            if (_instance == this)
            {
                GameEvents.OnGameStateChanged -= OnGameStateChanged;
                GameEvents.OnScreenTransitionRequested -= OnScreenTransitionRequested;
                _instance = null;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the UI navigation manager
        /// </summary>
        private void InitializeManager()
        {
            Debug.Log("UINavigationManager initialized");
            
            // Build screen dictionary
            BuildScreenDictionary();
            
            // Subscribe to events
            GameEvents.OnGameStateChanged += OnGameStateChanged;
            GameEvents.OnScreenTransitionRequested += OnScreenTransitionRequested;
            
            // Initialize screens
            InitializeScreens();
        }

        /// <summary>
        /// Builds the screen dictionary for quick lookup
        /// </summary>
        private void BuildScreenDictionary()
        {
            _screenDictionary.Clear();
            
            foreach (var screen in _uiScreens)
            {
                if (screen.CanvasGroup != null)
                {
                    if (_screenDictionary.ContainsKey(screen.GameState))
                    {
                        Debug.LogWarning($"Duplicate screen for GameState found: {screen.GameState}");
                    }
                    else
                    {
                        _screenDictionary.Add(screen.GameState, screen);
                    }
                }
            }
            
            Debug.Log($"UINavigationManager: {_screenDictionary.Count} UI screens registered");
        }

        /// <summary>
        /// Initializes all UI screens
        /// </summary>
        private void InitializeScreens()
        {
            if (_hideAllScreensOnStart)
            {
                // Hide all screens initially
                foreach (var screen in _screenDictionary.Values)
                {
                    if (screen.CanvasGroup != null)
                    {
                        screen.CanvasGroup.alpha = 0f;
                        screen.CanvasGroup.interactable = false;
                        screen.CanvasGroup.blocksRaycasts = false;
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles game state changes and shows corresponding UI screen
        /// </summary>
        /// <param name="newState">New game state</param>
        private void OnGameStateChanged(GameState newState)
        {
            ShowScreenForState(newState, _defaultTransitionType, _defaultTransitionDuration);
        }

        /// <summary>
        /// Handles screen transition requests
        /// </summary>
        /// <param name="targetState">Target game state</param>
        /// <param name="transitionType">Type of transition</param>
        private void OnScreenTransitionRequested(GameState targetState, TransitionType transitionType)
        {
            ShowScreenForState(targetState, transitionType, _defaultTransitionDuration);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the UI screen for a specific game state
        /// </summary>
        /// <param name="gameState">Game state to show screen for</param>
        /// <param name="transitionType">Type of transition to use</param>
        /// <param name="duration">Duration of the transition</param>
        public void ShowScreenForState(GameState gameState, TransitionType? transitionType = null, float? duration = null)
        {
            if (!_screenDictionary.ContainsKey(gameState))
            {
                Debug.LogWarning($"No UI screen found for GameState: {gameState}");
                return;
            }

            var targetScreen = _screenDictionary[gameState];
            if (targetScreen.CanvasGroup == null)
            {
                Debug.LogError($"CanvasGroup is null for GameState: {gameState}");
                return;
            }

            // If this is the same screen, don't transition
            if (_currentActiveScreen == targetScreen)
            {
                return;
            }

            // Stop current transition if running
            if (_currentTransitionCoroutine != null)
            {
                StopCoroutine(_currentTransitionCoroutine);
                _currentTransitionCoroutine = null;
            }

            // Perform transition
            TransitionType transition = transitionType ?? _defaultTransitionType;
            float transitionDuration = duration ?? _defaultTransitionDuration;

            if (_currentActiveScreen != null)
            {
                // Transition between screens
                _currentTransitionCoroutine = StartCoroutine(TransitionBetweenScreens(_currentActiveScreen, targetScreen, transition, transitionDuration));
            }
            else
            {
                // Show screen directly
                _currentTransitionCoroutine = StartCoroutine(ShowScreenDirectly(targetScreen, transition, transitionDuration));
            }

            _currentScreenState = gameState;
            _currentActiveScreen = targetScreen;

            Debug.Log($"Showing UI screen for GameState: {gameState}");
        }

        /// <summary>
        /// Shows a specific UI screen by GameState
        /// </summary>
        /// <param name="gameState">Game state to show</param>
        public void ShowScreen(GameState gameState)
        {
            ShowScreenForState(gameState);
        }

        /// <summary>
        /// Hides the current active screen
        /// </summary>
        /// <param name="transitionType">Type of transition to use</param>
        /// <param name="duration">Duration of the transition</param>
        public void HideCurrentScreen(TransitionType? transitionType = null, float? duration = null)
        {
            if (_currentActiveScreen == null)
            {
                return;
            }

            TransitionType transition = transitionType ?? _defaultTransitionType;
            float transitionDuration = duration ?? _defaultTransitionDuration;

            TransitionManager.Instance.TransitionScreen(_currentActiveScreen.CanvasGroup, transition, false, transitionDuration);
            
            _currentActiveScreen = null;
            _currentScreenState = GameState.Loading;
        }

        /// <summary>
        /// Gets the current active screen
        /// </summary>
        /// <returns>Current active screen data</returns>
        public UIScreenData GetCurrentActiveScreen()
        {
            return _currentActiveScreen;
        }

        /// <summary>
        /// Gets the current screen state
        /// </summary>
        /// <returns>Current game state</returns>
        public GameState GetCurrentScreenState()
        {
            return _currentScreenState;
        }

        /// <summary>
        /// Checks if a screen is currently active for a specific game state
        /// </summary>
        /// <param name="gameState">Game state to check</param>
        /// <returns>True if the screen is active</returns>
        public bool IsScreenActive(GameState gameState)
        {
            return _currentScreenState == gameState;
        }

        #endregion

        #region Private Transition Methods

        /// <summary>
        /// Transitions between two screens
        /// </summary>
        private System.Collections.IEnumerator TransitionBetweenScreens(UIScreenData hideScreen, UIScreenData showScreen, 
            TransitionType transitionType, float duration)
        {
            // Start both transitions simultaneously
            Coroutine hideCoroutine = TransitionManager.Instance.TransitionScreen(hideScreen.CanvasGroup, transitionType, false, duration);
            Coroutine showCoroutine = TransitionManager.Instance.TransitionScreen(showScreen.CanvasGroup, transitionType, true, duration);

            // Wait for both transitions to complete
            if (hideCoroutine != null) yield return hideCoroutine;
            if (showCoroutine != null) yield return showCoroutine;

            _currentTransitionCoroutine = null;
        }

        /// <summary>
        /// Shows a screen directly without hiding another
        /// </summary>
        private System.Collections.IEnumerator ShowScreenDirectly(UIScreenData showScreen, TransitionType transitionType, float duration)
        {
            Coroutine showCoroutine = TransitionManager.Instance.TransitionScreen(showScreen.CanvasGroup, transitionType, true, duration);
            
            if (showCoroutine != null) yield return showCoroutine;

            _currentTransitionCoroutine = null;
        }

        #endregion

        #region Editor Support

#if UNITY_EDITOR
        [ContextMenu("Refresh Screen Dictionary")]
        private void RefreshScreenDictionary()
        {
            BuildScreenDictionary();
        }

        [ContextMenu("Show MainMenu Screen")]
        private void ShowMainMenuScreen()
        {
            ShowScreenForState(GameState.MainMenu);
        }

        [ContextMenu("Show Gameplay Screen")]
        private void ShowGameplayScreen()
        {
            ShowScreenForState(GameState.Gameplay);
        }

        [ContextMenu("Hide Current Screen")]
        private void HideCurrentScreenEditor()
        {
            HideCurrentScreen();
        }
#endif

        #endregion
    }
} 