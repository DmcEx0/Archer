using System;
using System.Collections.Generic;
using Archer.Audio;
using Archer.Data;
using Archer.Presenters;
using UnityEngine;

namespace Archer.Model
{
    public abstract class CharacterFactory
    {
        private readonly AudioDataConfig _audioData;
        
        private Weapon _weaponModel;
        private Character _characterModel;
        private CharacterPresenter _characterTemplate;
        private WeaponPresenter _weaponTemplate;

        private ArrowDataConfig _arrowData;

        protected CharacterFactory(PresenterFactory factory, AudioDataConfig audioData)
        {
            Factory = factory;
            _audioData = audioData;
        }

        protected PresenterFactory Factory { get; private set; }

        protected abstract Presenter CreateCharacter(Character characterModel);
        protected abstract WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel);

        public KeyValuePair<CharacterPresenter, Character> SpawnCharacter(Health health, Transform characterSpawnPoint)
        {
            _characterModel = new Character(characterSpawnPoint.position, characterSpawnPoint.rotation, health);
            _characterTemplate = CreateCharacter(_characterModel) as CharacterPresenter;

            KeyValuePair<CharacterPresenter, Character> character =
                new KeyValuePair<CharacterPresenter, Character>(_characterTemplate, _characterModel);

            return character;
        }

        public KeyValuePair<WeaponPresenter, Weapon> SpawnWeapon(Presenter tempalte, WeaponDataConfig weaponData,
            ArrowDataConfig arrowData)
        {
            _arrowData = arrowData;

            Factory.CreatePoolOfPresenters(arrowData.Presenter);

            _weaponModel = new Weapon(CastToGeneratable(tempalte).GeneratingPoint.position,
                CastToGeneratable(tempalte).GeneratingPoint.rotation, weaponData.SpeedChangedAngle,
                weaponData.ShotPower, weaponData.Cooldown);
            _weaponTemplate = CreateWeapon(weaponData.Presenter as WeaponPresenter, _weaponModel);
            _weaponTemplate.transform.SetParent(CastToGeneratable(tempalte).GeneratingPoint);

            KeyValuePair<WeaponPresenter, Weapon> weapon =
                new KeyValuePair<WeaponPresenter, Weapon>(_weaponTemplate, _weaponModel);

            return weapon;
        }

        private IGeneratable CastToGeneratable(Presenter presenter)
        {
            if (presenter is IGeneratable)
            {
                return presenter as IGeneratable;
            }
            else
            {
                throw new InvalidOperationException(nameof(presenter));
            }
        }

        public void OnShot(Arrow arrow)
        {
            Factory.CreateArrow(_arrowData.Presenter, arrow);

            arrow.SetDamage(_arrowData.MainDamage, _arrowData.AdditionalDamage, _arrowData.SkillType);

            if (arrow.SkillType == ArrowSkillType.None)
                AudioHandler.Instance.Play(Sounds.Shot1);
            else
                AudioHandler.Instance.Play(Sounds.Shot2);
        }
    }
}