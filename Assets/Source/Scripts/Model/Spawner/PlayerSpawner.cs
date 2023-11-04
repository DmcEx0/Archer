using UnityEngine;

namespace Archer.Model
{
    public class PlayerSpawner : CharactersSpawner
    {
        public PlayerSpawner(PresenterFactory factory) : base(factory)
        {
        }

        protected override Presenter CreateCharacter(Character characterModel) => Factory.CreatePlayer(characterModel);

        protected override WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel) => Factory.CreateWeapon(weaponTemplate, weaponModel) as WeaponPresenter;
    }
}