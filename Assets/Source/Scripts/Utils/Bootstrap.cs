using Archer.Model;
using System.Collections.Generic;
using Archer.Audio;
using Archer.Data;
using Archer.Presenters;
using Archer.Tutor;
using Archer.UI;
using Archer.Yandex;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archer.Utils
{
    public class Bootstrap : MonoBehaviour
    {
        private const string TutorialSceneName = "Tutorial";

        [SerializeField] private InterstitialAd _interstitialAd;

        [SerializeField] private CurrentLevelConfig _currentLevelConfig;
        [SerializeField] private EquipmentListSO _equipmentListData;
        [SerializeField] private PresenterFactory _factory;
        [SerializeField] private Transform _startPlayerPosition;
        [SerializeField] private Transform _mainPlayerPosition;
        [SerializeField] private Transform _mainEnemyPosition;
        [SerializeField] private List<Transform> _enemiesSpawnPoints;

        [Space] [SerializeField] private EndGameWindowView _endGameWindow;
        [SerializeField] private SkillButtonView _skillButtonView;

        [Space] 
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioDataConfig _audioData;

        private IGameSession _gameSession;

        private RewardSystem _rewardSystem;

        private void Awake()
        {
            if (CheckCurrentScene().name == TutorialSceneName)
            {
                _gameSession = new TutorialGameSession(_factory, _audioData, _startPlayerPosition, _mainPlayerPosition,
                    _enemiesSpawnPoints, _mainEnemyPosition, _equipmentListData, _skillButtonView, _currentLevelConfig);
            }
            else
            {
                _gameSession = new MainGameSession(_factory, _audioData, _startPlayerPosition, _mainPlayerPosition,
                    _enemiesSpawnPoints, _mainEnemyPosition, _equipmentListData, _skillButtonView, _currentLevelConfig);
            }

            _rewardSystem = new RewardSystem();

            _audioData.Init(_sfxAudioSource, _musicAudioSource);
        }

        private void OnEnable()
        {
            _gameSession.LevelCompeted += OnShowEndGameWindow;
            _gameSession.EnemyDied += OnAddRewardOnEnemyKill;
        }

        private void OnDisable()
        {
            _gameSession.LevelCompeted -= OnShowEndGameWindow;
            _gameSession.EnemyDied -= OnAddRewardOnEnemyKill;
        }

        private void Start()
        {
            _audioData.Play(Sounds.Game);
            _gameSession.Init();
        }

        private void Update()
        {
            _gameSession.Update();
        }

        public void OnExitGame()
        {
            _gameSession.OnExitGame();
        }

        private void OnAddRewardOnEnemyKill()
        {
            _rewardSystem.AddCoinsOnKill(_currentLevelConfig.CoinsForEnemy);
            _rewardSystem.AddScoreOnKill(_currentLevelConfig.ScoreForEnemy);
        }

        private Scene CheckCurrentScene()
        {
            return SceneManager.GetActiveScene();
        }

        private void OnShowEndGameWindow(bool isPlayerWin)
        {
            AddCoins();
            AddScore();

            _endGameWindow.SetActiveNextLevelButton(isPlayerWin);
            _endGameWindow.SetAmountCoins(_rewardSystem.AmountCoins);
            _endGameWindow.gameObject.SetActive(true);

            _rewardSystem.Reset();

#if UNITY_WEBGL && !UNITY_EDITOR
        _interstitialAd.Show();
#endif
        }

        private void AddCoins()
        {
            var currentCoins = PlayerData.Instance.Coins + _rewardSystem.AmountCoins;
            PlayerData.Instance.SetCoins(currentCoins);
        }

        private void AddScore()
        {
            var currentScore = PlayerData.Instance.Score + _rewardSystem.AmountScore;
            PlayerData.Instance.SetScore(currentScore);
        }
    }
}