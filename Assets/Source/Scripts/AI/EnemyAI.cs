using UnityEngine;
using UnityEngine.Events;

namespace Archer.AI
{
    public class EnemyAI
    {
        private readonly float _distanceToPlayer;

        private Vector3 _velocity;
        private float _accumulatedPowerOfShot;

        public EnemyAI(float distanceToPlayer)
        {
            _distanceToPlayer = distanceToPlayer;
        }

        public event UnityAction Shot;

        public void SetStartVelocityOfFirstShoot(float accumulatedPowerOfShot)
        {
            _accumulatedPowerOfShot = accumulatedPowerOfShot;
        }

        public void TargetDetection(Vector3 position, Vector3 forward)
        {
            if (Physics.Linecast(position, CalculateEndPointAfterTime(position, forward), out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent(out PlayerPresenter playerPresenter))
                {
                    Shot?.Invoke();
                }
            }
        }

        private Vector3 CalculateEndPointAfterTime(Vector3 position, Vector3 forward)
        {
            _velocity = forward * _accumulatedPowerOfShot;

            float time = _distanceToPlayer / _velocity.magnitude;

            Vector3 endPoint = position + BallisticsRouter.GetCalculatedPositionAfterTime(_velocity, time);

            Debug.DrawLine(position, endPoint, Color.green, 0.01f);
            Debug.DrawRay(position, forward * _distanceToPlayer, Color.red, 0.01f);

            return endPoint;
        }
    }
}