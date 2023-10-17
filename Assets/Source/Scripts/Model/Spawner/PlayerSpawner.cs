namespace Archer.Model
{
    public class PlayerSpawner : CharactersSpawner
    {
        public PlayerSpawner(PresenterFactory factory) : base(factory)
        {
        }

        protected override Presenter CreateCharacter(Character characterModel) => Factory.CreatePlayer(characterModel);

        protected override Presenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel) => Factory.CreateWeapon(weaponTemplate, weaponModel);

        protected override IInputRouter GetInputRouter() => new PlayerInputRouter();
    }
}