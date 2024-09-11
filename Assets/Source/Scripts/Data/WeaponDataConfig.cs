using UnityEngine;

namespace Archer.Data
{
    [CreateAssetMenu]
    public class WeaponDataConfig : EquipmentDataConfig
    {
        [Space]
        [Range(1, 10)]
        [SerializeField] private float _shotPower;
        [Range(0, 5)]
        [SerializeField] private float _cooldown;
        [Range(5, 30)]
        [SerializeField] private float _speedChangedAngle;

        public float ShotPower => _shotPower;
        public float Cooldown => _cooldown;
        public float SpeedChangedAngle => _speedChangedAngle;
    }
}