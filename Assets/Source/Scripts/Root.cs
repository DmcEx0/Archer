using Archer.Model;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private EquipmentListSO _equipmentListData;
    [SerializeField] private PresenterFactory _factory;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Transform _mainEmemyPosition;
    [SerializeField] private List<Transform> _enemiesSpawnPoints;

    [Space]
    [SerializeField] private EndGameWindowView _loseGameWindow;
    [SerializeField] private EndGameWindowView _winGameWindow;

    private PlayerSpawner _playerSpawner;
    private EnemySpawner _enemySpawner;

    private Health _playerHealth;

    private GameSession _gameSession;

    public void OnEnable()
    {
        _playerSpawner.CharacterDying += ShowLoseGameWindow;
        _gameSession.AllEnemiesDying += ShowWinGameWindow;
    }

    private void OnDisable()
    {
        _playerSpawner.CharacterDying -= ShowLoseGameWindow;
        _gameSession.AllEnemiesDying -= ShowWinGameWindow;

        _playerSpawner.OnDisable();
        _gameSession.OnDisable();
    }

    private void Awake()
    {
        _playerSpawner = new PlayerSpawner(_factory);
        _enemySpawner = new EnemySpawner(_playerPosition.position, _factory);

        _playerHealth = new Health(200);
        _gameSession = new GameSession(_equipmentListData, _enemySpawner, _mainEmemyPosition, _enemiesSpawnPoints);
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        _playerSpawner.Update(Time.deltaTime);
        _gameSession.Update(Time.deltaTime);
    }

    private void Initialize()
    {
        ArrowDataSO arrowData = Config.Instance.ArrowConfig;
        WeaponDataSO weaponData = Config.Instance.WeaponConfig;

        var playerTemplate = _playerSpawner.SpawnCharacter(_playerHealth, _playerPosition);
        _playerSpawner.SpawnWeapon(playerTemplate, weaponData, arrowData);
        _playerSpawner.InitWeapon();
        _factory.CreatePoolOfPresenters(arrowData.Presenter);

        HealthBarView playerHealthBar = _playerSpawner.CharacterTemplate.GetComponentInChildren<HealthBarView>();
        playerHealthBar.Init(_playerHealth);

        PowerShotBarView powerShotPresenter = _playerSpawner.CharacterTemplate.GetComponentInChildren<PowerShotBarView>();
        powerShotPresenter.Init(_playerSpawner.InputRouter as PlayerInputRouter);

        _gameSession.ActivateNextEnemy();
    }

    private void ShowLoseGameWindow()
    {
        _loseGameWindow.gameObject.SetActive(true);
    }
    private void ShowWinGameWindow(int coins)
    {
        AddScore(coins);
        _winGameWindow.gameObject.SetActive(true);
    }

    private void AddScore(int coins)
    {
        PlayerData.Instance.Coins += coins;
    }
}