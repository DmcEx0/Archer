using UnityEngine;

public class WeaponPresenter : Presenter, IGeneratable
{
    [SerializeField] private Transform _arrowSlot;
    [SerializeField] private Transform _arrowSpawnPoint;
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _rightHandTarget;
    [SerializeField] private Transform _chestTarget;

    public Transform ArrowSlot => _arrowSlot;
    public Transform LeftHandTarget => _leftHandTarget;
    public Transform RightHandTarget => _rightHandTarget;
    public Transform ChestTarget => _chestTarget;
    public Transform GeneratingPoint => _arrowSpawnPoint;
}