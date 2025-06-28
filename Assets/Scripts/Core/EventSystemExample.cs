using UnityEngine;

namespace TicTacToe
{
    /// <summary>
    /// Example script demonstrating how to use the GameEvents system.
    /// This script shows proper event subscription and unsubscription patterns.
    /// </summary>
    public class EventSystemExample : MonoBehaviour
    {
        [Header("Event System Example")]
        [SerializeField] private bool _enableLogging = true;
        
        #region Unity Lifecycle
        
        private void OnEnable()
        {
            SubscribeToEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }
        
        #endregion
        
        #region Event Subscriptions
        
        /// <summary>
        /// Subscribe to all relevant game events
        /// </summary>
        private void SubscribeToEvents()
        {
            // Game State Events
            GameEvents.OnGameStateChanged += HandleGameStateChanged;
            GameEvents.OnGameStarted += HandleGameStarted;
            GameEvents.OnGameEnded += HandleGameEnded;
            GameEvents.OnPlayerWon += HandlePlayerWon;
            GameEvents.OnGameDraw += HandleGameDraw;
            
            // Turn Management Events
            GameEvents.OnPlayerTurnChanged += HandlePlayerTurnChanged;
            GameEvents.OnPlayerMoved += HandlePlayerMoved;
            GameEvents.OnInvalidMove += HandleInvalidMove;
            
            // Board Events
            GameEvents.OnBoardReset += HandleBoardReset;
            GameEvents.OnCellMarked += HandleCellMarked;
            GameEvents.OnWinningLineFound += HandleWinningLineFound;
            
            // AI Events
            GameEvents.OnAIThinkingStarted += HandleAIThinkingStarted;
            GameEvents.OnAIMoveCompleted += HandleAIMoveCompleted;
            GameEvents.OnAIDifficultyChanged += HandleAIDifficultyChanged;
            
            // UI Events
            GameEvents.OnUIUpdateRequired += HandleUIUpdateRequired;
            GameEvents.OnButtonClicked += HandleButtonClicked;
            GameEvents.OnSettingsChanged += HandleSettingsChanged;
            
            // Online Multiplayer Events
            GameEvents.OnConnectingToOnline += HandleConnectingToOnline;
            GameEvents.OnConnectedToOnline += HandleConnectedToOnline;
            GameEvents.OnOnlineConnectionFailed += HandleOnlineConnectionFailed;
            GameEvents.OnPlayerJoinedRoom += HandlePlayerJoinedRoom;
            GameEvents.OnPlayerLeftRoom += HandlePlayerLeftRoom;
            GameEvents.OnRoomReady += HandleRoomReady;
            
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Subscribed to all GameEvents");
            }
        }
        
