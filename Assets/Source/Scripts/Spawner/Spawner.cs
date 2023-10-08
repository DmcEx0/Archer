using Archer.Model;
using System;
using UnityEngine;

public abstract class Spawner
{
    private Weapon _weapon;
    private Character _character;

    private Presenter _weaponTemplate;
    private Presenter _characterTemplate;

    private IInputRouter _inputRouter;

    private ArrowConfigSO _arrowConfig;

    protected Spawner(PresenterFactory factory)
    {
        Factory = factory;
    }

    public Presenter CharacterTemplate => _characterTemplate;

    protected Character Character => _character;
    protected PresenterFactory Factory { get; private set; }

    protected abstract Presenter CreateCharacter(Character caharacterModel);
    protected abstract Presenter CreateWeapon(WeaponPresenter weaponTemplate ,Weapon weaponModel);
    protected abstract IInputRouter GetInputRouter();

    public void Update(float deltaTime)
    {
        _inputRouter.Update(deltaTime);
    }

    public void Spawn(Health health, WeaponConfigSO weaponConfig, ArrowConfigSO arrowConfig, Transform point)
    {
        _arrowConfig = arrowConfig;

        _character = new Character(point.position, point.rotation, health);
        _characterTemplate = CreateCharacter(_character);

        _weapon = new Weapon(CastToGeneratable(_characterTemplate).SpawnPoint.position, CastToGeneratable(_characterTemplate).SpawnPoint.rotation, weaponConfig.SpeedChangedAngle, weaponConfig.ShotPower, weaponConfig.Cooldown);
        _weaponTemplate = CreateWeapon(weaponConfig.Presenter ,_weapon);

        _weaponTemplate.gameObject.SetActive(true);

        Init();
    }

    private void Init()
    {
        _weapon.Init(_weaponTemplate.transform);
        _inputRouter = GetInputRouter().BindWeapon(_weapon);

        _weapon.Shot += OnShot;

        _character.Died += OnDestroy;

        _inputRouter.OnEnable();
    }

    private void OnDestroy()
    {
        _weapon.OnDestroy();
        _character.Destroy();
        _inputRouter.OnDisable();

        _character.Died -= OnDestroy;
        _weapon.Shot -= OnShot;
    }

    private void OnShot(Arrow arrow)
    {
        Factory.CreateArrow(_arrowConfig.Presenter, arrow);

        arrow.Init(CastToGeneratable(_weaponTemplate).SpawnPoint.position, Quaternion.Euler(0f, 0f, _weaponTemplate.transform.eulerAngles.x), _arrowConfig.Damage);
    }

    private IGeneratable CastToGeneratable(Presenter presenter)
    {
        if (presenter is IGeneratable)
        {
            return presenter as IGeneratable;
        }
        else
            throw new InvalidOperationException(nameof (presenter));
    }
}