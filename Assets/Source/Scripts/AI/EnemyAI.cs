using UnityEngine;
using UnityEngine.Events;

namespace Archer.AI
{
    public class EnemyAI
    {
        private const float DelayBeforeFiring = 0.2f;
        private const int PalyerLayer = 6;
        private const int TargetLayer = 9;

        private readonly TargetRouter _targetRouter;

        private readonly int _targetsLayerMask = (1 << PalyerLayer) | (1 << TargetLayer);

        private Vector3 _velocity;
        private float _accumulatedPowerOfShot;
        private float _distanceToTarget;

        private float _accumulatedTime;

        private Collider _target;

        public EnemyAI()
        {
            _targetRouter = new();
        }

        public event UnityAction Shot;

        public void SetStartVelocityOfFirstShoot(float accumulatedPowerOfShot)
        {
            _accumulatedPowerOfShot = accumulatedPowerOfShot;
        }

        public void SetTarget()
        {
            _target = _targetRouter.Target;
        }

        public void CheckTargetInDirection(Vector3 position, Vector3 forward, float deltaTime)
        {
            if (_target == null)
            {
                if (Physics.Raycast(position, forward, out RaycastHit hitInfo1))
                {
                    if (hitInfo1.collider.TryGetComponent(out HitBodyDetector body))
                    {
                        _target = hitInfo1.collider;
                    }
                }

                return;
            }

            _distanceToTarget = (_target.transform.position - position).magnitude;

            if (Physics.Linecast(position, CalculateEndPointAfterTime(position, forward), out RaycastHit hitInfo2, _targetsLayerMask))
            {
                _targetRouter.TryAddColliderInList(hitInfo2.collider);

                if (hitInfo2.collider == _target)
                {
                    _accumulatedTime += deltaTime;
                    if (_accumulatedTime >= DelayBeforeFiring)
                    {
                        Shot?.Invoke();
                        _accumulatedTime = 0f;
                    }
                }
            }
        }

        private Vector3 CalculateEndPointAfterTime(Vector3 position, Vector3 forward)
        {
            float addDistanceToTarget = 1f;
            _velocity = forward * _accumulatedPowerOfShot;

            float time = (_distanceToTarget + addDistanceToTarget) / _velocity.magnitude;

            Vector3 endPoint = position + BallisticsRouter.GetCalculatedPositionAfterTime(_velocity, time);

            Debug.DrawLine(position, endPoint, Color.green, 0.01f);
            Debug.DrawRay(position, forward * 10, Color.red, 0.01f);

            return endPoint;
        }
    }
}