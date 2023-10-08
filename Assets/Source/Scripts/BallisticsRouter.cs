using UnityEngine;

public struct BallisticsRouter
{
    private const float _gravity = 9.8f;

    public static Vector3 GetCalculatedPosition(ref Vector3 velocity, float deltaTime)
    {
        velocity += Vector3.down * _gravity * deltaTime;

        return velocity * deltaTime;
    }

    public static Vector3 GetCalculatedPositionAfterTime(Vector3 velocity, float time)
    {
        velocity = velocity * time + Vector3.down * _gravity * time * time /2f;

        return velocity ;
    }
}