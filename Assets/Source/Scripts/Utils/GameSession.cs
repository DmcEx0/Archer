using System;
using Archer.Model.FSM;
using Archer.Model;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Archer.Audio;
using Archer.Data;
using Archer.Presenters;
using Archer.UI;
using Random = UnityEngine.Random;

namespace Archer.Utils
{
    public class GameSession : IGameSession
    {
        private readonly CurrentLevelConfig _currentLevelConfig;

        private readonly PresenterFactory _presenterFactory;
        private readonly AudioDataConfig _audioData;

        private readonly Transform _startPlayerPosition;
        private readonly Transform _mainPlayerPosition;

        private readonly List<Transform> _enemiesSpawnPoints;
        private readonly Transform _mainEnemyPosition;
        
        private readonly EquipmentListSO _equipmentListData;
        private readonly SkillButtonView _skillButtonView;

        private CharacterStateMachine _playerStateMachine;
        private CharacterStateMachine _enemyStateMachine;

        private CharacterSpawner _player;
        private CharacterSpawner _enemy;

        private Dictionary<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> _enemies;

        private Weapon _playerWeapon;

        private bool _isPlayerVictory;

        public event Action EnemyDied;
        public event Action<bool> LevelCompeted;

        public GameSession(PresenterFactory presenterFactory, AudioDataConfig audioData, Transform startPlayerPos,
            Transform mainPlayerPos, List<Transform> enemySpawnPoints, Transform mainEnemyPos,
            EquipmentListSO equipmentListData, SkillButtonView skillButtonView, CurrentLevelConfig currentLevelConfig)
        {
            _presenterFactory = presenterFactory;
            _audioData = audioData;
            _startPlayerPosition = startPlayerPos;
            _mainPlayerPosition = mainPlayerPos;
            _enemiesSpawnPoints = enemySpawnPoints;
            _mainEnemyPosition = mainEnemyPos;
            _equipmentListData = equipmentListData;
            _skillButtonView = skillButtonView;
            _currentLevelConfig = currentLevelConfig;
        }

        public void Init()
        {
            _enemies = new();

            _player = new Player(_presenterFactory, _audioData);
            _enemy = new Enemy(_presenterFactory, _audioData);

            ConfigurePlayer();
            ConfigureEnemies();

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

        public void OnExitGame()
        {
            _playerStateMachine.EnterIn(StatesType.Idle);
        }

        private void ConfigurePlayer()
        {
            Health health = new(_currentLevelConfig.PlayerHealth);

            WeaponDataConfig weaponData =
                _equipmentListData.WeaponsData.FirstOrDefault(w => w.ID == PlayerData.Instance.CrossbowID);
            ArrowDataConfig arrowData =
                _equipmentListData.ArrowsData.FirstOrDefault(a => a.ID == PlayerData.Instance.ArrowID);

            _presenterFactory.CreatePoolOfPresenters(arrowData.Presenter);

            KeyValuePair<CharacterPresenter, Character> player =
                _player.SpawnCharacter(health, _startPlayerPosition);

            KeyValuePair<WeaponPresenter, Weapon>
                weapon = _player.SpawnWeapon(player.Key, weaponData, arrowData);
            weapon.Key.gameObject.SetActive(false);

            _playerWeapon = weapon.Value;
            _playerWeapon.GetActivatedSkillStatus += _skillButtonView.GetActivatedStatus;
            _playerWeapon.ActivatingSkill += _skillButtonView.OnResetButton;

            _skillButtonView.OnUIPressing += _playerWeapon.GetUIPressStatus;

            _playerStateMachine = new CharacterStateMachine(player, weapon, _player,
                _startPlayerPosition.position, _mainPlayerPosition);
            _playerStateMachine.Died += OnPlayerDied;
            _playerStateMachine.EnterIn(StatesType.Idle);
        }

        private void ConfigureEnemies()
        {
            for (int i = 0; i < _enemiesSpawnPoints.Count; i++)
            {
                int randomIndexArrow = Random.Range(0, _equipmentListData.ArrowsData.Count);
                int randomIndexWeapon = Random.Range(0, _equipmentListData.WeaponsData.Count);

                int healthValue = Random.Range(_currentLevelConfig.MinHealthEnemy, _currentLevelConfig.MaxHealthEnemy);
                Health health = new(healthValue);

                KeyValuePair<CharacterPresenter, Character> newEnemy =
                    _enemy.SpawnCharacter(health, _enemiesSpawnPoints[i]);
                newEnemy.Key.GettingHitInHead += OnGettingHitInHead;

                KeyValuePair<WeaponPresenter, Weapon> weapon = _enemy.SpawnWeapon(newEnemy.Key,
                    _equipmentListData.WeaponsData[randomIndexWeapon], _equipmentListData.ArrowsData[randomIndexArrow]);
                weapon.Key.gameObject.SetActive(false);

                CharacterStateMachine newStateMachine = new CharacterStateMachine(newEnemy, weapon, _enemy,
                    newEnemy.Value.Position, _mainEnemyPosition);
                newStateMachine.Died += OnEnemyDied;

                newStateMachine.EnterIn(StatesType.Idle);

                _enemies.Add(newEnemy, newStateMachine);
            }
        }

        private void ActivateNextEnemy()
        {
            if (_enemies.Count == 0)
            {
                _isPlayerVictory = true;

                _playerStateMachine.EnterIn(StatesType.Idle);
                LevelCompeted?.Invoke(_isPlayerVictory);
                PlayerData.Instance.CompleteLevel();

                Unsubscribe();

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

            _enemyStateMachine.Character.Key.GettingHitInHead -= OnGettingHitInHead;
            _enemyStateMachine.Died -= OnEnemyDied;

            ActivateNextEnemy();
        }

        private void OnPlayerDied()
        {
            _isPlayerVictory = false;

            Unsubscribe();

            _enemyStateMachine.EnterIn(StatesType.Idle);
            LevelCompeted?.Invoke(_isPlayerVictory);
        }

        private void Unsubscribe()
        {
            _skillButtonView.OnUIPressing -= _playerWeapon.GetUIPressStatus;
            _playerWeapon.GetActivatedSkillStatus -= _skillButtonView.GetActivatedStatus;
            _playerWeapon.ActivatingSkill -= _skillButtonView.OnResetButton;
            _playerStateMachine.Died -= OnPlayerDied;
        }

        private void OnGettingHitInHead()
        {
            _skillButtonView.OnCooldownChanged();
        }
    }
}