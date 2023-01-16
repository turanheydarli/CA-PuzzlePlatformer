using System;
using System.Collections;
using Code.Scripts.Common;
using Code.Scripts.StateMachines.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        [Header("Home Panel")] [SerializeField]
        private GameObject homePanel;

        [SerializeField] private Button homeQuitButton, homeContinueButton, homeSettingsButton, newGameButton;
        private bool _isHome = true;

        [Header("Pause Panel")] [SerializeField]
        private GameObject pausePanel;

        [SerializeField] private Button pauseQuitButton;

        private static bool _isPaused;

        [Header("Settings Panel")] [SerializeField]
        private GameObject settingsPanel;

        private bool _isSetting;

        [Header("Quit Panel")] [SerializeField]
        private GameObject quitPanel;

        [SerializeField] private Button quitYesButton, quitNoButton;
        private bool _isQuiting;
        private InputReader _inputReader;

        [Header("Loading Panel")] [SerializeField]
        private GameObject loadingPanel;

        private bool _isLoading = true;

        [SerializeField] private GameObject[] parchments;
        private int _openIndex = 0;

        [Header("Game Panel")] [SerializeField]
        private TMP_Text strawberryCountText, healthCountText, chestCountText;

        private PlayerStateMachine _playerStateMachine;

        private void Start()
        {
            Load(5f);
            ControlHomePanel();

            Cursor.visible = false;

            _inputReader = FindObjectOfType<InputReader>();
            _inputReader.OnPause += ControlPausePanel;
            _playerStateMachine = FindObjectOfType<PlayerStateMachine>();

            _playerStateMachine.CollectableDetector.OnCollectableDetect += HandleCollectableDetect;

            pauseQuitButton.onClick.AddListener(ControlQuitPanel);

            homeQuitButton.onClick.AddListener(ControlQuitPanel);
            homeContinueButton.onClick.AddListener(ControlHomePanel);
            homeSettingsButton.onClick.AddListener(ControlSettingsPanel);
            newGameButton.onClick.AddListener(StartNewGame);
            quitYesButton.onClick.AddListener(Application.Quit);
            quitNoButton.onClick.AddListener(ControlQuitPanel);
        }

        private void HandleCollectableDetect(Transform obj)
        {
            strawberryCountText.text = (_playerStateMachine.StrawberryCount + 1) + "x";
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

        private void ControlSettingsPanel()
        {
            settingsPanel.SetActive(_isSetting);
            _isSetting = !_isSetting;
        }

        private void ControlQuitPanel()
        {
            quitPanel.SetActive(_isQuiting);
            _isQuiting = !_isQuiting;
        }

        private void ControlLoadingPanel()
        {
            loadingPanel.SetActive(_isLoading);
            _isLoading = !_isLoading;
        }

        public void StartNewGame()
        {
            ControlHomePanel();
            Load(5f);
            ESDataManager.Instance.Reset();
            SceneManager.LoadSceneAsync(0);
        }

        public void Load(float lifetime)
        {
            StartCoroutine(LoadingAnimation(lifetime));
        }

        public void ShowParchment()
        {
            DOTween.Sequence().Join(parchments[_openIndex].transform.DOScale(0, 0))
                .AppendCallback(() => { parchments[_openIndex].SetActive(true); })
                .Join(parchments[_openIndex].transform.DOScale(1, 1.5f))
                .InsertCallback(3, () => { parchments[_openIndex].SetActive(false); })
                .OnComplete(() => { _openIndex++; });
        }

        private IEnumerator LoadingAnimation(float lifetime)
        {
            ControlLoadingPanel();
            yield return new WaitForSeconds(lifetime);
            ControlLoadingPanel();
        }

        public void SetHealth(int health)
        {
            healthCountText.text = health + "x";
        }
        
        public void SetChest(int chestCount)
        {
            chestCountText.text = chestCount + "x";
        }
    }
}