using UnityEngine;

[CreateAssetMenu]
public class ArrowDataSO : EquipmentDataSO
{
    [Space]
    [SerializeField] private int _damage;

    public int Damage => _damage;
}