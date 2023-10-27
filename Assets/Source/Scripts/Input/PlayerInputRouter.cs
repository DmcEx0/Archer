using Archer.Model;
using UnityEngine;
using System;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.Events;

public class PlayerInputRouter : IInputRouter
{
    private AnimationController _animationController;

    private readonly PlayerInput _input;

    private readonly float _maxPower = 3f;
    private readonly float _minPower = 1f;

    private Weapon _weapon;

    private float _power = 1;

    public PlayerInputRouter(AnimationController animationController)
    {
        _input = new PlayerInput();
        _animationController = animationController;
    }

    public event UnityAction<float, float> PowerChanged;

    public void OnEnable()
    {
        _input.Enable();

        _input.Player.Shoot.canceled += Shoot;
    }

    public void OnDisable()
    {
        _input.Disable();
        _input.Player.Shoot.canceled -= Shoot;
    }

    public IInputRouter BindWeapon(Weapon weapon)
    {
        _weapon = weapon;
        return this;
    }

    private void Shoot(CallbackContext ctx)
    { 
        float power = (float)ctx.time - (float)ctx.startTime;
        power = Mathf.Clamp(power, _minPower, _maxPower);

        TryShoot(_weapon, power);
    }

    private void TryShoot(Weapon weapon, float power)
    {
        if( weapon == null )
            throw new InvalidOperationException();

        if (weapon.CanShoot)
        {
            _animationController.PlayShoot(weapon.Cooldown);
            weapon.Shoot(power);
        }
    }

    public void Update(float deltaTime)
    {
        if (_input.Player.Shoot.inProgress && _weapon.CanShoot)
        {
            _power += deltaTime;
            _power = Mathf.Clamp(_power, _minPower, _maxPower);
            PowerChanged?.Invoke(_power, _maxPower);
            return;
        }

        _power = 1;
        PowerChanged?.Invoke(_power, _maxPower);

    }
}