using Archer.Model;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class ArrowPresenter : Presenter
{
    private Arrow _model => Model is Arrow ? Model as Arrow : null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Presenter presenter))
        {
            if (presenter is IDamageable)
            {
                IDamageable damageable = presenter as IDamageable;
                damageable.TakeDamage(_model.Damage);
            }
            _model.DestroyAll();
            transform.SetParent(other.transform);
        }
    }
}