        /// <summary>
        /// Unsubscribe from all game events to prevent memory leaks
        /// </summary>
        private void UnsubscribeFromEvents()
        {
            // Game State Events
            GameEvents.OnGameStateChanged -= HandleGameStateChanged;
            GameEvents.OnGameStarted -= HandleGameStarted;
            GameEvents.OnGameEnded -= HandleGameEnded;
            GameEvents.OnPlayerWon -= HandlePlayerWon;
            GameEvents.OnGameDraw -= HandleGameDraw;
            
            // Turn Management Events
            GameEvents.OnPlayerTurnChanged -= HandlePlayerTurnChanged;
            GameEvents.OnPlayerMoved -= HandlePlayerMoved;
            GameEvents.OnInvalidMove -= HandleInvalidMove;
            
            // Board Events
            GameEvents.OnBoardReset -= HandleBoardReset;
            GameEvents.OnCellMarked -= HandleCellMarked;
            GameEvents.OnWinningLineFound -= HandleWinningLineFound;
            
            // AI Events
            GameEvents.OnAIThinkingStarted -= HandleAIThinkingStarted;
            GameEvents.OnAIMoveCompleted -= HandleAIMoveCompleted;
            GameEvents.OnAIDifficultyChanged -= HandleAIDifficultyChanged;
            
            // UI Events
            GameEvents.OnUIUpdateRequired -= HandleUIUpdateRequired;
            GameEvents.OnButtonClicked -= HandleButtonClicked;
            GameEvents.OnSettingsChanged -= HandleSettingsChanged;
            
            // Online Multiplayer Events
            GameEvents.OnConnectingToOnline -= HandleConnectingToOnline;
            GameEvents.OnConnectedToOnline -= HandleConnectedToOnline;
            GameEvents.OnOnlineConnectionFailed -= HandleOnlineConnectionFailed;
            GameEvents.OnPlayerJoinedRoom -= HandlePlayerJoinedRoom;
            GameEvents.OnPlayerLeftRoom -= HandlePlayerLeftRoom;
            GameEvents.OnRoomReady -= HandleRoomReady;
            
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Unsubscribed from all GameEvents");
            }
        }
        
        #endregion
        
        #region Event Handlers
        
        // Game State Event Handlers
        private void HandleGameStateChanged(GameState newState)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Game state changed to {newState}");
            }
        }
        
        private void HandleGameStarted()
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Game started");
            }
        }
        
        private void HandleGameEnded(GameEvents.GameResult result)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Game ended with result {result}");
            }
        }
        
        private void HandlePlayerWon(GameEvents.PlayerType player)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Player {player} won!");
            }
        }
        
        private void HandleGameDraw()
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Game ended in a draw");
            }
        }
        
        // Turn Management Event Handlers
        private void HandlePlayerTurnChanged(GameEvents.PlayerType player)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Player turn changed to {player}");
            }
        }
        
        private void HandlePlayerMoved(GameEvents.PlayerType player, int row, int col)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Player {player} moved to ({row}, {col})");
            }
        }
        
        private void HandleInvalidMove(string message)
        {
            if (_enableLogging)
            {
                Debug.LogWarning($"EventSystemExample: Invalid move - {message}");
            }
        }
        
        // Board Event Handlers
        private void HandleBoardReset()
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Board reset");
            }
        }
        
        private void HandleCellMarked(int row, int col, GameEvents.PlayerType player)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Cell ({row}, {col}) marked by {player}");
            }
        }
        
        private void HandleWinningLineFound(Vector2Int start, Vector2Int end)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Winning line found from ({start.x}, {start.y}) to ({end.x}, {end.y})");
            }
        }
        
        // AI Event Handlers
        private void HandleAIThinkingStarted()
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: AI started thinking");
            }
        }
        
        private void HandleAIMoveCompleted(int row, int col)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: AI completed move to ({row}, {col})");
            }
        }
        
        private void HandleAIDifficultyChanged(GameEvents.AIDifficulty difficulty)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: AI difficulty changed to {difficulty}");
            }
        }
        
        // UI Event Handlers
        private void HandleUIUpdateRequired()
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: UI update required");
            }
        }
        
        private void HandleButtonClicked(string buttonName)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Button '{buttonName}' clicked");
            }
        }
        
        private void HandleSettingsChanged(GameSettings settings)
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Settings changed");
            }
        }
        
        // Online Multiplayer Event Handlers
        private void HandleConnectingToOnline()
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Connecting to online services");
            }
        }
        
        private void HandleConnectedToOnline()
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Connected to online services");
            }
        }
        
        private void HandleOnlineConnectionFailed(string error)
        {
            if (_enableLogging)
            {
                Debug.LogError($"EventSystemExample: Online connection failed - {error}");
            }
        }
        
        private void HandlePlayerJoinedRoom(string playerName)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Player '{playerName}' joined room");
            }
        }
        
        private void HandlePlayerLeftRoom(string playerName)
        {
            if (_enableLogging)
            {
                Debug.Log($"EventSystemExample: Player '{playerName}' left room");
            }
        }
        
        private void HandleRoomReady()
        {
            if (_enableLogging)
            {
                Debug.Log("EventSystemExample: Room is ready to start");
            }
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Toggles event logging on/off
        /// </summary>
        public void ToggleLogging()
        {
            _enableLogging = !_enableLogging;
            Debug.Log($"EventSystemExample: Logging {( _enableLogging ? "enabled" : "disabled" )}");
        }
        
        #endregion
    }
} 