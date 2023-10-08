using UnityEngine;

public class TrajectoryRenderer
{
    private LineRenderer _lineRendererComponent;

    public void Init(LineRenderer lineRendererComponent)
    {
        _lineRendererComponent = lineRendererComponent;
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed ,float deltaTime)
    {
        Vector3[] points = new Vector3[10];

        _lineRendererComponent.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.5f;

            points[i] = origin + speed * time + 9.8f * Vector3.down * time * time / 2f;
            //points[i] = origin + speed * time + BallisticsRouter.GetCalculatedPosition(speed, deltaTime) * deltaTime;

            if (points[i].y < 0)
            {
                _lineRendererComponent.positionCount = i + 1;
                break;
            }
        }

        _lineRendererComponent.SetPositions(points);
    }
}
