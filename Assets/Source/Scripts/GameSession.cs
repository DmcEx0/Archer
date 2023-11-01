using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class GameSession : IUpdatetable
    {
        private const int _maxNumberOfEnemyOnScene = 3;
        private const int _minNumberOfEnemyOnScene = 1;

        private readonly int _numberOfEnemyOnScene;

        private readonly EnemySpawner _enemySpawner;
        private readonly Transform _mainEnemySpawnPoint;
        private readonly List<Transform> _enemySpawnPoints;
        private readonly EquipmentListSO _enemyEquipment;
        private readonly Score _score;

        private Dictionary<Presenter, Character> _enemies;
        private Queue<WeaponDataSO> _enemyWeapons;
        private Queue<ArrowDataSO> _enemyArrow;

        private bool _isEnemyTakenPosition = false;

        public event UnityAction<int> AllEnemiesDying;

        public GameSession(EquipmentListSO equipmentList ,EnemySpawner enemySpawner,Transform mainEnemySpawnPoint , List<Transform> enemySpawnPoins, Score score)
        {
            _enemyEquipment = equipmentList;
            _enemySpawner = enemySpawner;
            _enemySpawnPoints = enemySpawnPoins;
            _mainEnemySpawnPoint = mainEnemySpawnPoint;
            _score = score;

            _numberOfEnemyOnScene = Random.Range(_minNumberOfEnemyOnScene, _maxNumberOfEnemyOnScene + 1);

            Init();
        }

        public void Update(float deltaTime)
        {
            if (_isEnemyTakenPosition)
            {
                _enemySpawner.Update(deltaTime);
            }
        }

        public async void WaitForEnemyTakenPosition()
        {
            if (_enemies.Count == 0)
            {
                AllEnemiesDying?.Invoke(_score.AmountCoins);
                return;
            }

            int index = 0;
            KeyValuePair<Presenter, Character> currentEnemy = _enemies.First();
            currentEnemy.Key.AnimationController.PlaySitIdle();

            while (currentEnemy.Value.Position != _mainEnemySpawnPoint.position)
            {
                currentEnemy.Value.MoveTo(currentEnemy.Key.AnimationController.TakenPosition(_enemySpawnPoints[index].position, _mainEnemySpawnPoint.position, Time.deltaTime));

                await Task.Yield();
            }

            ActivateNextEnemy(currentEnemy, currentEnemy.Key.AnimationController);
            _isEnemyTakenPosition = true;
        }

        public void OnDisable()
        {
            _enemySpawner.OnDisable();
            _enemySpawner.CharacterDying -= OnEnemyDying;
        }

        private void ActivateNextEnemy(KeyValuePair<Presenter, Character> nextEnemyTemplate, AnimationController animationController)
        {
            nextEnemyTemplate.Value.Rotate(_mainEnemySpawnPoint.rotation);

            _enemySpawner.ActivateEnemyAI(animationController, nextEnemyTemplate.Value.Position);
            _enemySpawner.SpawnWeapon(nextEnemyTemplate.Key, _enemyWeapons.Dequeue(), _enemyArrow.Dequeue());

            _enemySpawner.InitWeapon();
        }

        private void Init()
        {
            _enemies = new();
            _enemyWeapons = new();
            _enemyArrow = new();

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
                KeyValuePair<Presenter, Character> currentEnemy = _enemySpawner.SpawnCharacter(emenyHealth, _enemySpawnPoints[i]);

                HealthBarView enemyHealthBarView = currentEnemy.Key.GetComponentInChildren<HealthBarView>();
                enemyHealthBarView.Init(emenyHealth);
                _enemies.Add(currentEnemy.Key, currentEnemy.Value);
                
                _enemyWeapons.Enqueue(_enemyEquipment.WeaponsData[/*Random.Range(0, _enemyEquipment.WeaponsData.Count)*/0]);
                _enemyArrow.Enqueue(_enemyEquipment.ArrowsData[Random.Range(0, _enemyEquipment.ArrowsData.Count)]);
            }
        }

        private void OnEnemyDying()
        {
            _enemies.Remove(_enemies.First().Key);
            _score.AddCoinsOnKill(Config.Instance.CoinsForEnemy);

            _isEnemyTakenPosition = false;

            WaitForEnemyTakenPosition();
        }
    }
}