using Archer.Model.FSM;
using Archer.Model;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class GameSession
{
    private PresenterFactory _presenterFactory;
    private AudioDataSO _audioData;

    private Transform _startPlayerPosition;
    private Transform _mainPlayerPosition;

    private List<Transform> _enemiesSpawnPoints;
    private Transform _mainEnemyPosition;

    private CharacterStateMachine _playerStateMachine;
    private CharacterStateMachine _enemyStateMachine;

    private CharactersSpawner _playerSpawner;
    private CharactersSpawner _enemySpawner;

    private Dictionary<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> _enemies;

    private Weapon _playerWeapon;
    private EquipmentListSO _equipmentListData;

    private SkillButtonView _skillButtonView;

    public event UnityAction EnemyDied;
    public event UnityAction LevelCompete;

    public GameSession(PresenterFactory presenterFactory, AudioDataSO audioData, Transform startPlayerPos, Transform mainPlayerPos, List<Transform> enemySpawnPoints, Transform mainEnemyPos, EquipmentListSO equipmentListData, SkillButtonView skillButtonView)
    {
        _presenterFactory = presenterFactory;
        _audioData = audioData;
        _startPlayerPosition = startPlayerPos;
        _mainPlayerPosition = mainPlayerPos;
        _enemiesSpawnPoints = enemySpawnPoints;
        _mainEnemyPosition = mainEnemyPos;
        _equipmentListData = equipmentListData;
        _skillButtonView = skillButtonView;
    }

    public void Init()
    {
        _enemies = new();

        _playerSpawner = new PlayerSpawner(_presenterFactory, _audioData);
        _enemySpawner = new EnemySpawner(_presenterFactory, _audioData);

        CreatePlayer();
        CreateEnemies();

        _playerStateMachine.EnterIn(StatesType.Battle);

        ActivateNextEnemy();

    }

    public void Update()
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
        Health health = new Health(200);

        ArrowDataSO arrowData = Config.Instance.ArrowConfig;
        WeaponDataSO weaponData = Config.Instance.WeaponConfig;

        _presenterFactory.CreatePoolOfPresenters(arrowData.Presenter);

        KeyValuePair<CharacterPresenter, Character> player = _playerSpawner.SpawnCharacter(health, _startPlayerPosition);

        KeyValuePair<WeaponPresenter, Weapon> weapon = _playerSpawner.SpawnWeapon(player.Key, weaponData, arrowData);
        weapon.Key.gameObject.SetActive(false);

        _playerWeapon = weapon.Value;
        _playerWeapon.GetActivatedSkillStatus += _skillButtonView.GetActivatedStatus;
        _playerWeapon.ActivatedSkill += _skillButtonView.ResetButton;

        _playerStateMachine = new CharacterStateMachine(player, weapon, _playerSpawner, _startPlayerPosition.position, _mainPlayerPosition);
        _playerStateMachine.Died += OnPlayerDied;
        _playerStateMachine.EnterIn(StatesType.Idle);
    }

    private void CreateEnemies()
    {
        int minHealthValue = 60;
        int maxHealthValue = 70;

        for (int i = 0; i < 3; i++)
        {
            int healthValue = Random.Range(minHealthValue, maxHealthValue);
            Health health = new Health(healthValue);

            KeyValuePair<CharacterPresenter, Character> newEnemy = _enemySpawner.SpawnCharacter(health, _enemiesSpawnPoints[i]);
            newEnemy.Key.HitInHead += OnHitInHead;

            KeyValuePair<WeaponPresenter, Weapon> weapon = _enemySpawner.SpawnWeapon(newEnemy.Key, _equipmentListData.WeaponsData[0], _equipmentListData.ArrowsData[0]);
            weapon.Key.gameObject.SetActive(false);

            CharacterStateMachine newStateMachine = new CharacterStateMachine(newEnemy, weapon, _enemySpawner, newEnemy.Value.Position, _mainEnemyPosition);
            newStateMachine.Died += OnEnemyDied;

            newStateMachine.EnterIn(StatesType.Idle);

            _enemies.Add(newEnemy, newStateMachine);
        }
    }

    private void ActivateNextEnemy()
    {
        if (_enemies.Count == 0)
        {
            _playerStateMachine.EnterIn(StatesType.Idle);
            LevelCompete?.Invoke();
            PlayerData.Instance.CompleteLevel();

            return;
        }

        KeyValuePair<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> enemy = _enemies.First();
        _enemyStateMachine = enemy.Value;
        enemy.Value.EnterIn(StatesType.Battle);

        _enemies.Remove(enemy.Key);
    }

    private void OnEnemyDied()
    {
        EnemyDied?.Invoke();

        _enemyStateMachine.Character.Key.HitInHead -= OnHitInHead;
        _enemyStateMachine.Died -= OnEnemyDied;

        ActivateNextEnemy();
    }

    private void OnPlayerDied()
    {
        _playerWeapon.GetActivatedSkillStatus -= _skillButtonView.GetActivatedStatus;
        _playerWeapon.ActivatedSkill -= _skillButtonView.ResetButton;
        _playerStateMachine.Died -= OnPlayerDied;
        _enemyStateMachine.EnterIn(StatesType.Idle);
        LevelCompete?.Invoke();
    }

    private void OnHitInHead()
    {
        _skillButtonView.OnCooldownChanged();
    }
}