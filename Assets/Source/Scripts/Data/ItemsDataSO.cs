using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemsDataSO : ScriptableObject
{
    [SerializeField] private List<WeaponConfigSO> _weaponsConfigs;
    [SerializeField] private List<ArrowConfigSO> _arrowsConfigs;

    public IReadOnlyList<WeaponConfigSO> WeaponsConfigs => _weaponsConfigs;
    public IReadOnlyList<ArrowConfigSO> ArrowsConfigs => _arrowsConfigs;
}