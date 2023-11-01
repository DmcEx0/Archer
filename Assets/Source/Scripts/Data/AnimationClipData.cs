using UnityEngine;

[CreateAssetMenu]
public class AnimationClipData : ScriptableObject
{
    [SerializeField] private AnimationClip _aim;
    [SerializeField] private AnimationClip _reload;
    [SerializeField] private AnimationClip _shoot;
    [SerializeField] private AnimationClip _death;
    [SerializeField] private AnimationClip _hit_A;
    [SerializeField] private AnimationClip _hit_B;
    [SerializeField] private AnimationClip _running;
    [SerializeField] private AnimationClip _sitIdle;
    [SerializeField] private AnimationClip _sitStandUp;

    public float AimLenght => _aim.length;
    public float RealoadLenght => _reload.length;
    public float ShootLenght => _shoot.length;
    public float DeathLenght => _death.length;
    public float Hit_A_Lenght => _hit_A.length;
    public float Hit_B_Lenght => _hit_B.length;
    public float RunningLenght => _running.length;
    public float SitIdleLenght => _sitIdle.length;
    public float SitStandUpLenght => _sitStandUp.length;
}