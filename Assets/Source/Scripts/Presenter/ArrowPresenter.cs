using Archer.Model;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class ArrowPresenter : Presenter
{
    private Collider _collider;
    public Arrow ArrowModel => base.Model is Arrow ? base.Model as Arrow : null;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        _collider.enabled = false;
        ArrowModel.DestroyAll();
        //transform.SetParent(other.transform);
    }
}