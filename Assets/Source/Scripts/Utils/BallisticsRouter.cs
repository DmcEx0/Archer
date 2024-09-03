using UnityEngine;

namespace Archer.Utils
{
    public struct BallisticsRouter
    {
        private const float Gravity = 9.8f;

        public static Vector3 GetCalculatedPosition(ref Vector3 velocity, float deltaTime)
        {
            velocity += Vector3.down * Gravity * deltaTime;

            return velocity * deltaTime;
        }

        public static Vector3 GetCalculatedPositionAfterTime(Vector3 velocity, float time)
        {
            velocity = velocity * time + Vector3.down * Gravity * time * time / 2f;

            return velocity;
        }
    }
}