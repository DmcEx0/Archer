using Archer.AI;
using Archer.Animations;
using Archer.Model;

namespace Archer.Input
{
    public class EnemyInputRouter : IInputRouter
    {
        private const float MaxPower = 3f;
        
        private readonly EnemyAI _enemyAI;
        private readonly AnimationController _animationController;
        
        private Weapon _weapon;

        public EnemyInputRouter(EnemyAI enemyAI, AnimationController animationController)
        {
            _enemyAI = enemyAI;
            _animationController = animationController;
        }

        public void BindWeapon(Weapon weapon)
        {
            _weapon = weapon;

            _enemyAI.SetStartVelocityOfFirstShoot(MaxPower * _weapon.StartedPowerOfShot);
        }

        public void CanGainingPower(bool isCanNot)
        {
        }

        public void Update(float deltaTime)
        {
            _enemyAI.CheckTargetInDirection(_weapon.ArrowSpawnPosition, _weapon.Forward,
                _weapon.Rotation.eulerAngles.x);
        }

        public void OnEnable()
        {
            _enemyAI.Shot += Shoot;
        }

        public void OnDisable()
        {
            _enemyAI.Shot -= Shoot;
        }

        private void Shoot()
        {
            TryShoot(_weapon, MaxPower);
        }

        private void TryShoot(Weapon weapon, float power)
        {
            if (weapon == null)
                throw new System.NullReferenceException(nameof(weapon));

            if (weapon.CanShoot)
            {
                weapon.Shoot(power);

                _animationController.PlayShoot(weapon.Cooldown);
                _enemyAI.SetTarget();
            }
        }
    }
}