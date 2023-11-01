using UnityEngine;

namespace Archer.Model
{
    public class PlayerSpawner : CharactersSpawner
    {
        private AnimationController _animationController;

        public PlayerSpawner(PresenterFactory factory) : base(factory)
        {
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            EnabledIK(_animationController);
        }

        protected override Presenter CreateCharacter(Character characterModel)
        {
            Presenter playerTempalte = Factory.CreatePlayer(characterModel);

            _animationController = playerTempalte.AnimationController;

            return playerTempalte;
        }

        protected override WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel) => Factory.CreateWeapon(weaponTemplate, weaponModel) as WeaponPresenter;

        protected override IInputRouter GetInputRouter() => new PlayerInputRouter(_animationController);
    }
}