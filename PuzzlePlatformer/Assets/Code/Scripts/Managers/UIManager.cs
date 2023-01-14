using Code.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Managers
{
    public class UIManager : BaseManager<UIManager>
    {
        [Header("Home Panel")] [SerializeField]
        private GameObject homePanel;

        [SerializeField] private Button homeQuitButton, homeContinueButton, homeSettingsButton, newGameButton;
        private bool _isHome;

        [Header("Pause Panel")] [SerializeField]
        private GameObject pausePanel;

        [SerializeField] private Button pauseQuitButton;

        private bool _isPaused;

        [Header("Settings Panel")] [SerializeField]
        private GameObject settingsPanel;

        private bool _isSetting;


        [Header("Quit Panel")] [SerializeField]
        private GameObject quitPanel;

        [SerializeField] private Button quitYesButton, quitNoButton;
        private bool _isQuiting;

        private InputReader _inputReader;

        private void Start()
        {
            Cursor.visible = false;

            _inputReader = FindObjectOfType<InputReader>();
            _inputReader.OnPause += ControlPausePanel;

            pauseQuitButton.onClick.AddListener(ControlQuitPanel);

            homeQuitButton.onClick.AddListener(ControlQuitPanel);
            homeContinueButton.onClick.AddListener(ControlHomePanel);
            homeSettingsButton.onClick.AddListener(ControlSettingsPanel);

            quitYesButton.onClick.AddListener(Application.Quit);
            quitNoButton.onClick.AddListener(ControlQuitPanel);
        }

        private void ControlHomePanel()
        {
            homePanel.SetActive(_isHome);
            _isHome = !_isHome;
        }

        private void ControlPausePanel()
        {
            if (!_isHome)
            {
                Time.timeScale = _isPaused ? 0 : 1;
                pausePanel.SetActive(_isPaused);
                _isPaused = !_isPaused;
            }
        }

        public void ControlSettingsPanel()
        {
            settingsPanel.SetActive(_isSetting);
            _isSetting = !_isSetting;
        }

        private void ControlQuitPanel()
        {
            quitPanel.SetActive(_isQuiting);
            _isQuiting = !_isQuiting;
        }
    }
}