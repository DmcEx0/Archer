using Archer.Model;
using UnityEngine;

[RequireComponent(typeof(AnimationController), typeof(BoxCollider))]
public class PlayerPresenter : Presenter, IGeneratable, IDamageable
{
    [SerializeField] private Transform _weaponPosition;

    private Character _model => Model is Character ? Model as Character : null;
    public Transform GeneratingPoint => _weaponPosition;

    public void TakeDamage(int damage)
    {
        _model.TakeDamage(damage);
        AnimationController.PlayeHitA();
    }
}