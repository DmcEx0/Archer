using Archer.Model.FSM;
using Archer.Model;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Archer.Audio;
using Archer.Data;
using Archer.Presenters;
using Archer.UI;
using UnityEngine.Events;

namespace Archer.Utils
{
    public class GameSession : IGameSession
    {
        private readonly ConfigCurrentLvl _configCurrentLvl;

        private readonly PresenterFactory _presenterFactory;
        private readonly AudioDataSO _audioData;

        private readonly Transform _startPlayerPosition;
        private readonly Transform _mainPlayerPosition;

        private readonly List<Transform> _enemiesSpawnPoints;
        private readonly Transform _mainEnemyPosition;
        
        private readonly EquipmentListSO _equipmentListData;
        private readonly SkillButtonView _skillButtonView;

        private CharacterStateMachine _playerStateMachine;
        private CharacterStateMachine _enemyStateMachine;

        private CharactersSpawner _playerSpawner;
        private CharactersSpawner _enemySpawner;

        private Dictionary<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> _enemies;

        private Weapon _playerWeapon;

        private bool _isPlayerVictory;

        public event UnityAction EnemyDied;
        public event UnityAction<bool> LevelCompete;

        public GameSession(PresenterFactory presenterFactory, AudioDataSO audioData, Transform startPlayerPos,
            Transform mainPlayerPos, List<Transform> enemySpawnPoints, Transform mainEnemyPos,
            EquipmentListSO equipmentListData, SkillButtonView skillButtonView, ConfigCurrentLvl configCurrentLvl)
        {
            _presenterFactory = presenterFactory;
            _audioData = audioData;
            _startPlayerPosition = startPlayerPos;
            _mainPlayerPosition = mainPlayerPos;
            _enemiesSpawnPoints = enemySpawnPoints;
            _mainEnemyPosition = mainEnemyPos;
            _equipmentListData = equipmentListData;
            _skillButtonView = skillButtonView;
            _configCurrentLvl = configCurrentLvl;
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

        public void OnExitGame()
        {
            _playerStateMachine.EnterIn(StatesType.Idle);
        }

        private void CreatePlayer()
        {
            Health health = new(_configCurrentLvl.PlayerHealth);

            WeaponDataSO weaponData =
                _equipmentListData.WeaponsData.FirstOrDefault(w => w.ID == PlayerData.Instance.CrossbowID);
            ArrowDataSO arrowData =
                _equipmentListData.ArrowsData.FirstOrDefault(a => a.ID == PlayerData.Instance.ArrowID);

            _presenterFactory.CreatePoolOfPresenters(arrowData.Presenter);

            KeyValuePair<CharacterPresenter, Character> player =
                _playerSpawner.SpawnCharacter(health, _startPlayerPosition);

            KeyValuePair<WeaponPresenter, Weapon>
                weapon = _playerSpawner.SpawnWeapon(player.Key, weaponData, arrowData);
            weapon.Key.gameObject.SetActive(false);

            _playerWeapon = weapon.Value;
            _playerWeapon.GetActivatedSkillStatus += _skillButtonView.GetActivatedStatus;
            _playerWeapon.ActivatedSkill += _skillButtonView.ResetButton;

            _skillButtonView.OnUIPressed += _playerWeapon.GetUIPressStatus;

            _playerStateMachine = new CharacterStateMachine(player, weapon, _playerSpawner,
                _startPlayerPosition.position, _mainPlayerPosition);
            _playerStateMachine.Died += OnPlayerDied;
            _playerStateMachine.EnterIn(StatesType.Idle);
        }

        private void CreateEnemies()
        {
            for (int i = 0; i < _enemiesSpawnPoints.Count; i++)
            {
                int randomIndexArrow = Random.Range(0, _equipmentListData.ArrowsData.Count);
                int randomIndexWeapon = Random.Range(0, _equipmentListData.WeaponsData.Count);

                int healthValue = Random.Range(_configCurrentLvl.MinHealthEnemy, _configCurrentLvl.MaxHealthEnemy);
                Health health = new(healthValue);

                KeyValuePair<CharacterPresenter, Character> newEnemy =
                    _enemySpawner.SpawnCharacter(health, _enemiesSpawnPoints[i]);
                newEnemy.Key.HitInHead += OnHitInHead;

                KeyValuePair<WeaponPresenter, Weapon> weapon = _enemySpawner.SpawnWeapon(newEnemy.Key,
                    _equipmentListData.WeaponsData[randomIndexWeapon], _equipmentListData.ArrowsData[randomIndexArrow]);
                weapon.Key.gameObject.SetActive(false);

                CharacterStateMachine newStateMachine = new CharacterStateMachine(newEnemy, weapon, _enemySpawner,
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
                LevelCompete?.Invoke(_isPlayerVictory);
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

            _enemyStateMachine.Character.Key.HitInHead -= OnHitInHead;
            _enemyStateMachine.Died -= OnEnemyDied;

            ActivateNextEnemy();
        }

        private void OnPlayerDied()
        {
            _isPlayerVictory = false;

            Unsubscribe();

            _enemyStateMachine.EnterIn(StatesType.Idle);
            LevelCompete?.Invoke(_isPlayerVictory);
        }

        private void Unsubscribe()
        {
            _skillButtonView.OnUIPressed -= _playerWeapon.GetUIPressStatus;
            _playerWeapon.GetActivatedSkillStatus -= _skillButtonView.GetActivatedStatus;
            _playerWeapon.ActivatedSkill -= _skillButtonView.ResetButton;
            _playerStateMachine.Died -= OnPlayerDied;
        }

        private void OnHitInHead()
        {
            _skillButtonView.OnCooldownChanged();
        }
    }
}