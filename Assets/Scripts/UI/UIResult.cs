using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the Result UI panel.
/// </summary>
namespace TicTacToe
{
    public class UIResult : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _playAgainButton;

        private void Awake()
        {
            if (_mainMenuButton != null)
                _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            if (_playAgainButton != null)
                _playAgainButton.onClick.AddListener(OnPlayAgainClicked);
        }

        public void Show()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }
        }

        public void Hide()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }
        }

        private void OnMainMenuClicked()
        {
            GameEvents.RaiseGameStateChanged(GameState.MainMenu);
        }

        private void OnPlayAgainClicked()
        {
            GameEvents.RaiseGameStateChanged(GameState.MatchMaking);
        }
    }
} 