using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Model
{
    public abstract class CharactersSpawner
    {
        private Weapon _weaponModel;
        private Character _characterModel;
        private Presenter _characterTemplate;
        private WeaponPresenter _weaponTemplate;

        private ArrowDataSO _arrowData;

        protected CharactersSpawner(PresenterFactory factory)
        {
            Factory = factory;
        }

        public PresenterFactory Factory { get; private set; }

        protected abstract Presenter CreateCharacter(Character caharacterModel);
        protected abstract WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel);

        public KeyValuePair<Presenter, Character> SpawnCharacter(Health health, Transform characterSpawnPoint)
        {
            _characterModel = new Character(characterSpawnPoint.position, characterSpawnPoint.rotation, health);
            _characterTemplate = CreateCharacter(_characterModel);

            KeyValuePair<Presenter, Character> character = new KeyValuePair<Presenter, Character>(_characterTemplate, _characterModel);

            return character;
        }

        public KeyValuePair<WeaponPresenter, Weapon> SpawnWeapon(Presenter tempalte, WeaponDataSO weaponData, ArrowDataSO arrowData)
        {
            _arrowData = arrowData;

            Factory.CreatePoolOfPresenters(arrowData.Presenter);

            _weaponModel = new Weapon(CastToGeneratable(tempalte).GeneratingPoint.position, CastToGeneratable(tempalte).GeneratingPoint.rotation, weaponData.SpeedChangedAngle, weaponData.ShotPower, weaponData.Cooldown);
            _weaponTemplate = CreateWeapon(weaponData.Presenter as WeaponPresenter, _weaponModel);
            _weaponTemplate.transform.SetParent(CastToGeneratable(tempalte).GeneratingPoint);

            KeyValuePair<WeaponPresenter, Weapon> weapon = new KeyValuePair<WeaponPresenter, Weapon>(_weaponTemplate, _weaponModel);

            return weapon;
        }

        private IGeneratable CastToGeneratable(Presenter presenter)
        {
            if (presenter is IGeneratable)
            {
                return presenter as IGeneratable;
            }
            else
                throw new InvalidOperationException(nameof(presenter));
        }

        public void OnShot(Arrow arrow)
        {
            Factory.CreateArrow(_arrowData.Presenter, arrow);

            arrow.SetDamage(_arrowData.Damage);
        }
    }
}