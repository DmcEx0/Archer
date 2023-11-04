using Archer.AI;
using UnityEngine;

namespace Archer.Model
{
    public class EnemySpawner : CharactersSpawner
    {
        public EnemySpawner(PresenterFactory factory) : base(factory)
        {
        }

        protected override Presenter CreateCharacter(Character characterModel) => Factory.CreateEnemy(characterModel);

        protected override WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel) => Factory.CreateWeapon(weaponTemplate, weaponModel) as WeaponPresenter;
    }
}