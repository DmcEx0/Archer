using UnityEngine;
using UnityEngine.Events;

public class HitBodyDetector : MonoBehaviour
{
    public event UnityAction<int, int, ArrowSkillType> Hit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ArrowPresenter arrow))
            Hit?.Invoke(arrow.ArrowModel.Damage, arrow.ArrowModel.AdditionalDamage, arrow.ArrowModel.SkillType);
    }
}