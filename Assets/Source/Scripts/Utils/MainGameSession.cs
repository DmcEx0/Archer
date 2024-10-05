using System.Collections.Generic;
using System.Linq;
using Archer.Audio;
using Archer.Data;
using Archer.Model;
using Archer.Model.FSM;
using Archer.Presenters;
using Archer.UI;
using Archer.Utils;
using UnityEngine;

public class MainGameSession : BaseGameSession
{
    public MainGameSession(PresenterFactory presenterFactory, AudioDataConfig audioData, Transform startPlayerPos, Transform mainPlayerPos, List<Transform> enemySpawnPoints, Transform mainEnemyPos, EquipmentListSO equipmentListData, SkillButtonView skillButtonView, CurrentLevelConfig currentLevelConfig) : base(presenterFactory, audioData, startPlayerPos, mainPlayerPos, enemySpawnPoints, mainEnemyPos, equipmentListData, skillButtonView, currentLevelConfig)
    {
    }

    protected override void ConfigEnemies()
    {
        for (int i = 0; i < EnemiesSpawnPoints.Count; i++)
        {
            int randomIndexArrow = Random.Range(0, EquipmentListData.ArrowsData.Count);
            int randomIndexWeapon = Random.Range(0, EquipmentListData.WeaponsData.Count);

            int healthValue = Random.Range(CurrentLevelConfig.MinHealthEnemy, CurrentLevelConfig.MaxHealthEnemy);
            Health health = new(healthValue);

            KeyValuePair<CharacterPresenter, Character> newEnemy =
                Enemy.SpawnCharacter(health, EnemiesSpawnPoints[i]);
            newEnemy.Key.GettingHitInHead += OnGettingHitInHead;

            KeyValuePair<WeaponPresenter, Weapon> weapon = Enemy.SpawnWeapon(newEnemy.Key,
                EquipmentListData.WeaponsData[randomIndexWeapon], EquipmentListData.ArrowsData[randomIndexArrow]);
            weapon.Key.gameObject.SetActive(false);

            CharacterStateMachine newStateMachine = new CharacterStateMachine(newEnemy, weapon, Enemy,
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

    protected override void OnPlayerDie()
    {
        IsPlayerVictory = false;

        Unsubscribe();

        EnemyStateMachine.EnterIn(StatesType.Idle);
        LevelCompeted?.Invoke(IsPlayerVictory);
    }
}
