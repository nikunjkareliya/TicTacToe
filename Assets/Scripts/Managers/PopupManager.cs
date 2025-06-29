using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <summary>
    /// Manages popup windows that can be shown over any current UI screen.
    /// Uses event-driven communication and CanvasGroup for smooth transitions.
    /// </summary>
    public class PopupManager : MonoBehaviour
    {
        #region Singleton

        private static PopupManager _instance;
        public static PopupManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PopupManager>();
                    
                    if (_instance == null)
                    {
                        // Auto-initialization for quick testing
                        GameObject popupManagerObject = new GameObject("PopupManager");
                        _instance = popupManagerObject.AddComponent<PopupManager>();
                        Debug.Log("PopupManager auto-created for quick testing");
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Serialized Fields

        [Header("Popup Settings")]
        [SerializeField] private Transform _popupContainer;
        [SerializeField] private TransitionType _defaultTransitionType = TransitionType.Scale;
        [SerializeField] private float _defaultTransitionDuration = 0.3f;

        [Header("Popup References")]
        [SerializeField] private List<PopupData> _popups = new List<PopupData>();

        #endregion

        #region Private Fields

        private Dictionary<string, PopupData> _popupDictionary = new Dictionary<string, PopupData>();
        private List<string> _activePopups = new List<string>();

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
                Debug.LogWarning("Duplicate PopupManager found. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                // Unsubscribe from events to prevent memory leaks
                GameEvents.OnShowPopup -= OnShowPopupEvent;
                GameEvents.OnHidePopup -= OnHidePopupEvent;
                GameEvents.OnHideAllPopups -= OnHideAllPopupsEvent;

                _instance = null;
            }
        }

    #endregion

        #region Initialization

        /// <summary>
        /// Initializes the popup manager
        /// </summary>
        private void InitializeManager()
        {
            Debug.Log("PopupManager initialized");
            
            // Build popup dictionary
            BuildPopupDictionary();
            
            // Subscribe to events
            GameEvents.OnShowPopup += OnShowPopupEvent;
            GameEvents.OnHidePopup += OnHidePopupEvent;
            GameEvents.OnHideAllPopups += OnHideAllPopupsEvent;
            
            // Initialize all popups as hidden
            InitializePopups();
        }

        /// <summary>
        /// Builds the popup dictionary for quick lookup
        /// </summary>
        private void BuildPopupDictionary()
        {
            _popupDictionary.Clear();
            
            foreach (var popup in _popups)
            {
                if (!string.IsNullOrEmpty(popup.PopupName) && popup.CanvasGroup != null)
                {
                    if (_popupDictionary.ContainsKey(popup.PopupName))
                    {
                        Debug.LogWarning($"Duplicate popup name found: {popup.PopupName}");
                    }
                    else
                    {
                        _popupDictionary.Add(popup.PopupName, popup);
                    }
                }
            }
            
            Debug.Log($"PopupManager: {_popupDictionary.Count} popups registered");
        }

        /// <summary>
        /// Initializes all popups to hidden state
        /// </summary>
        private void InitializePopups()
        {
            foreach (var popup in _popupDictionary.Values)
            {
                if (popup.CanvasGroup != null)
                {
                    popup.CanvasGroup.alpha = 0f;
                    popup.CanvasGroup.interactable = false;
                    popup.CanvasGroup.blocksRaycasts = false;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows a popup by name
        /// </summary>
        /// <param name="popupName">Name of the popup to show</param>
        /// <param name="transitionType">Type of transition to use</param>
        /// <param name="duration">Duration of the transition</param>
        public void ShowPopup(string popupName, TransitionType? transitionType = null, float? duration = null)
        {
            if (!_popupDictionary.ContainsKey(popupName))
            {
                Debug.LogError($"Popup '{popupName}' not found in PopupManager");
                return;
            }

            var popup = _popupDictionary[popupName];
            if (popup.CanvasGroup == null)
            {
                Debug.LogError($"CanvasGroup is null for popup '{popupName}'");
                return;
            }

            // Check if popup is already active
            if (_activePopups.Contains(popupName))
            {
                Debug.LogWarning($"Popup '{popupName}' is already active");
                return;
            }

            // Add to active popups
            _activePopups.Add(popupName);

            // Perform transition
            TransitionType transition = transitionType ?? _defaultTransitionType;
            float transitionDuration = duration ?? _defaultTransitionDuration;

            TransitionManager.Instance.TransitionScreen(popup.CanvasGroup, transition, true, transitionDuration);
            
            Debug.Log($"Showing popup: {popupName}");
        }

        /// <summary>
        /// Hides a popup by name
        /// </summary>
        /// <param name="popupName">Name of the popup to hide</param>
        /// <param name="transitionType">Type of transition to use</param>
        /// <param name="duration">Duration of the transition</param>
        public void HidePopup(string popupName, TransitionType? transitionType = null, float? duration = null)
        {
            if (!_popupDictionary.ContainsKey(popupName))
            {
                Debug.LogError($"Popup '{popupName}' not found in PopupManager");
                return;
            }

            var popup = _popupDictionary[popupName];
            if (popup.CanvasGroup == null)
            {
                Debug.LogError($"CanvasGroup is null for popup '{popupName}'");
                return;
            }

            // Check if popup is not active
            if (!_activePopups.Contains(popupName))
            {
                Debug.LogWarning($"Popup '{popupName}' is not active");
                return;
            }

            // Remove from active popups
            _activePopups.Remove(popupName);

            // Perform transition
            TransitionType transition = transitionType ?? _defaultTransitionType;
            float transitionDuration = duration ?? _defaultTransitionDuration;

            TransitionManager.Instance.TransitionScreen(popup.CanvasGroup, transition, false, transitionDuration);
            
            Debug.Log($"Hiding popup: {popupName}");
        }

        /// <summary>
        /// Hides all active popups
        /// </summary>
        /// <param name="transitionType">Type of transition to use</param>
        /// <param name="duration">Duration of the transition</param>
        public void HideAllPopups(TransitionType? transitionType = null, float? duration = null)
        {
            if (_activePopups.Count == 0)
            {
                return;
            }

            TransitionType transition = transitionType ?? _defaultTransitionType;
            float transitionDuration = duration ?? _defaultTransitionDuration;

            // Hide all active popups
            foreach (string popupName in _activePopups.ToArray())
            {
                HidePopup(popupName, transition, transitionDuration);
            }

            Debug.Log($"Hiding all popups ({_activePopups.Count} active)");
        }

        /// <summary>
        /// Checks if a popup is currently active
        /// </summary>
        /// <param name="popupName">Name of the popup to check</param>
        /// <returns>True if the popup is active</returns>
        public bool IsPopupActive(string popupName)
        {
            return _activePopups.Contains(popupName);
        }

        /// <summary>
        /// Gets the number of active popups
        /// </summary>
        /// <returns>Number of active popups</returns>
        public int GetActivePopupCount()
        {
            return _activePopups.Count;
        }

        /// <summary>
        /// Gets all active popup names
        /// </summary>
        /// <returns>List of active popup names</returns>
        public List<string> GetActivePopupNames()
        {
            return new List<string>(_activePopups);
        }

        #endregion

        #region Editor Support

    #if UNITY_EDITOR
        [ContextMenu("Refresh Popup Dictionary")]
        private void RefreshPopupDictionary()
        {
            BuildPopupDictionary();
        }

        [ContextMenu("Show All Popups")]
        private void ShowAllPopups()
        {
            foreach (var popup in _popupDictionary.Values)
            {
                if (popup.CanvasGroup != null)
                {
                    ShowPopup(popup.PopupName);
                }
            }
        }
    #endif

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event handler for show popup event
        /// </summary>
        private void OnShowPopupEvent(string popupName)
        {
            ShowPopup(popupName);
        }

        /// <summary>
        /// Event handler for hide popup event
        /// </summary>
        private void OnHidePopupEvent(string popupName)
        {
            HidePopup(popupName);
        }

        /// <summary>
        /// Event handler for hide all popups event
        /// </summary>
        private void OnHideAllPopupsEvent()
        {
            HideAllPopups();
        }

        #endregion
    }

    /// <summary>
    /// Data structure for popup information
    /// </summary>
    [System.Serializable]
    public class PopupData
    {
        [SerializeField] private string _popupName;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private string _description;

        public string PopupName => _popupName;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public string Description => _description;
    }
} 