using UnityEngine;

public static class HashAnimationNames
{
    public const string AimString = "Aim";
    public const string ReloadString = "Reload";
    public const string ShootString = "Shoot";
    public const string DeathString = "Death";
    public const string Hit_A_String = "Hit_A";
    public const string Hit_B_String = "Hit_B";
    public const string RunningString = "Running";
    public const string SitIdleString = "SitIdle";
    public const string SitStandUpString = "SitStandUp";

    public static readonly int Aim = Animator.StringToHash(AimString);
    public static readonly int Reload = Animator.StringToHash(ReloadString);
    public static readonly int Shoot = Animator.StringToHash(ShootString);
    public static readonly int Death = Animator.StringToHash(DeathString);
    public static readonly int Hit_A = Animator.StringToHash(Hit_A_String);
    public static readonly int Hit_B = Animator.StringToHash(Hit_B_String);
    public static readonly int Running = Animator.StringToHash(RunningString);
}