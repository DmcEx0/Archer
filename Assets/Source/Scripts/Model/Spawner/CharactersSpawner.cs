using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public abstract class CharactersSpawner : IUpdatetable
    {
        private Weapon _weaponModel;
        private Character _characterModel;
        private WeaponPresenter _weaponTemplate;

        private IInputRouter _inputRouter;

        private ArrowDataSO _arrowData;

        protected CharactersSpawner(PresenterFactory factory)
        {
            Factory = factory;
        }

        public event UnityAction CharacterDying;
        public IInputRouter InputRouter => _inputRouter;
        protected Character CurrentCharacter => _characterModel;
        protected Weapon CurrentWeapon => _weaponModel;
        protected PresenterFactory Factory { get; private set; }

        protected abstract Presenter CreateCharacter(Character caharacterModel);
        protected abstract WeaponPresenter CreateWeapon(WeaponPresenter weaponTemplate, Weapon weaponModel);
        protected abstract IInputRouter GetInputRouter();

        public virtual void Update(float deltaTime)
        {
            _inputRouter.Update(deltaTime);
        }

        public KeyValuePair<Presenter, Character> SpawnCharacter(Health health, Transform characterSpawnPoint)
        {
            _characterModel = new Character(characterSpawnPoint.position, characterSpawnPoint.rotation, health);
            Presenter characterTemplate = CreateCharacter(_characterModel);

            _characterModel.Died += OnDying;

            KeyValuePair<Presenter, Character> character = new KeyValuePair<Presenter, Character>(characterTemplate, _characterModel);

            return character;
        }

        public void SpawnWeapon(Presenter tempalte, WeaponDataSO weaponData, ArrowDataSO arrowData)
        {
            _arrowData = arrowData;

            Factory.CreatePoolOfPresenters(arrowData.Presenter);

            _weaponModel = new Weapon(CastToGeneratable(tempalte).GeneratingPoint.position, CastToGeneratable(tempalte).GeneratingPoint.rotation, weaponData.SpeedChangedAngle, weaponData.ShotPower, weaponData.Cooldown);
            _weaponTemplate = CreateWeapon(weaponData.Presenter as WeaponPresenter, _weaponModel);
            _weaponModel.SetArrowSpawnPoint(CastToGeneratable(tempalte).GeneratingPoint.position);
            _weaponTemplate.transform.SetParent(CastToGeneratable(tempalte).GeneratingPoint);
        }

        public void InitWeapon()
        {
            _inputRouter = GetInputRouter().BindWeapon(_weaponModel);

            _weaponModel.Shoted += OnShot;

            _inputRouter.OnEnable();
        }

        public void EnabledIK(AnimationController animationController)
        {
            animationController.SetTargetsForHands(_weaponTemplate.RightHandTarget, _weaponTemplate.LeftHandTarget, _weaponTemplate.ChestTarget);
        }

        public void OnDisable()
        {
            OnDisableCharacter();

            _characterModel.Died -= OnDying;
        }

        private void OnDisableCharacter()
        {
            _weaponModel.Destroy();
            _inputRouter.OnDisable();

            _weaponModel.Shoted -= OnShot;
        }

        private void OnShot(Arrow arrow)
        {
            Factory.CreateArrow(_arrowData.Presenter, arrow);

            arrow.Init(CastToGeneratable(_weaponTemplate).GeneratingPoint.position, Quaternion.Euler(0f, 0f, _weaponTemplate.transform.eulerAngles.x), _arrowData.Damage);
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

        private void OnDying()
        {
            OnDisableCharacter();
            CharacterDying?.Invoke();
        }
    }
}