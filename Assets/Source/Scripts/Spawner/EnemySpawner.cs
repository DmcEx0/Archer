using Archer.AI;
using Archer.Model;
using UnityEngine;

public class EnemySpawner : Spawner
{
    private EnemyAI _enemyAI;
    private Vector3 _playerPosition;

    public EnemySpawner(Vector3 playerPosition ,PresenterFactory factory) : base(factory)
    {
        _playerPosition = playerPosition;
    }

    protected override Presenter CreateCharacter(Character characterModel)
    {
        Presenter presenter = Factory.CreateEnemy(characterModel);

        _enemyAI = new EnemyAI(GetCalculatedDistanceToPlayer(_playerPosition));

        return presenter;
    }

    protected override Presenter CreateWeapon(WeaponPresenter weaponTemplate ,Weapon weaponModel)
    {
        return Factory.CreateWeapon(weaponTemplate ,weaponModel);
    }

    protected override IInputRouter GetInputRouter() => new EnemyInputRouter(_enemyAI);

    private float GetCalculatedDistanceToPlayer(Vector3 playerPosition)
    {
        Vector3 direction = Character.Position - playerPosition;

        return direction.magnitude;
    }
}