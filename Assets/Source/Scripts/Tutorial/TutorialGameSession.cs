using Archer.Model.FSM;
using Archer.Model;
using System.Collections.Generic;
using System.Linq;
using Archer.Audio;
using Archer.Data;
using Archer.Presenters;
using Archer.UI;
using Archer.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Archer.Tutor
{
    public class TutorialGameSession : BaseGameSession
    {
        public TutorialGameSession(PresenterFactory presenterFactory, AudioDataConfig audioData, Transform startPlayerPos, Transform mainPlayerPos, List<Transform> enemySpawnPoints, Transform mainEnemyPos, EquipmentListSO equipmentListData, SkillButtonView skillButtonView, CurrentLevelConfig currentLevelConfig) : base(presenterFactory, audioData, startPlayerPos, mainPlayerPos, enemySpawnPoints, mainEnemyPos, equipmentListData, skillButtonView, currentLevelConfig)
        {
        }

        protected override void ConfigEnemies()
        {
            int defaultArrowIndex = 0;
            int defaultWeaponIndex = 0;

            for (int i = 0; i < EnemiesSpawnPoints.Count; i++)
            {
                int healthValue = Random.Range(CurrentLevelConfig.MinHealthEnemy, CurrentLevelConfig.MaxHealthEnemy);
                Health health = new(healthValue);

                KeyValuePair<CharacterPresenter, Character> newEnemy =
                    EnemyFactory.SpawnCharacter(health, EnemiesSpawnPoints[i]);
                newEnemy.Key.GettingHitInHead += OnGettingHitInHead;

                KeyValuePair<WeaponPresenter, Weapon> weapon = EnemyFactory.SpawnWeapon(newEnemy.Key,
                    EquipmentListData.WeaponsData[defaultWeaponIndex], EquipmentListData.ArrowsData[defaultArrowIndex]);
                weapon.Key.gameObject.SetActive(false);

                CharacterStateMachine newStateMachine = new CharacterStateMachine(newEnemy, weapon, EnemyFactory,
                    newEnemy.Value.Position, MainEnemyPosition);
                newStateMachine.Died += OnEnemyDied;

                newStateMachine.EnterIn(StatesType.Idle);

                Enemies.Add(newEnemy, newStateMachine);
            }
        }
        
        protected override void ActiveEnem()
        {
            if (Enemies.Count == 0)
            {
                IsPlayerVictory = true;

                Unsubscribe();

                PlayerStateMachine.EnterIn(StatesType.Idle);
                PlayerData.Instance.TutorialIsComplete = true;
                LevelCompeted?.Invoke(IsPlayerVictory);
                return;
            }

            KeyValuePair<KeyValuePair<CharacterPresenter, Character>, CharacterStateMachine> enemy = Enemies.First();
            EnemyStateMachine = enemy.Value;
            enemy.Value.EnterIn(StatesType.Battle);

            Enemies.Remove(enemy.Key);
        }

        protected override void OnPlayerDie()
        {
            IsPlayerVictory = false;

            Unsubscribe();

            EnemyStateMachine.EnterIn(StatesType.Idle);
            LevelCompeted?.Invoke(IsPlayerVictory);

            PlayerData.Instance.TutorialIsComplete = false;
        }
    }
}