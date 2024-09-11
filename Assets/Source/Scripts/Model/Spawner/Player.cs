using Archer.Audio;
using Archer.Presenters;

namespace Archer.Model
{
    public class Player : CharacterSpawner
    {
        public Player(PresenterFactory factory, AudioDataConfig audioData) : base(factory, audioData)
        {
        }

        protected override Presenter CreateCharacter(Character characterModel) => Factory.CreatePlayer(characterModel);

        protected override WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel) =>
            Factory.CreateWeapon(weaponTemplate, weaponModel) as WeaponPresenter;
    }
}