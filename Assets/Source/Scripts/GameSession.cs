using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class GameSession
    {
        private const int _maxNumberOfEnemyOnScene = 3;
        private const int _minNumberOfEnemyOnScene = 1;

        private readonly Queue<Presenter> _enemies;
        private readonly Queue<WeaponDataSO> _enemyWeapons;
        private readonly Queue<ArrowDataSO> _enemyArrow;
        private readonly int _numberOfEnemyOnScene;

        private readonly EnemySpawner _enemySpawner;
        private readonly Transform _mainEnemySpawnPoint;
        private readonly List<Transform> _enemySpawnPoints;
        private readonly EquipmentListSO _enemyEquipment;

        private Score _score;

        public event UnityAction<int> AllEnemiesDying;

        public GameSession(EquipmentListSO equipmentList ,EnemySpawner enemySpawner,Transform mainEnemySpawnPoint , List<Transform> enemySpawnPoins)
        {
            _enemyEquipment = equipmentList;
            _enemySpawner = enemySpawner;
            _enemySpawnPoints = enemySpawnPoins;
            _mainEnemySpawnPoint = mainEnemySpawnPoint;

            _enemies = new Queue<Presenter>();
            _enemyWeapons = new Queue<WeaponDataSO>();
            _enemyArrow = new Queue<ArrowDataSO>();

            _score = new Score();

            _numberOfEnemyOnScene = Random.Range(_minNumberOfEnemyOnScene, _maxNumberOfEnemyOnScene + 1);

            SpawnEnemies();
            _enemySpawner.CharacterDying += OnEnemyDying;
        }

        private void SpawnEnemies()
        {
            if (_enemySpawnPoints.Count < _numberOfEnemyOnScene)
                throw new System.ArgumentOutOfRangeException();

            for (int i = 0; i < _numberOfEnemyOnScene; i++)
            {
                Health emenyHealth = new Health(Random.Range(20, 30));
                Presenter currentEnemy = _enemySpawner.SpawnCharacter(emenyHealth, _enemySpawnPoints[i]);
                HealthBarView enemyHealthBarView = currentEnemy.GetComponentInChildren<HealthBarView>();
                enemyHealthBarView.Init(emenyHealth);
                _enemies.Enqueue(currentEnemy);
                
                _enemyWeapons.Enqueue(_enemyEquipment.WeaponsData[Random.Range(0, _enemyEquipment.WeaponsData.Count)]);
                _enemyArrow.Enqueue(_enemyEquipment.ArrowsData[Random.Range(0, _enemyEquipment.ArrowsData.Count)]);
            }
        }

        private void OnEnemyDying()
        {
            _score.AddCoinsOnKill();
            ActivateNextEnemy();
        }

        public void Update(float deltaTime)
        {
            _enemySpawner.Update(deltaTime);
        }

        public void OnDestroy()
        {
            _enemySpawner.OnDestroy();
        }

        public void OnDisable()
        {
            _enemySpawner.OnDisable();
            _enemySpawner.CharacterDying -= OnEnemyDying;
        }

        public void ActivateNextEnemy()
        {
            if(_enemies.Count == 0)
            {
                AllEnemiesDying?.Invoke(_score.AmountCoins);
                return;
            }

            Presenter currentEnemy = _enemies.Dequeue();

            currentEnemy.transform.position = _mainEnemySpawnPoint.position;
            currentEnemy.transform.rotation = _mainEnemySpawnPoint.rotation;

            _enemySpawner.ActivateEnemyAI(currentEnemy.transform.position);
            _enemySpawner.SpawnWeapon(currentEnemy ,_enemyWeapons.Dequeue(), _enemyArrow.Dequeue());

            _enemySpawner.InitWeapon();
        }
    }
}