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
    public abstract class BaseGameSession : IGameSession
    {
        private readonly PresenterFactory _presenterFactory;
        private readonly AudioDataConfig _audioData;

        private readonly Transform _startPlayerPosition;
        private readonly Transform _mainPlayerPosition;

        private readonly SkillButtonView _skillButtonView;

        private CharacterFactory _playerFactory;

        private Weapon _playerWeapon;

        protected readonly CurrentLevelConfig CurrentLevelConfig;
        protected readonly EquipmentListSO EquipmentListData;
        protected readonly List<Transform> EnemiesSpawnPoints;
        protected readonly Transform MainEnemyPosition;

        protected CharacterStateMachine PlayerStateMachine { get; private set; }

        protected Dictionary<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> Enemies
        {
            get; private set;
        }

        protected CharacterStateMachine EnemyStateMachine { get; set; }
        protected CharacterFactory EnemyFactory { get; private set; }
        protected bool IsPlayerVictory { get; set; }

        public Action<bool> LevelCompeted { get; set; }
        public event Action EnemyDied;

        public BaseGameSession(PresenterFactory presenterFactory, AudioDataConfig audioData, Transform startPlayerPos,
            Transform mainPlayerPos, List<Transform> enemySpawnPoints, Transform mainEnemyPos,
            EquipmentListSO equipmentListData, SkillButtonView skillButtonView, CurrentLevelConfig currentLevelConfig)
        {
            _presenterFactory = presenterFactory;
            _audioData = audioData;
            _startPlayerPosition = startPlayerPos;
            _mainPlayerPosition = mainPlayerPos;
            EnemiesSpawnPoints = enemySpawnPoints;
            MainEnemyPosition = mainEnemyPos;
            EquipmentListData = equipmentListData;
            _skillButtonView = skillButtonView;
            CurrentLevelConfig = currentLevelConfig;
        }

        public void Init()
        {
            Enemies = new();

            _playerFactory = new PlayerFactory(_presenterFactory, _audioData);
            EnemyFactory = new EnemyFactory(_presenterFactory, _audioData);

            ConfigurePlayer();
            ConfigureEnemies();

            PlayerStateMachine.EnterIn(StatesType.Battle);

            ActivateNextEnemy();
        }

        public void Update()
        {
            PlayerStateMachine.Update(Time.deltaTime);
            EnemyStateMachine.Update(Time.deltaTime);

            foreach (var stateMachine in Enemies)
            {
                stateMachine.Value.Update(Time.deltaTime);
            }
        }

        public void OnExitGame()
        {
            PlayerStateMachine.EnterIn(StatesType.Idle);
        }

        protected void OnGettingHitInHead()
        {
            _skillButtonView.OnCooldownChanged();
        }

        protected void OnEnemyDied()
        {
            EnemyDied?.Invoke();

            EnemyStateMachine.Character.Key.GettingHitInHead -= OnGettingHitInHead;
            EnemyStateMachine.Died -= OnEnemyDied;

            ActivateNextEnemy();
        }

        protected void Unsubscribe()
        {
            _skillButtonView.OnUIPressing -= _playerWeapon.GetUIPressStatus;
            _playerWeapon.GetActivatedSkillStatus -= _skillButtonView.GetActivatedStatus;
            _playerWeapon.ActivatingSkill -= _skillButtonView.OnResetButton;
            PlayerStateMachine.Died -= OnPlayerDied;
        }

        private void ConfigurePlayer()
        {
            Health health = new(CurrentLevelConfig.PlayerHealth);

            WeaponDataConfig weaponData =
                EquipmentListData.WeaponsData.FirstOrDefault(w => w.ID == PlayerData.Instance.CrossbowID);
            ArrowDataConfig arrowData =
                EquipmentListData.ArrowsData.FirstOrDefault(a => a.ID == PlayerData.Instance.ArrowID);

            _presenterFactory.CreatePoolOfPresenters(arrowData.Presenter);

            KeyValuePair<CharacterPresenter, Character> player =
                _playerFactory.SpawnCharacter(health, _startPlayerPosition);

            KeyValuePair<WeaponPresenter, Weapon>
                weapon = _playerFactory.SpawnWeapon(player.Key, weaponData, arrowData);
            weapon.Key.gameObject.SetActive(false);

            _playerWeapon = weapon.Value;
            _playerWeapon.GetActivatedSkillStatus += _skillButtonView.GetActivatedStatus;
            _playerWeapon.ActivatingSkill += _skillButtonView.OnResetButton;

            _skillButtonView.OnUIPressing += _playerWeapon.GetUIPressStatus;

            PlayerStateMachine = new CharacterStateMachine(player, weapon, _playerFactory,
                _startPlayerPosition.position, _mainPlayerPosition);
            PlayerStateMachine.Died += OnPlayerDied;
            PlayerStateMachine.EnterIn(StatesType.Idle);
        }

        protected abstract void ConfigEnemies();

        private void ConfigureEnemies()
        {
            for (int i = 0; i < EnemiesSpawnPoints.Count; i++)
            {
                int randomIndexArrow = Random.Range(0, EquipmentListData.ArrowsData.Count);
                int randomIndexWeapon = Random.Range(0, EquipmentListData.WeaponsData.Count);

                int healthValue = Random.Range(CurrentLevelConfig.MinHealthEnemy, CurrentLevelConfig.MaxHealthEnemy);
                Health health = new(healthValue);

                KeyValuePair<CharacterPresenter, Character> newEnemy =
                    EnemyFactory.SpawnCharacter(health, EnemiesSpawnPoints[i]);
                newEnemy.Key.GettingHitInHead += OnGettingHitInHead;

                KeyValuePair<WeaponPresenter, Weapon> weapon = EnemyFactory.SpawnWeapon(newEnemy.Key,
                    EquipmentListData.WeaponsData[randomIndexWeapon], EquipmentListData.ArrowsData[randomIndexArrow]);
                weapon.Key.gameObject.SetActive(false);

                CharacterStateMachine newStateMachine = new CharacterStateMachine(newEnemy, weapon, EnemyFactory,
                    newEnemy.Value.Position, MainEnemyPosition);
                newStateMachine.Died += OnEnemyDied;

                newStateMachine.EnterIn(StatesType.Idle);

                Enemies.Add(newEnemy, newStateMachine);
            }
        }

        protected abstract void ActiveEnem();

        private void ActivateNextEnemy()
        {
            if (Enemies.Count == 0)
            {
                IsPlayerVictory = true;

                PlayerStateMachine.EnterIn(StatesType.Idle);
                LevelCompeted?.Invoke(IsPlayerVictory);
                PlayerData.Instance.CompleteLevel();

                Unsubscribe();

                return;
            }

            KeyValuePair<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> enemy = Enemies.First();
            EnemyStateMachine = enemy.Value;
            enemy.Value.EnterIn(StatesType.Battle);

            Enemies.Remove(enemy.Key);
        }

        protected abstract void OnPlayerDie();
        
        private void OnPlayerDied()
        {
            IsPlayerVictory = false;

            Unsubscribe();

            EnemyStateMachine.EnterIn(StatesType.Idle);
            LevelCompeted?.Invoke(IsPlayerVictory);
        }
    }
}