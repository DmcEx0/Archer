using ParadoxNotion;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.AI
{
    public class EnemyAI
    {
        private const float AddDistance = 0.5f;
        private const int PalyerLayer = 6;
        private const int TargetLayer = 9;

        private readonly float _distanceToPlayer;

        private readonly TargetRouter _targetRouter;

        private readonly int _targetsLayerMask = (1 << PalyerLayer) | (1 << TargetLayer);

        private Vector3 _velocity;
        private float _accumulatedPowerOfShot;

        private Collider _target;

        public EnemyAI(float distanceToPlayer)
        {
            _distanceToPlayer = distanceToPlayer;
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
            if (Physics.Linecast(position, CalculateEndPointAfterTime(position, forward), out RaycastHit hitInfo, _targetsLayerMask))
            {
                _targetRouter.TryAddColliderInList(hitInfo.collider);

                if (_target == null)
                {
                    SetTarget();
                    return;
                }

                if (hitInfo.collider == _target)
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
            Debug.DrawRay(position, forward * _distanceToPlayer, Color.red, 0.01f);

            return endPoint;
        }
    }
}