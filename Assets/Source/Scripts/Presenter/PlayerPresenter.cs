using UnityEngine;

[RequireComponent(typeof(AnimationController), typeof(BoxCollider))]
public class PlayerPresenter : Presenter, IGeneratable
{
    [SerializeField] private Transform _weaponPosition;

    public Transform GeneratingPoint => _weaponPosition;
}