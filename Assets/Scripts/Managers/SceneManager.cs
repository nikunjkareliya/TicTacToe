using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe
{
    /// <summary>
    /// Manages scene loading and unloading operations.
    /// Provides progress tracking and event-driven communication.
    /// Follows Single Responsibility Principle for scene management.
    /// </summary>
    public class SceneManager : MonoBehaviour
    {
        #region Singleton

        private static SceneManager _instance;
        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SceneManager>();
                    
                    if (_instance == null)
                    {
                        // Auto-initialization for quick testing
                        GameObject sceneManagerObject = new GameObject("SceneManager");
                        _instance = sceneManagerObject.AddComponent<SceneManager>();
                        Debug.Log("SceneManager auto-created for quick testing");
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Serialized Fields

        [Header("Scene Settings")]
        [SerializeField] private string _loadingSceneName = "Loading";
        [SerializeField] private string _gameSceneName = "Game";
        [SerializeField] private bool _showLoadingProgress = true;

        [Header("Loading Settings")]
        [SerializeField] private float _minimumLoadingTime = 1f; // Minimum time to show loading screen
        [SerializeField] private bool _useLoadingScene = true;

        #endregion

        #region Private Fields

        private bool _isLoading = false;
        private string _currentLoadingScene;
        private Coroutine _loadingCoroutine;

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
                Debug.LogWarning("Duplicate SceneManager found. Destroying duplicate.");
                Destroy(gameObject);
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

        #region Initialization

        /// <summary>
        /// Initializes the scene manager
        /// </summary>
        private void InitializeManager()
        {
            Debug.Log("SceneManager initialized");
            
            // Subscribe to events
            GameEvents.OnLoadScene += OnLoadSceneEvent;
            GameEvents.OnUnloadScene += OnUnloadSceneEvent;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a scene by name
        /// </summary>
        /// <param name="sceneName">Name of the scene to load</param>
        /// <param name="useLoadingScene">Whether to use loading scene transition</param>
        public void LoadScene(string sceneName, bool useLoadingScene = true)
        {
            if (_isLoading)
            {
                Debug.LogWarning("Scene loading already in progress");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is null or empty");
                return;
            }

            _currentLoadingScene = sceneName;
            _isLoading = true;

            if (useLoadingScene && _useLoadingScene)
            {
                StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
            }
            else
            {
                StartCoroutine(LoadSceneDirectly(sceneName));
            }
        }

        /// <summary>
        /// Unloads a scene by name
        /// </summary>
        /// <param name="sceneName">Name of the scene to unload</param>
        public void UnloadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is null or empty");
                return;
            }

            StartCoroutine(UnloadSceneCoroutine(sceneName));
        }

        /// <summary>
        /// Loads the game scene from loading scene
        /// </summary>
        public void LoadGameScene()
        {
            LoadScene(_gameSceneName, false);
        }

        /// <summary>
        /// Loads the loading scene
        /// </summary>
        public void LoadLoadingScene()
        {
            LoadScene(_loadingSceneName, false);
        }

        /// <summary>
        /// Checks if a scene is currently loading
        /// </summary>
        /// <returns>True if a scene is loading</returns>
        public bool IsLoading()
        {
            return _isLoading;
        }

        /// <summary>
        /// Gets the name of the currently loading scene
        /// </summary>
        /// <returns>Name of the loading scene</returns>
        public string GetCurrentLoadingScene()
        {
            return _currentLoadingScene;
        }

        #endregion

        #region Private Loading Methods

        /// <summary>
        /// Loads a scene with loading screen transition
        /// </summary>
        private IEnumerator LoadSceneWithLoadingScreen(string sceneName)
        {
            Debug.Log($"Loading scene with loading screen: {sceneName}");

            // Load loading scene first
            yield return LoadSceneDirectly(_loadingSceneName);

            // Wait a frame to ensure loading scene is fully loaded
            yield return null;

            // Start loading the target scene
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            float startTime = Time.time;
            float progress = 0f;

            // Load scene in background
            while (asyncLoad.progress < 0.9f)
            {
                progress = asyncLoad.progress / 0.9f;
                
                if (_showLoadingProgress)
                {
                    GameEvents.RaiseLoadingProgressUpdated(progress);
                }
                
                yield return null;
            }

            // Ensure minimum loading time
            float elapsedTime = Time.time - startTime;
            if (elapsedTime < _minimumLoadingTime)
            {
                yield return new WaitForSeconds(_minimumLoadingTime - elapsedTime);
            }

            // Complete loading
            asyncLoad.allowSceneActivation = true;
            
            // Wait for scene to fully load
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Scene loaded successfully
            _isLoading = false;
            _currentLoadingScene = null;
            
            GameEvents.RaiseLoadingProgressUpdated(1f);
            GameEvents.RaiseSceneLoadComplete();
            
            Debug.Log($"Scene loaded successfully: {sceneName}");
        }

        /// <summary>
        /// Loads a scene directly without loading screen
        /// </summary>
        private IEnumerator LoadSceneDirectly(string sceneName)
        {
            Debug.Log($"Loading scene directly: {sceneName}");

            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            
            while (!asyncLoad.isDone)
            {
                if (_showLoadingProgress)
                {
                    GameEvents.RaiseLoadingProgressUpdated(asyncLoad.progress);
                }
                yield return null;
            }

            _isLoading = false;
            _currentLoadingScene = null;
            
            GameEvents.RaiseLoadingProgressUpdated(1f);
            GameEvents.RaiseSceneLoadComplete();
            
            Debug.Log($"Scene loaded successfully: {sceneName}");
        }

        /// <summary>
        /// Unloads a scene asynchronously
        /// </summary>
        private IEnumerator UnloadSceneCoroutine(string sceneName)
        {
            Debug.Log($"Unloading scene: {sceneName}");

            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
            
            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            Debug.Log($"Scene unloaded successfully: {sceneName}");
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event handler for load scene event
        /// </summary>
        private void OnLoadSceneEvent(string sceneName)
        {
            LoadScene(sceneName);
        }

        /// <summary>
        /// Event handler for unload scene event
        /// </summary>
        private void OnUnloadSceneEvent(string sceneName)
        {
            UnloadScene(sceneName);
        }

        #endregion

        #region Editor Support

    #if UNITY_EDITOR
        [ContextMenu("Load Loading Scene")]
        private void LoadLoadingSceneEditor()
        {
            LoadLoadingScene();
        }

        [ContextMenu("Load Game Scene")]
        private void LoadGameSceneEditor()
        {
            LoadGameScene();
        }

        [ContextMenu("Reload Current Scene")]
        private void ReloadCurrentScene()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            LoadScene(currentSceneName, false);
        }
    #endif

        #endregion
    }
} 