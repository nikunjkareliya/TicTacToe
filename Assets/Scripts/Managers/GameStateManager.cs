using UnityEngine;

namespace TicTacToe
{
    /// <summary>
    /// Manages the current game state and provides state transition functionality.
    /// Follows the Singleton pattern and persists across scene loads.
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        #region Singleton

        private static GameStateManager _instance;
        public static GameStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameStateManager>();
                    
                    if (_instance == null)
                    {
                        // Auto-initialization for quick testing
                        GameObject gameStateManagerObject = new GameObject("GameStateManager");
                        _instance = gameStateManagerObject.AddComponent<GameStateManager>();
                        Debug.Log("GameStateManager auto-created for quick testing");
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Properties

        [SerializeField] private GameState _currentState = GameState.Loading;
        
        /// <summary>
        /// Current game state
        /// </summary>
        public GameState CurrentState
        {
            get => _currentState;
            private set
            {
                if (_currentState != value)
                {
                    GameState previousState = _currentState;
                    _currentState = value;
                    
                    Debug.Log($"Game State Changed: {previousState} -> {_currentState}");
                    
                    // Raise event for state change
                    GameEvents.RaiseGameStateChanged(_currentState);
                }
            }
        }

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
                Debug.LogWarning("Duplicate GameStateManager found. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // If this is the first time and we're not in loading state, set initial state
            if (CurrentState == GameState.Loading)
            {
                // Check if we're in the game scene directly (for quick testing)
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Loading")
                {
                    SetState(GameState.MainMenu);
                }
            }
        }

        private void OnDestroy()
        {
            // Clean up singleton reference if this instance is being destroyed
            if (_instance == this)
            {
                _instance = null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the current game state and raises the appropriate event
        /// </summary>
        /// <param name="newState">The new game state to set</param>
        public void SetState(GameState newState)
        {
            CurrentState = newState;
        }

        /// <summary>
        /// Gets the current game state
        /// </summary>
        /// <returns>Current game state</returns>
        public GameState GetCurrentState()
        {
            return CurrentState;
        }

        /// <summary>
        /// Checks if the current state matches the given state
        /// </summary>
        /// <param name="state">State to check against</param>
        /// <returns>True if current state matches</returns>
        public bool IsCurrentState(GameState state)
        {
            return CurrentState == state;
        }

        /// <summary>
        /// Transitions to the next state in the game flow
        /// </summary>
        public void TransitionToNextState()
        {
            GameState nextState = GetNextState(CurrentState);
            SetState(nextState);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the manager
        /// </summary>
        private void InitializeManager()
        {
            Debug.Log("GameStateManager initialized");
            
            // Subscribe to scene load complete event to handle state transitions
            GameEvents.OnSceneLoadComplete += OnSceneLoadComplete;
        }

        /// <summary>
        /// Handles scene load completion
        /// </summary>
        private void OnSceneLoadComplete()
        {
            // If we just loaded the game scene from loading scene, transition to MainMenu
            if (CurrentState == GameState.Loading)
            {
                SetState(GameState.MainMenu);
            }
        }

        /// <summary>
        /// Gets the next logical state in the game flow
        /// </summary>
        /// <param name="currentState">Current state</param>
        /// <returns>Next state</returns>
        private GameState GetNextState(GameState currentState)
        {
            switch (currentState)
            {
                case GameState.Loading:
                    return GameState.MainMenu;
                case GameState.MainMenu:
                    return GameState.MatchMaking;
                case GameState.MatchMaking:
                    return GameState.Gameplay;
                case GameState.Gameplay:
                    return GameState.Result;
                case GameState.Paused:
                    return GameState.Gameplay; // Return to gameplay when unpausing
                case GameState.Result:
                    return GameState.MainMenu; // Return to main menu after result
                default:
                    return GameState.MainMenu;
            }
        }

        #endregion

        #region Editor Support

    #if UNITY_EDITOR
        [ContextMenu("Set State to MainMenu")]
        private void SetToMainMenu()
        {
            SetState(GameState.MainMenu);
        }

        [ContextMenu("Set State to Gameplay")]
        private void SetToGameplay()
        {
            SetState(GameState.Gameplay);
        }

        [ContextMenu("Set State to Paused")]
        private void SetToPaused()
        {
            SetState(GameState.Paused);
        }
    #endif

        #endregion
    } 
}