using Archer.AI;
using Archer.Model;
using System;
using UnityEngine;

public class EnemyInputRouter : IInputRouter
{
    private EnemyAI _enemyAI;
    private float _maxPower = 3f;

    public EnemyInputRouter(EnemyAI enemyAI)
    {
        _enemyAI = enemyAI;
    }

    private Weapon _weapon;

    public IInputRouter BindWeapon(Weapon weapon)
    {
        _weapon = weapon;

        _enemyAI.SetStartVelocityOfFirstShoot(_maxPower * _weapon.StartedPowerOfShot);

        return this;
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
        if(weapon == null)
            throw new InvalidOperationException();


        if(weapon.CanShoot)
            weapon.Shoot(power);
    }

    public void Update(float deltaTime)
    {
        _enemyAI.TargetDetection(_weapon.Position, -_weapon.Forward);
    }
}