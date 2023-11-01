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

            MoveTo(position);
            Rotate(rotation);
        }

        public event UnityAction<Arrow> Shoted;
        public Vector3 ArrowSpawnPosition { get; private set; }
        public bool CanShoot { get; private set; } = false;
        public float StartedPowerOfShot => _startedPowerOfShot;
        public float Cooldown => _cooldown;

        public void Shoot(float accumulatedPower)
        {
            if (CanShoot == false)
                throw new InvalidOperationException();

            Vector3 accumulatedVelocity = Forward * (_startedPowerOfShot * accumulatedPower);

            Arrow arrow = GetArrow(accumulatedVelocity);
            Shoted?.Invoke(arrow);

            CanShoot = false;
        }

        public void Update(float deltaTime)
        {
            Reload(deltaTime);
            MoveTo(Position);

            if (_changedUp == true)
            {
                ChangeAngle(deltaTime, _maxAngle);

                if ((int)Rotation.eulerAngles.x == (int)_maxAngle)
                    _changedUp = false;
            }
            else if(_changedUp == false)
            {
                ChangeAngle(deltaTime, _minAngle);

                if ((int)Rotation.eulerAngles.x == (int)_minAngle + 360)
                    _changedUp = true;
            }
        }

        public void SetArrowSpawnPoint(Vector3 arrowSpawnPosition)
        {
            ArrowSpawnPosition = arrowSpawnPosition;
        }

        private Arrow GetArrow(Vector3 velocity)
        {
            return new Arrow(ArrowSpawnPosition, velocity);
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

        private Vector3 GetTargetDirection(float angle) => new Vector3(angle, Rotation.eulerAngles.y, Rotation.eulerAngles.z);

        private void ChangeAngle(float deltaTime, float angle)
        {
            Quaternion nextRotation = Quaternion.RotateTowards(Rotation, Quaternion.Euler(GetTargetDirection(angle)), _speedChangedAngle * deltaTime);

            Rotate(nextRotation);
        }
    }
}