using Agava.YandexGames;
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

    private CharacterStateMachine _playerStateMachine;
    private CharacterStateMachine _enemyStateMachine;

    private CharactersSpawner _playerSpawner;
    private CharactersSpawner _enemySpawner;

    private Weapon _playerWeapon;

    private RevardSystem _revardSystem;

    private Dictionary<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> _enemies;

    private void Awake()
    {
        _enemies = new();
        _revardSystem = new RevardSystem();

        _audioData.Init(_SFXAudioSource, _musicAudioSource);

        _playerSpawner = new PlayerSpawner(_factory, _audioData);
        _enemySpawner = new EnemySpawner(_factory, _audioData);
    }

    private void Start()
    {
        _audioData.Play(Sounds.Game, true);

        CreatePlayer();
        CreateEnemies();

        _playerStateMachine.EnterIn(StatesType.Battle);

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
        Health health = new Health(100);

        ArrowDataSO arrowData = Config.Instance.ArrowConfig;
        WeaponDataSO weaponData = Config.Instance.WeaponConfig;

        _factory.CreatePoolOfPresenters(arrowData.Presenter);

        KeyValuePair<CharacterPresenter, Character> player = _playerSpawner.SpawnCharacter(health, _startPlayerPosition);

        KeyValuePair<WeaponPresenter, Weapon> weapon = _playerSpawner.SpawnWeapon(player.Key, weaponData, arrowData);
        _playerWeapon = weapon.Value;
        _playerWeapon.GetActivatedSkillStatus += _skillButtonView.GetActivatedStatus;
        _playerWeapon.ActivatedSkill += _skillButtonView.ResetButton;

        _playerStateMachine = new CharacterStateMachine(player, weapon, _playerSpawner, _startPlayerPosition.position, _mainPlayerPosition);
        _playerStateMachine.Died += OnPlayerDied;
        _playerStateMachine.EnterIn(StatesType.Idle);
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < 1; i++)
        {
            Health health = new Health(20);

            KeyValuePair<CharacterPresenter, Character> newEnemy = _enemySpawner.SpawnCharacter(health, _enemiesSpawnPoints[i]);
            newEnemy.Key.HitInHead += OnHitInHead;

            KeyValuePair<WeaponPresenter, Weapon> weapon = _enemySpawner.SpawnWeapon(newEnemy.Key, _equipmentListData.WeaponsData[0], _equipmentListData.ArrowsData[0]);
            weapon.Key.gameObject.SetActive(false);

            CharacterStateMachine newStateMachine = new CharacterStateMachine(newEnemy, weapon, _enemySpawner, newEnemy.Value.Position, _mainEmemyPosition);
            newStateMachine.Died += OnEnemyDied;

            newStateMachine.EnterIn(StatesType.Idle);

            _enemies.Add(newEnemy, newStateMachine);
        }
    }

    private void OnEnemyDied()
    {
        _revardSystem.AddCoinsOnKill(Config.Instance.CoinsForEnemy);
        _revardSystem.AddScoreOnKill(Config.Instance.ScoreForEnemt);

        _enemyStateMachine.Character.Key.HitInHead -= OnHitInHead;
        _enemyStateMachine.Died -= OnEnemyDied;

        ActivateNextEnemy();
    }

    private void ActivateNextEnemy()
    {
        if (_enemies.Count == 0)
        {
            _playerStateMachine.EnterIn(StatesType.Idle);
            ShowEndGameWindow();
            return;
        }

        KeyValuePair<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> enemy = _enemies.First();
        _enemyStateMachine = enemy.Value;
        enemy.Value.EnterIn(StatesType.Battle);

        _enemies.Remove(enemy.Key);
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

    private void OnPlayerDied()
    {
        _playerWeapon.GetActivatedSkillStatus -= _skillButtonView.GetActivatedStatus;
        _playerWeapon.ActivatedSkill -= _skillButtonView.ResetButton;
        _playerStateMachine.Died -= OnPlayerDied;
        _enemyStateMachine.EnterIn(StatesType.Idle);
        ShowEndGameWindow();
    }

    private void OnHitInHead()
    {
        _skillButtonView.OnCooldownChanged();
    }
}