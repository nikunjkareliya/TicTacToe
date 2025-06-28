using System;
using UnityEngine;

namespace TicTacToe
{
    /// <summary>
    /// Central event system for the TicTacToe game.
    /// Provides a decoupled communication system between different game components.
    /// </summary>
    public static class GameEvents
    {
        #region Game State Events

        /// <summary>
        /// Raised when the game state changes
        /// </summary>
        public static event Action<GameState> OnGameStateChanged;

        /// <summary>
        /// Raised when a new game starts
        /// </summary>
        public static event Action OnGameStarted;

        /// <summary>
        /// Raised when the game ends (win, draw, or quit)
        /// </summary>
        public static event Action<GameResult> OnGameEnded;

        /// <summary>
        /// Raised when a player wins
        /// </summary>
        public static event Action<PlayerType> OnPlayerWon;

        /// <summary>
        /// Raised when the game ends in a draw
        /// </summary>
        public static event Action OnGameDraw;

        #endregion

        #region Turn Management Events

        /// <summary>
        /// Raised when the current player changes
        /// </summary>
        public static event Action<PlayerType> OnPlayerTurnChanged;

        /// <summary>
        /// Raised when a player makes a move
        /// </summary>
        public static event Action<PlayerType, int, int> OnPlayerMoved;

        /// <summary>
        /// Raised when a move is invalid
        /// </summary>
        public static event Action<string> OnInvalidMove;

        #endregion

        #region Board Events

        /// <summary>
        /// Raised when the board is reset
        /// </summary>
        public static event Action OnBoardReset;

        /// <summary>
        /// Raised when a cell is marked
        /// </summary>
        public static event Action<int, int, PlayerType> OnCellMarked;

        /// <summary>
        /// Raised when a winning line is found
        /// </summary>
        public static event Action<Vector2Int, Vector2Int> OnWinningLineFound;

        #endregion

        #region AI Events

        /// <summary>
        /// Raised when AI starts thinking
        /// </summary>
        public static event Action OnAIThinkingStarted;

        /// <summary>
        /// Raised when AI finishes thinking and makes a move
        /// </summary>
        public static event Action<int, int> OnAIMoveCompleted;

        /// <summary>
        /// Raised when AI difficulty changes
        /// </summary>
        public static event Action<AIDifficulty> OnAIDifficultyChanged;

        #endregion

        #region UI Events

        /// <summary>
        /// Raised when UI needs to be updated
        /// </summary>
        public static event Action OnUIUpdateRequired;

        /// <summary>
        /// Raised when a button is clicked
        /// </summary>
        public static event Action<string> OnButtonClicked;

        /// <summary>
        /// Raised when settings are changed
        /// </summary>
        public static event Action<GameSettings> OnSettingsChanged;

        #endregion

        #region Online Multiplayer Events

        /// <summary>
        /// Raised when connecting to online services
        /// </summary>
        public static event Action OnConnectingToOnline;

        /// <summary>
        /// Raised when successfully connected to online services
        /// </summary>
        public static event Action OnConnectedToOnline;

        /// <summary>
        /// Raised when connection to online services fails
        /// </summary>
        public static event Action<string> OnOnlineConnectionFailed;

        /// <summary>
        /// Raised when a player joins the room
        /// </summary>
        public static event Action<string> OnPlayerJoinedRoom;

        /// <summary>
        /// Raised when a player leaves the room
        /// </summary>
        public static event Action<string> OnPlayerLeftRoom;

        /// <summary>
        /// Raised when the room is ready to start
        /// </summary>
        public static event Action OnRoomReady;

        #endregion

        #region Event Raising Methods

        // Game State Events
        public static void RaiseGameStateChanged(GameState newState) => OnGameStateChanged?.Invoke(newState);
        public static void RaiseGameStarted() => OnGameStarted?.Invoke();
        public static void RaiseGameEnded(GameResult result) => OnGameEnded?.Invoke(result);
        public static void RaisePlayerWon(PlayerType player) => OnPlayerWon?.Invoke(player);
        public static void RaiseGameDraw() => OnGameDraw?.Invoke();

        // Turn Management Events
        public static void RaisePlayerTurnChanged(PlayerType player) => OnPlayerTurnChanged?.Invoke(player);
        public static void RaisePlayerMoved(PlayerType player, int row, int col) => OnPlayerMoved?.Invoke(player, row, col);
        public static void RaiseInvalidMove(string message) => OnInvalidMove?.Invoke(message);

        // Board Events
        public static void RaiseBoardReset() => OnBoardReset?.Invoke();
        public static void RaiseCellMarked(int row, int col, PlayerType player) => OnCellMarked?.Invoke(row, col, player);
        public static void RaiseWinningLineFound(Vector2Int start, Vector2Int end) => OnWinningLineFound?.Invoke(start, end);

        // AI Events
        public static void RaiseAIThinkingStarted() => OnAIThinkingStarted?.Invoke();
        public static void RaiseAIMoveCompleted(int row, int col) => OnAIMoveCompleted?.Invoke(row, col);
        public static void RaiseAIDifficultyChanged(AIDifficulty difficulty) => OnAIDifficultyChanged?.Invoke(difficulty);

        // UI Events
        public static void RaiseUIUpdateRequired() => OnUIUpdateRequired?.Invoke();
        public static void RaiseButtonClicked(string buttonName) => OnButtonClicked?.Invoke(buttonName);
        public static void RaiseSettingsChanged(GameSettings settings) => OnSettingsChanged?.Invoke(settings);

        // Online Multiplayer Events
        public static void RaiseConnectingToOnline() => OnConnectingToOnline?.Invoke();
        public static void RaiseConnectedToOnline() => OnConnectedToOnline?.Invoke();
        public static void RaiseOnlineConnectionFailed(string error) => OnOnlineConnectionFailed?.Invoke(error);
        public static void RaisePlayerJoinedRoom(string playerName) => OnPlayerJoinedRoom?.Invoke(playerName);
        public static void RaisePlayerLeftRoom(string playerName) => OnPlayerLeftRoom?.Invoke(playerName);
        public static void RaiseRoomReady() => OnRoomReady?.Invoke();

        #endregion

        #region Enums

        public enum GameResult
        {
            PlayerXWon,
            PlayerOWon,
            Draw,
            Quit
        }

        public enum PlayerType
        {
            None,
            X,
            O
        }

        public enum AIDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        #endregion
    }
} 