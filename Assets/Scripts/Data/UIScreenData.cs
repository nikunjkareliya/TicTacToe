using UnityEngine;

/// <summary>
/// Data structure for UI screen information
/// </summary>
namespace TicTacToe
{
    [System.Serializable]
    public class UIScreenData
    {
        [SerializeField] private GameState _gameState;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private string _description;
        [SerializeField] private TransitionType _preferredTransitionType = TransitionType.Fade;

        public GameState GameState => _gameState;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public string Description => _description;
        public TransitionType PreferredTransitionType => _preferredTransitionType;
    }
} 