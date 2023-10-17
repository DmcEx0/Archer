using UnityEngine;

[CreateAssetMenu]
public class WeaponDataSO : EquipmentDataSO
{
    [Space]
    [SerializeField] private float _shotPower;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _speedChangedAngle;

    public float ShotPower => _shotPower;
    public float Cooldown => _cooldown;
    public float SpeedChangedAngle => _speedChangedAngle;
}