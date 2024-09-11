using Archer.Model;
using UnityEngine;
using System;
using Archer.Animations;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.Events;

namespace Archer.Input
{
    public class PlayerInputRouter : IInputRouter
    {
        private readonly AnimationController _animationController;

        private readonly PlayerInput _input;

        private readonly float _maxPower = 3f;
        private readonly float _minPower = 1f;

        private Weapon _weapon;

        private float _power = 1;
        private bool _isUIPressed = false;

        public PlayerInputRouter(AnimationController animationController)
        {
            _input = new PlayerInput();
            _animationController = animationController;
        }

        public event UnityAction<float, float> PowerChanged;

        public void BindWeapon(Weapon weapon)
        {
            _weapon = weapon;
        }

        public void CanGainingPower(bool isCanNot)
        {
            PowerChanged?.Invoke(_power, _maxPower);

            _isUIPressed = isCanNot;
        }

        public void Update(float deltaTime)
        {
            if (_isUIPressed == true)
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

        private void Shoot(CallbackContext ctx)
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
            _input.Player.Shoot.canceled += Shoot;
        }

        public void OnDisable()
        {
            _input.Disable();
            _input.Player.Shoot.canceled -= Shoot;
        }

        private void TryShoot(Weapon weapon, float power)
        {
            if (weapon == null)
                throw new InvalidOperationException();

            if (weapon.CanShoot)
            {
                _animationController.PlayShoot(weapon.Cooldown);
                weapon.Shoot(power);
            }
        }
    }
}