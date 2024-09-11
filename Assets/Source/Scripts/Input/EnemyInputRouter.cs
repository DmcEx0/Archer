using Archer.AI;
using Archer.Animations;
using Archer.Model;

namespace Archer.Input
{
    public class EnemyInputRouter : IInputRouter
    {
        private const float MaxPower = 3f;
        
        private readonly EnemyAI _enemyAI;
        private readonly AnimationHandler _animationHandler;
        
        private Weapon _weapon;

        public EnemyInputRouter(EnemyAI enemyAI, AnimationHandler animationHandler)
        {
            _enemyAI = enemyAI;
            _animationHandler = animationHandler;
        }

        public void BindWeapon(Weapon weapon)
        {
            _weapon = weapon;

            _enemyAI.SetStartVelocityOfFirstShoot(MaxPower * _weapon.StartedPowerOfShot);
        }

        public void SetGainingPowerState(bool isCanNot)
        {
        }

        public void Update(float deltaTime)
        {
            _enemyAI.CheckTargetInDirection(_weapon.ArrowSpawnPosition, _weapon.Forward,
                _weapon.Rotation.eulerAngles.x);
        }

        public void OnEnable()
        {
            _enemyAI.Shot += OnShoot;
        }

        public void OnDisable()
        {
            _enemyAI.Shot -= OnShoot;
        }

        private void OnShoot()
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

                _animationHandler.PlayShoot(weapon.Cooldown);
                _enemyAI.ConfigureTarget();
            }
        }
    }
}