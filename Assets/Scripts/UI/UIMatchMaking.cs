using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <summary>
    /// Controls the MatchMaking UI panel.
    /// </summary>
    public class UIMatchMaking : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _cancelButton;

        private void Awake()
        {
            if (_cancelButton != null)
                _cancelButton.onClick.AddListener(OnCancelClicked);
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

        private void OnCancelClicked()
        {
            GameEvents.RaiseGameStateChanged(GameState.MainMenu);
        }
    }
} 