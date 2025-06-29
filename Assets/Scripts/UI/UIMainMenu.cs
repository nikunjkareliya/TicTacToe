using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the Main Menu UI panel.
/// </summary>
namespace TicTacToe
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        private void Awake()
        {
            if (_playButton != null)
                _playButton.onClick.AddListener(OnPlayClicked);
            if (_settingsButton != null)
                _settingsButton.onClick.AddListener(OnSettingsClicked);
            if (_quitButton != null)
                _quitButton.onClick.AddListener(OnQuitClicked);
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

        private void OnPlayClicked()
        {
            GameEvents.RaiseGameStateChanged(GameState.MatchMaking);
        }

        private void OnSettingsClicked()
        {
            GameEvents.RaiseShowPopup("SettingsPopup");
        }

        private void OnQuitClicked()
        {
            Application.Quit();
        }
    }
} 