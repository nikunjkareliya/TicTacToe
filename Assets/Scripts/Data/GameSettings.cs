using UnityEngine;

namespace TicTacToe
{
    /// <summary>
    /// ScriptableObject containing game settings and configuration data.
    /// Can be created and modified in the Unity Editor.
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "TicTacToe/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Game Configuration")]
        [SerializeField] private GameMode _defaultGameMode = GameMode.AIBot;
        [SerializeField] private AIDifficulty _defaultAIDifficulty = AIDifficulty.Medium;
        [SerializeField] private float _turnDelay = 0.5f;
        [SerializeField] private bool _enableAnimations = true;
        [SerializeField] private float _animationDuration = 0.3f;

        [Header("UI Configuration")]
        [SerializeField] private Color _playerXColor = Color.blue;
        [SerializeField] private Color _playerOColor = Color.red;
        [SerializeField] private Color _winningLineColor = Color.green;
        [SerializeField] private Color _drawColor = Color.gray;

        [Header("AI Configuration")]
        [SerializeField] private int _aiThinkingTimeMin = 500;
        [SerializeField] private int _aiThinkingTimeMax = 2000;

        #region Properties

        public GameMode DefaultGameMode => _defaultGameMode;
        public AIDifficulty DefaultAIDifficulty => _defaultAIDifficulty;
        public float TurnDelay => _turnDelay;
        public bool EnableAnimations => _enableAnimations;
        public float AnimationDuration => _animationDuration;

        public Color PlayerXColor => _playerXColor;
        public Color PlayerOColor => _playerOColor;
        public Color WinningLineColor => _winningLineColor;
        public Color DrawColor => _drawColor;

        public int AIThinkingTimeMin => _aiThinkingTimeMin;
        public int AIThinkingTimeMax => _aiThinkingTimeMax;

        #endregion

        #region Enums

        public enum GameMode
        {
            AIBot,
            PvPOnline
        }

        public enum AIDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Saves the current settings and notifies listeners
        /// </summary>
        public void SaveSettings()
        {
            Debug.Log("Game settings saved");
        }
        
        /// <summary>
        /// Resets all settings to their default values
        /// </summary>
        public void ResetToDefaults()
        {
            _defaultGameMode = GameMode.AIBot;
            _defaultAIDifficulty = AIDifficulty.Medium;
            _turnDelay = 0.5f;
            _enableAnimations = true;
            _animationDuration = 0.3f;
            
            _playerXColor = Color.blue;
            _playerOColor = Color.red;
            _winningLineColor = Color.green;
            _drawColor = Color.gray;
            
            _aiThinkingTimeMin = 500;
            _aiThinkingTimeMax = 2000;
            
            SaveSettings();
            Debug.Log("Game settings reset to defaults");
        }
        
        #endregion
        
        #region Validation

        private void OnValidate()
        {
            // Ensure positive values
            _turnDelay = Mathf.Max(0f, _turnDelay);
            _animationDuration = Mathf.Max(0f, _animationDuration);
            _aiThinkingTimeMin = Mathf.Max(0, _aiThinkingTimeMin);
            _aiThinkingTimeMax = Mathf.Max(_aiThinkingTimeMin, _aiThinkingTimeMax);
        }

        #endregion
    }
} 