using System;
using Archer.Model;
using Archer.Presenters;
using UnityEngine;

namespace Archer.TargetComponent
{
    public class HitHeadDetector : MonoBehaviour
    {
        public event Action<int, int, ArrowSkillType> GettingHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArrowPresenter arrow))
                GettingHit?.Invoke(arrow.ArrowModel.Damage, arrow.ArrowModel.AdditionalDamage, arrow.ArrowModel.SkillType);
        }
    }
}