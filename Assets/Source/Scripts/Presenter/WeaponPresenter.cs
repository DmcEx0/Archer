using UnityEngine;

public class WeaponPresenter : Presenter, IGeneratable
{
    [SerializeField] private Transform _shotPoint;

    public Transform SpawnPoint => _shotPoint;
}