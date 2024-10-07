using System;
using Archer.Model;
using Archer.Presenters;
using UnityEngine;

namespace Archer.TargetComponent
{
    public class HitBodyPartDetector : MonoBehaviour
    {
        [SerializeField] private BodyParts _currentPart;

        public event Action<int, int, ArrowSkillType, BodyParts> GettingHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArrowPresenter arrow))
            {
                GettingHit?.Invoke(arrow.ArrowModel.Damage, arrow.ArrowModel.AdditionalDamage,
                    arrow.ArrowModel.SkillType, _currentPart);
            }
        }
    }
}