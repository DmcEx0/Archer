using Archer.AI;
using UnityEngine;

namespace Archer.Model
{
    public class EnemySpawner : CharactersSpawner
    {
        private EnemyAI _enemyAI; 
        private AnimationController _animationController;

        private Vector3 _playerPosition;

        public EnemySpawner(Vector3 playerPosition, PresenterFactory factory) : base(factory)
        {
            _playerPosition = playerPosition;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            EnabledIK(_animationController);
        }

        protected override Presenter CreateCharacter(Character characterModel) => Factory.CreateEnemy(characterModel);

        protected override WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel) => Factory.CreateWeapon(weaponTemplate, weaponModel) as WeaponPresenter;

        protected override IInputRouter GetInputRouter() => new EnemyInputRouter(_enemyAI, _animationController);

        public void ActivateEnemyAI(AnimationController animationController ,Vector3 enemyPosition)
        {
            _animationController = animationController;

            Vector3 direction = enemyPosition - _playerPosition;

            _enemyAI = new EnemyAI(direction.magnitude);
        }
    }
}