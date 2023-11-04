using Archer.Model;
using Assets.Source.Scripts.FSM.States;
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

    private CharacterStateMachine _playerStateMachine;
    private CharacterStateMachine _enemyStateMachine;

    private CharactersSpawner _playerSpawner;
    private CharactersSpawner _enemySpawner;

    private Score _score;

    private Dictionary<KeyValuePair<Presenter, Character>, CharacterStateMachine> _enemies;

    private void Awake()
    {
        _enemies = new();
        _score = new Score();

        _playerSpawner = new PlayerSpawner(_factory);
        _enemySpawner = new EnemySpawner(_factory);
    }

    private void Start()
    {
        CreatePlayer();
        CreateEnemies();
        ActivateNextEnemy();
    }

    private void Update()
    {
        _playerStateMachine.Update(Time.deltaTime);
        _enemyStateMachine.Update(Time.deltaTime);

        foreach (var stateMachine in _enemies)
        {
            stateMachine.Value.Update(Time.deltaTime);
        }
    }

    private void CreatePlayer()
    {
        Health health = new Health(1000);

        ArrowDataSO arrowData = Config.Instance.ArrowConfig;
        WeaponDataSO weaponData = Config.Instance.WeaponConfig;

        _factory.CreatePoolOfPresenters(arrowData.Presenter);

        KeyValuePair<Presenter, Character> player = _playerSpawner.SpawnCharacter(health, _startPlayerPosition);

        KeyValuePair<WeaponPresenter, Weapon> weapon = _playerSpawner.SpawnWeapon(player.Key, weaponData, arrowData);

        _playerStateMachine = new CharacterStateMachine(player, weapon, _playerSpawner, _startPlayerPosition.position, _mainPlayerPosition);

        _playerStateMachine.EnterIn(StatesEnum.Battle);
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < _enemiesSpawnPoints.Count; i++)
        {
            Health health = new Health(20);
            KeyValuePair<Presenter, Character> newEnemy = _enemySpawner.SpawnCharacter(health, _enemiesSpawnPoints[i]);

            KeyValuePair<WeaponPresenter, Weapon> weapon = _enemySpawner.SpawnWeapon(newEnemy.Key, _equipmentListData.WeaponsData[0], _equipmentListData.ArrowsData[0]);
            weapon.Key.gameObject.SetActive(false);

            CharacterStateMachine newStateMachine = new CharacterStateMachine(newEnemy, weapon, _enemySpawner, newEnemy.Value.Position, _mainEmemyPosition);
            newStateMachine.Died += OnEnemyDied;
            newStateMachine.EnterIn(StatesEnum.Idle);
            _enemies.Add(newEnemy, newStateMachine);
        }
    }

    private void OnEnemyDied()
    {
        _score.AddCoinsOnKill(Config.Instance.CoinsForEnemy);
        _enemyStateMachine.Died -= OnEnemyDied;

        ActivateNextEnemy();
    }

    private void ActivateNextEnemy()
    {
        if (_enemies.Count == 0)
        {
            ShowEndGameWindow();
            return;
        }

        KeyValuePair<KeyValuePair<Presenter, Character>, CharacterStateMachine> enemy = _enemies.First();
        _enemyStateMachine = enemy.Value;
        enemy.Value.EnterIn(StatesEnum.Battle);

        _enemies.Remove(enemy.Key);
    }

    private void ShowEndGameWindow()
    {
        AddScore(_score.AmountCoins);
        _endGameWindow.SetAmountCoins(_score.AmountCoins);
        _endGameWindow.gameObject.SetActive(true);
    }

    private void AddScore(int coins)
    {
        PlayerData.Instance.Coins += coins;
    }
}