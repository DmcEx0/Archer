using UnityEngine;

[CreateAssetMenu]
public class WeaponConfigSO : ScriptableObject
{
    [SerializeField] private WeaponPresenter _presenter;
    [SerializeField] private float _shotPower;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _speedChangedAngle;

    public WeaponPresenter Presenter => _presenter;
    public float ShotPower => _shotPower;
    public float Cooldown => _cooldown;
    public float SpeedChangedAngle => _speedChangedAngle;
}