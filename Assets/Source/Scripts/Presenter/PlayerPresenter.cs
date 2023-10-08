using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPresenter : Presenter, IGeneratable
{
    [SerializeField] private Transform _weaponPosition;

    public Transform SpawnPoint => _weaponPosition;
}