using Archer.Model;
using UnityEngine;
using System;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputRouter : IInputRouter
{
    private PlayerInput _input;

    private Weapon _weapon;

    private float _time = 1;

    private float _maxPower = 3f;
    private float _minPower = 1f;

    public PlayerInputRouter()
    {
        _input = new PlayerInput();
    }

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
            weapon.Shoot(power);
    }

    public void Update(float deltaTime)
    {
        if (_weapon == null)
            return;

        //float offset = 0.5f;

        //_trajectoryRenderer.ShowTrajectory(_weapon.Position + -_weapon.Forward * offset, -_weapon.Forward * _time, deltaTime);

        if (_input.Player.Shoot.inProgress)
        {
            _time += deltaTime;
            _time = Mathf.Clamp(_time, _minPower, _maxPower);
            return;
        }

        _time = 1;
    }
}