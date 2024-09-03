using Archer.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
    private const string TutorialSceneName = "Tutorial";

    [SerializeField] private InterstitialAd _interstitialAd;

    [SerializeField] private ConfigCurrentLvl _configCurrentLvl;
    [SerializeField] private EquipmentListSO _equipmentListData;
    [SerializeField] private PresenterFactory _factory;
    [SerializeField] private Transform _startPlayerPosition;
    [SerializeField] private Transform _mainPlayerPosition;
    [SerializeField] private Transform _mainEmemyPosition;
    [SerializeField] private List<Transform> _enemiesSpawnPoints;

    [Space]
    [SerializeField] private EndGameWindowView _endGameWindow;
    [SerializeField] private SkillButtonView _skillButtonView;

    [Space]
    [SerializeField] private AudioSource _SFXAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioDataSO _audioData;

    private IGameSession _gameSession;

    private RevardSystem _revardSystem;

    private void Awake()
    {
        if (CheckCurrentScene().name == TutorialSceneName)
        {
            _gameSession = new TutorialGameSession(_factory, _audioData, _startPlayerPosition, _mainPlayerPosition,
            _enemiesSpawnPoints, _mainEmemyPosition, _equipmentListData, _skillButtonView, _configCurrentLvl);
        }
        else
        {
            _gameSession = new GameSession(_factory, _audioData, _startPlayerPosition, _mainPlayerPosition,
    _enemiesSpawnPoints, _mainEmemyPosition, _equipmentListData, _skillButtonView, _configCurrentLvl);
        }

        _revardSystem = new RevardSystem();

        _audioData.Init(_SFXAudioSource, _musicAudioSource);
    }

    private void OnEnable()
    {
        _gameSession.LevelCompete += ShowEndGameWindow;
        _gameSession.EnemyDied += AddRevardOnEnemyKill;
    }

    private void OnDisable()
    {
        _gameSession.LevelCompete -= ShowEndGameWindow;
        _gameSession.EnemyDied -= AddRevardOnEnemyKill;
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

    private void AddRevardOnEnemyKill()
    {
        _revardSystem.AddCoinsOnKill(_configCurrentLvl.CoinsForEnemy);
        _revardSystem.AddScoreOnKill(_configCurrentLvl.ScoreForEnemy);
    }

    private Scene CheckCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }

    private void ShowEndGameWindow(bool isPlayerWin)
    {
        AddCoins();
        AddScore();

        _endGameWindow.SetActiveNextLevelButton(isPlayerWin);
        _endGameWindow.SetAmountCoins(_revardSystem.AmountCoins);
        _endGameWindow.gameObject.SetActive(true);

        _revardSystem.Reset();

#if UNITY_WEBGL && !UNITY_EDITOR
        _interstitialAd.Show();
#endif
    }

    private void AddCoins()
    {
        PlayerData.Instance.Coins += _revardSystem.AmountCoins;
    }

    private void AddScore()
    {
        PlayerData.Instance.Score += _revardSystem.AmountScore;
    }
}