using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentListSO : ScriptableObject
{
    [SerializeField] private List<WeaponDataSO> _weaponsData;
    [SerializeField] private List<ArrowDataSO> _arrowsData;

    public IReadOnlyList<WeaponDataSO> WeaponsData => _weaponsData;
    public IReadOnlyList<ArrowDataSO> ArrowsData => _arrowsData;
}