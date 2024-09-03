using System.Collections.Generic;
using Archer.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.AI
{
    public class EnemyAI
    {
        private const int MaxRayDistanse = 100;
        private const int MaxNumberOfTargets = 4;
        private const float AngleRange = 0.2f;

        private const int PalyerLayer = 6;
        private const int TargetLayer = 9;

        private readonly TargetRouter _targetRouter;

        private readonly int _targetsLayerMask = (1 << PalyerLayer) | (1 << TargetLayer);

        private Vector3 _velocity;
        private float _accumulatedPowerOfShot;
        private float _distanceToTarget;

        private float _minAngleRange;
        private float _maxAngleRange;

        private Collider _target;
        private float _weaponRotationXToShot = 100;

        private List<Collider> _colliders;

        private bool _isChosenFirstTarget = false;

        public EnemyAI()
        {
            _targetRouter = new();
            _colliders = new();
        }

        public event UnityAction Shot;

        public void SetStartVelocityOfFirstShoot(float accumulatedPowerOfShot)
        {
            _accumulatedPowerOfShot = accumulatedPowerOfShot;
        }

        public void SetTarget()
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
                if (Physics.Raycast(position, forward, out RaycastHit hitInfo1, MaxRayDistanse, _targetsLayerMask) && _target == null)
                {
                    if (_colliders.Count == 0 || _colliders.Contains(hitInfo1.collider) == false)
                    {
                        _colliders.Add(hitInfo1.collider);
                        _target = hitInfo1.collider;
                        _distanceToTarget = (_target.transform.position - position).magnitude;
                    }
                }

                if (Physics.Linecast(position, CalculateEndPointAfterTime(position, forward), out RaycastHit hitInfo2, _targetsLayerMask))
                {
                    if (hitInfo2.collider == _target)
                    {
                        _targetRouter.TryAddTargets(hitInfo2.collider, RoundValue(rotationX));

                        if (_isChosenFirstTarget == false)
                            SetTarget();

                        _target = null;
                    }
                }
            }

            float roundCurrentRotationX = RoundValue(rotationX);

            if (roundCurrentRotationX > _minAngleRange && roundCurrentRotationX < _maxAngleRange)
                Shot?.Invoke();
        }

        private float RoundValue(float value)
        {
            return Mathf.Round(value * 10.0f) * 0.1f;
        }

        private Vector3 CalculateEndPointAfterTime(Vector3 position, Vector3 forward)
        {
            float addDistanceToTarget = 2.4f;
            _velocity = forward * _accumulatedPowerOfShot;

            float time = (_distanceToTarget + addDistanceToTarget) / _velocity.magnitude;

            Vector3 endPoint = position + BallisticsRouter.GetCalculatedPositionAfterTime(_velocity, time);

            Debug.DrawLine(position, endPoint, Color.green, 0.001f);
            Debug.DrawRay(position, forward * 20, Color.red, 0.001f);

            return endPoint;
        }
    }
}