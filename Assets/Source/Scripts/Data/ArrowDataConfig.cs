using Archer.Model;
using UnityEngine;

namespace Archer.Data
{
    [CreateAssetMenu]
    public class ArrowDataConfig : EquipmentDataConfig
    {
        [Space]
        [Range(1, 50)]
        [SerializeField] private int _mainDamage;
        [Range(1, 10)]
        [SerializeField] private int _additionalDamage;
        [SerializeField] private ArrowSkillType _skillType;

        public int MainDamage => _mainDamage;
        public int AdditionalDamage => _additionalDamage;
        public ArrowSkillType SkillType => _skillType;
    }
}