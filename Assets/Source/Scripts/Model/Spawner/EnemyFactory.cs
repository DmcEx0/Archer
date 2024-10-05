using Archer.Audio;
using Archer.Presenters;

namespace Archer.Model
{
    public class EnemyFactory : CharacterFactory
    {
        public EnemyFactory(PresenterFactory factory, AudioDataConfig audioData) : base(factory, audioData)
        {
        }

        protected override Presenter CreateCharacter(Character characterModel) => Factory.CreateEnemy(characterModel);

        protected override WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel) => Factory.CreateWeapon(weaponTemplate, weaponModel) as WeaponPresenter;
    }
}