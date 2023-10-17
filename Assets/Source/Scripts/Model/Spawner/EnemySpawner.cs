using Archer.AI;
using Archer.Model;
using UnityEngine;

namespace Archer.Model
{
    public class EnemySpawner : CharactersSpawner
    {
        private EnemyAI _enemyAI; 
        private Vector3 _playerPosition;

        public EnemySpawner(Vector3 playerPosition, PresenterFactory factory) : base(factory)
        {
            _playerPosition = playerPosition;
        }

        protected override Presenter CreateCharacter(Character characterModel) => Factory.CreateEnemy(characterModel);

        protected override Presenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel) => Factory.CreateWeapon(weaponTemplate, weaponModel);

        protected override IInputRouter GetInputRouter() => new EnemyInputRouter(_enemyAI);

        public void ActivateEnemyAI(Vector3 enemyPosition)
        {
            Vector3 direction = enemyPosition - _playerPosition;

            _enemyAI = new EnemyAI(direction.magnitude);
        }
    }
}