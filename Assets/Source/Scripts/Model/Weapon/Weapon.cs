using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class Weapon : Transformable, IUpdatetable
    {

        private readonly float _cooldown;
        private readonly float _startedPowerOfShot;
        private readonly float _durationChangedAngle;

        private readonly float _minAngle = -30;
        private readonly float _maxAngle = 30;

        private Transform _transform; // модель не работает с трансформом


        private Tweener _tweener;
        private float _accumulatedTime;

        public Weapon(Vector3 position ,Quaternion rotation ,float durationChangedAngle, float startedPowerOfShot, float cooldown) : base(position, rotation)
        {
            _durationChangedAngle = durationChangedAngle;
            _startedPowerOfShot = startedPowerOfShot;
            _cooldown = cooldown;
        }

        public bool CanShoot { get; private set; } = false;

        public float StartedPowerOfShot => _startedPowerOfShot;

        public event UnityAction<Arrow> Shot;

        public void Init(Transform transform)
        {
            _transform = transform;

            ChangeDirection();
        }

        public void Shoot(float accumulatedPower)
        {
            if (CanShoot == false)
                throw new InvalidOperationException();

            Vector3 accumulatedVelocity = -Forward * (_startedPowerOfShot * accumulatedPower);

            Arrow arrow = GetArrow(accumulatedVelocity);
            Shot?.Invoke(arrow);

            CanShoot = false;
        }

        public void OnDestroy()
        {
            _tweener.Kill();
            Destroy();
        }

        public void Update(float deltaTime)
        {
            Reload(deltaTime);

            SetRotation(Quaternion.Euler(_transform.eulerAngles));
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

        private void ChangeDirection()
        {
            _tweener = _transform.DORotate(GetTargetDirection(_maxAngle), _durationChangedAngle)
                .From(GetTargetDirection(_minAngle))
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            //var currentAngle = Rotation.eulerAngles;

            //Vector3 newPos = Vector3.MoveTowards(currentAngle, GetTargetDirection(_maxAngle), 0.07f);
            //SetRotation(Quaternion.Euler(newPos));
        }

        private Vector3 GetTargetDirection(float angle) => new Vector3(angle, Rotation.eulerAngles.y, 0);
    }
}