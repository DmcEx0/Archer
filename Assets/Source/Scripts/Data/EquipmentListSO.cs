using System.Collections.Generic;
using UnityEngine;

namespace Archer.Data
{
    [CreateAssetMenu]
    public class EquipmentListSO : ScriptableObject
    {
        [SerializeField] private List<WeaponDataConfig> _weaponsData;
        [SerializeField] private List<ArrowDataConfig> _arrowsData;

        public IReadOnlyList<WeaponDataConfig> WeaponsData => _weaponsData;
        public IReadOnlyList<ArrowDataConfig> ArrowsData => _arrowsData;
    }
}