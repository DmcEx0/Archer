using Archer.Model;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ArrowPresenter : Presenter
{
    private Arrow _model => Model is Arrow ? Model as Arrow : null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Presenter presenter))
        {
            if(presenter is EnemyPresenter || presenter is PlayerPresenter)
            {
                presenter.OnCollision(_model.Damage);
            }

            DestroyCompose();
        }
    }
}