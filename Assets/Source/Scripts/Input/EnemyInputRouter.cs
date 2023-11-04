using Archer.AI;
using Archer.Model;
using UnityEngine;

public class EnemyInputRouter : IInputRouter
{
    private EnemyAI _enemyAI;
    private Weapon _weapon;
    private AnimationController _animationController;

    private float _maxPower = 3f;

    public EnemyInputRouter(EnemyAI enemyAI, AnimationController animationController)
    {
        _enemyAI = enemyAI;
        _animationController = animationController;
    }

    public IInputRouter BindWeapon(Weapon weapon)
    {
        _weapon = weapon;

        _enemyAI.SetStartVelocityOfFirstShoot(_maxPower * _weapon.StartedPowerOfShot);

        return this;
    }

    public void Update(float deltaTime)
    {
        _enemyAI.CheckTargetInDirection(_weapon.ArrowSpawnPosition, _weapon.Forward);
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
        TryShoot(_weapon, _maxPower);
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