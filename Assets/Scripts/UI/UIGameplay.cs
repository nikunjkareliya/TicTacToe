using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <summary>
    /// Controls the Gameplay UI panel.
    /// </summary>
    public class UIGameplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _pauseButton;

        private void Awake()
        {
            if (_pauseButton != null)
                _pauseButton.onClick.AddListener(OnPauseClicked);
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

        private void OnPauseClicked()
        {
            GameEvents.RaiseShowPopup("PausePopup");
        }
    }
} 