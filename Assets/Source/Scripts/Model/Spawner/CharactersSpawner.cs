using System;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public abstract class CharactersSpawner
    {
        private Weapon _weapon;
        private Character _character;

        private Presenter _weaponTemplate;
        private Presenter _characterTemplate;

        private IInputRouter _inputRouter;

        private ArrowDataSO _arrowData;

        protected CharactersSpawner(PresenterFactory factory)
        {
            Factory = factory;
        }

        public Presenter CharacterTemplate => _characterTemplate;//???
        public IInputRouter InputRouter => _inputRouter;
        protected PresenterFactory Factory { get; private set; }

        public UnityAction CharacterDying;

        protected abstract Presenter CreateCharacter(Character caharacterModel);
        protected abstract Presenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel);
        protected abstract IInputRouter GetInputRouter();

        public void OnDisable()
        {
            _character.Died -= OnDying;
        }

        public void OnDestroy()
        {
            WeaponDestroy();
        }

        public void Update(float deltaTime)
        {
            _inputRouter.Update(deltaTime);
        }

        public Presenter SpawnCharacter(Health health, Transform characterSpawnPoint)
        {
            _character = new Character(characterSpawnPoint.position, characterSpawnPoint.rotation, health);
            _characterTemplate = CreateCharacter(_character);

            _character.Died += OnDying;

            return _characterTemplate;
        }

        public void SpawnWeapon(Presenter tempalte, WeaponDataSO weaponData, ArrowDataSO arrowData)
        {
            _arrowData = arrowData;

            Factory.CreatePoolOfPresenters(arrowData.Presenter);

            _weapon = new Weapon(CastToGeneratable(tempalte).SpawnPoint.position, CastToGeneratable(tempalte).SpawnPoint.rotation, weaponData.SpeedChangedAngle, weaponData.ShotPower, weaponData.Cooldown);
            _weaponTemplate = CreateWeapon(weaponData.Presenter as WeaponPresenter, _weapon);

            _weaponTemplate.gameObject.SetActive(true);
        }

        public void InitWeapon()
        {
            _weapon.Init(_weaponTemplate.transform);
            _inputRouter = GetInputRouter().BindWeapon(_weapon);

            _weapon.Shot += OnShot;

            _inputRouter.OnEnable();
        }

        private void WeaponDestroy() //???????
        {
            _weapon.OnDestroy();
        }

        private void OnDying()
        {
            _weapon.OnDestroy();
            _inputRouter.OnDisable();

            _weapon.Shot -= OnShot;

            CharacterDying?.Invoke();
        }

        private void OnShot(Arrow arrow)
        {
            Factory.CreateArrow(_arrowData.Presenter, arrow);

            arrow.Init(CastToGeneratable(_weaponTemplate).SpawnPoint.position, Quaternion.Euler(0f, 0f, _weaponTemplate.transform.eulerAngles.x), _arrowData.Damage);
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
    }
}