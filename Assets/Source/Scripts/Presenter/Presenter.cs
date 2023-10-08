using System;
using Archer.Model;
using UnityEngine;

public class Presenter : MonoBehaviour
{
    private Transformable _model;

    private IUpdatetable _updatetable = null;

    protected Transformable Model => _model;

    private void Update() => _updatetable?.Update(Time.deltaTime);

    public void Init(Transformable model)
    {
        _model = model;

        if(_model is IUpdatetable)
            _updatetable = (IUpdatetable) _model;

        _model.Rotated += OnRotated;
        _model.ChangedPosition += OnChangePosition;
        _model.Destroying += OnDestroying;

        OnRotated();
        OnChangePosition();
    }

    public void DestroyCompose()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
    }

    public void OnCollision(int damage)
    {
        if (_model is Character)
        {
            Character character = _model as Character;
            character.TakeDamage(damage);
        }
        else
            throw new InvalidOperationException();
    }

    private void OnRotated()
    {
        transform.rotation = _model.Rotation;
    }

    private void OnChangePosition()
    {
        transform.position = _model.Position;
    }

    private void OnDestroying()
    {
        _model.Rotated -= OnRotated;
        _model.ChangedPosition -= OnChangePosition;
        _model.Destroying -= OnDestroying;

        DestroyCompose();
    }
}