using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyPresenter : Presenter, IGeneratable
{
    [SerializeField] private Transform _weaponPosition;

    public Transform SpawnPoint => _weaponPosition;
}