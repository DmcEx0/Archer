using System;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class Weapon : Transformable, IUpdatetable
    {
        private readonly float _cooldown;
        private readonly float _startedPowerOfShot;
        private readonly float _speedChangedAngle;

        private readonly float _minAngle = -30;
        private readonly float _maxAngle = 30;

        private float _accumulatedTime;

        private bool _changedUp = true;

        public Weapon(Vector3 position ,Quaternion rotation ,float speedChangedAngle, float startedPowerOfShot, float cooldown) : base(position, rotation)
        {
            _speedChangedAngle = speedChangedAngle;
            _startedPowerOfShot = startedPowerOfShot;
            _cooldown = cooldown;
        }

        public bool CanShoot { get; private set; } = false;

        public float StartedPowerOfShot => _startedPowerOfShot;

        public event UnityAction<Arrow> Shot;

        public void Shoot(float accumulatedPower)
        {
            if (CanShoot == false)
                throw new InvalidOperationException();

            Vector3 accumulatedVelocity = -Forward * (_startedPowerOfShot * accumulatedPower);

            Arrow arrow = GetArrow(accumulatedVelocity);
            Shot?.Invoke(arrow);

            CanShoot = false;
        }

        public void Update(float deltaTime)
        {
            Reload(deltaTime);

            if (_changedUp == true)
            {
                ChangeAngleUp(deltaTime);
            }
            else if(_changedUp == false)
            {
                ChangeAngleDown(deltaTime);
            }
        }

        private Arrow GetArrow(Vector3 velocity)
        {
            return new Arrow(Position, velocity);
        }

        private void Reload(float deltaTime)
        {
            if (CanShoot == true)
                return;

            _accumulatedTime += deltaTime;

            if (_accumulatedTime > _cooldown)
            {
                CanShoot = true;
                _accumulatedTime = 0;
            }
        }

        //private void ChangeDirection()
        //{
        //    _tweener = _transform.DORotate(GetTargetDirection(_maxAngle), _durationChangedAngle)
        //        .From(GetTargetDirection(_minAngle))
        //        .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        //}

        private Vector3 GetTargetDirection(float angle) => new Vector3(angle, Rotation.eulerAngles.y, 0);

        private void ChangeAngleUp(float deltaTime)
        {
            Quaternion nextRotation = Quaternion.RotateTowards(Rotation, Quaternion.Euler(GetTargetDirection(_maxAngle)), _speedChangedAngle * deltaTime);

            SetRotation(nextRotation);

            if ((int)Rotation.eulerAngles.x == (int)_maxAngle)
                _changedUp = false;
        }

        private void ChangeAngleDown(float deltaTime)
        {
            Quaternion nextRotation = Quaternion.RotateTowards(Rotation, Quaternion.Euler(GetTargetDirection(_minAngle)), _speedChangedAngle * deltaTime);

            SetRotation(nextRotation);

            if ((int)Rotation.eulerAngles.x == (int)_minAngle + 360)
                _changedUp = true;
        }
    }
}