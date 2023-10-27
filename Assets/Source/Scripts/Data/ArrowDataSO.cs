using UnityEngine;

[CreateAssetMenu]
public class ArrowDataSO : EquipmentDataSO
{
    [Space]
    [Range(1, 50)]
    [SerializeField] private int _damage;

    public int Damage => _damage;
}