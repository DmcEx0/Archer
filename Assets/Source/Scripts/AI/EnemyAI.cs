using UnityEngine;
using UnityEngine.Events;

namespace Archer.AI
{
    public class EnemyAI
    {
        private const float AddDistance = 0.5f;
        private const int PalyerLayer = 6;
        private const int TargetLayer = 9;

        private readonly TargetRouter _targetRouter;

        private readonly int _targetsLayerMask = (1 << PalyerLayer) | (1 << TargetLayer);

        private Vector3 _velocity;
        private float _accumulatedPowerOfShot;
        private float _distanceToPlayer;

        private Collider _target;

        private bool _isPlayerFound = false;

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

        public void CheckTargetInDirection(Vector3 position, Vector3 forward)
        {
            if (_isPlayerFound == false)
            {
                if (Physics.Raycast(position, forward, out RaycastHit hitInfo1))
                {
                    if (hitInfo1.collider.TryGetComponent(out PlayerPresenter player))
                    {
                        _distanceToPlayer = (position - player.transform.position).magnitude - 0.7f;
                        _isPlayerFound = true;
                    }
                }
            }

            if (Physics.Linecast(position, CalculateEndPointAfterTime(position, forward), out RaycastHit hitInfo2, _targetsLayerMask))
            {
                _targetRouter.TryAddColliderInList(hitInfo2.collider);

                if (_target == null)
                {
                    SetTarget();
                    return;
                }

                if (hitInfo2.collider == _target)
                {
                    Shot?.Invoke();
                }
            }
        }

        private Vector3 CalculateEndPointAfterTime(Vector3 position, Vector3 forward)
        {
            _velocity = forward * _accumulatedPowerOfShot;

            float time = (_distanceToPlayer + AddDistance) / _velocity.magnitude;

            Vector3 endPoint = position + BallisticsRouter.GetCalculatedPositionAfterTime(_velocity, time);

            Debug.DrawLine(position, endPoint, Color.green, 0.01f);
            Debug.DrawRay(position, forward * 10, Color.red, 0.01f);

            return endPoint;
        }
    }
}