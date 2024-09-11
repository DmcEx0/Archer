using System;
using System.Collections.Generic;
using Archer.Utils;
using UnityEngine;

namespace Archer.AI
{
    public class EnemyAI
    {
        private const int MaxRayDistance = 100;
        private const int MaxNumberOfTargets = 4;
        private const float AngleRange = 0.2f;

        private const int PlayerLayer = 6;
        private const int TargetLayer = 9;
        private const bool IsChosenFirstTarget = false;
        private const int TargetsLayerMask = (1 << PlayerLayer) | (1 << TargetLayer);

        private readonly TargetRouter _targetRouter;
        private readonly List<Collider> _colliders;

        private Vector3 _velocity;
        private float _accumulatedPowerOfShot;
        private float _distanceToTarget;

        private float _minAngleRange;
        private float _maxAngleRange;

        private Collider _target;
        private float _weaponRotationXToShot = 100;

        public EnemyAI()
        {
            _targetRouter = new();
            _colliders = new();
        }

        public event Action Shot;

        public void SetStartVelocityOfFirstShoot(float accumulatedPowerOfShot)
        {
            _accumulatedPowerOfShot = accumulatedPowerOfShot;
        }

        public void ConfigureTarget()
        {
            var target = _targetRouter.Target;
            _weaponRotationXToShot = target.Value;
            _minAngleRange = _weaponRotationXToShot - AngleRange;
            _maxAngleRange = _weaponRotationXToShot + AngleRange;
        }

        public void CheckTargetInDirection(Vector3 position, Vector3 forward, float rotationX)
        {
            if (_targetRouter.CurrentNumberOfTargets < MaxNumberOfTargets)
            {
                if (Physics.Raycast(position, forward, out RaycastHit hitInfo1, MaxRayDistance, TargetsLayerMask) && _target == null)
                {
                    if (_colliders.Count == 0 || _colliders.Contains(hitInfo1.collider) == false)
                    {
                        _colliders.Add(hitInfo1.collider);
                        _target = hitInfo1.collider;
                        _distanceToTarget = (_target.transform.position - position).magnitude;
                    }
                }

                if (Physics.Linecast(position, GetCalculatedEndPointAfterTime(position, forward), out RaycastHit hitInfo2, TargetsLayerMask))
                {
                    if (hitInfo2.collider == _target)
                    {
                        _targetRouter.TryAddTargets(hitInfo2.collider, GetRoundValue(rotationX));

                        if (IsChosenFirstTarget == false)
                            ConfigureTarget();

                        _target = null;
                    }
                }
            }

            float roundCurrentRotationX = GetRoundValue(rotationX);

            if (roundCurrentRotationX > _minAngleRange && roundCurrentRotationX < _maxAngleRange)
                Shot?.Invoke();
        }

        private float GetRoundValue(float value)
        {
            float additionalValue = 10f;
            float defaultOffset = 0.1f;
            return Mathf.Round(value * additionalValue) * defaultOffset;
        }

        private Vector3 GetCalculatedEndPointAfterTime(Vector3 position, Vector3 forward)
        {
            float addDistanceToTarget = 2.4f;
            _velocity = forward * _accumulatedPowerOfShot;

            float time = (_distanceToTarget + addDistanceToTarget) / _velocity.magnitude;

            Vector3 endPoint = position + BallisticsRouter.GetCalculatedPositionAfterTime(_velocity, time);

            return endPoint;
        }
    }
}