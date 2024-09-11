using Archer.Model;
using UnityEngine;
using System;
using Archer.Animations;
using static UnityEngine.InputSystem.InputAction;

namespace Archer.Input
{
    public class PlayerInputRouter : IInputRouter
    {
        private readonly AnimationHandler _animationHandler;

        private readonly PlayerInput _input;

        private readonly float _maxPower = 3f;
        private readonly float _minPower = 1f;

        private Weapon _weapon;

        private float _power = 1;
        private bool _isPressedUI = false;

        public PlayerInputRouter(AnimationHandler animationHandler)
        {
            _input = new PlayerInput();
            _animationHandler = animationHandler;
        }

        public event Action<float, float> PowerChanged;

        public void BindWeapon(Weapon weapon)
        {
            _weapon = weapon;
        }

        public void SetGainingPowerState(bool isCanNot)
        {
            PowerChanged?.Invoke(_power, _maxPower);

            _isPressedUI = isCanNot;
        }

        public void Update(float deltaTime)
        {
            if (_isPressedUI == true)
            {
                return;
            }

            if (_input.Player.Shoot.inProgress && _weapon.CanShoot)
            {
                _power += deltaTime;
                _power = Mathf.Clamp(_power, _minPower, _maxPower);
                PowerChanged?.Invoke(_power, _maxPower);
                return;
            }

            PowerChanged?.Invoke(_power, _maxPower);
            _power = 1;
        }

        private void OnShoot(CallbackContext ctx)
        {
            float minPowerToShot = 1.2f;
            float power = (float)ctx.time - (float)ctx.startTime;

            power = Mathf.Clamp(power, _minPower, _maxPower);

            if (_power > minPowerToShot)
                TryShoot(_weapon, power);
        }

        public void OnEnable()
        {
            _input.Enable();
            _input.Player.Shoot.canceled += OnShoot;
        }

        public void OnDisable()
        {
            _input.Disable();
            _input.Player.Shoot.canceled -= OnShoot;
        }

        private void TryShoot(Weapon weapon, float power)
        {
            if (weapon == null)
                throw new InvalidOperationException();

            if (weapon.CanShoot)
            {
                _animationHandler.PlayShoot(weapon.Cooldown);
                weapon.Shoot(power);
            }
        }
    }
}