using Archer.Model;
using Archer.Model.FSM;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Root : MonoBehaviour
{
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

    private GameSession _gameSession;

    private RevardSystem _revardSystem;

    private void Awake()
    {
        _gameSession = new GameSession(_factory, _audioData, _startPlayerPosition, _mainPlayerPosition,
            _enemiesSpawnPoints, _mainEmemyPosition, _equipmentListData, _skillButtonView);

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

    private void AddRevardOnEnemyKill()
    {
        _revardSystem.AddCoinsOnKill(Config.Instance.CoinsForEnemy);
        _revardSystem.AddScoreOnKill(Config.Instance.ScoreForEnemt);
    }

    private void ShowEndGameWindow()
    {
        AddCoins();
        AddScore();

        _endGameWindow.SetAmountCoins(_revardSystem.AmountCoins);
        _endGameWindow.gameObject.SetActive(true);

        _revardSystem.Reset();
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