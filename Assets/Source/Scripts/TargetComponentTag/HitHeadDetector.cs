using Archer.Model;
using Archer.Presenters;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.TargetComponent
{
    public class HitHeadDetector : MonoBehaviour
    {
        public event UnityAction<int, int, ArrowSkillType> Hit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArrowPresenter arrow))
                Hit?.Invoke(arrow.ArrowModel.Damage, arrow.ArrowModel.AdditionalDamage, arrow.ArrowModel.SkillType);
        }
    }
}