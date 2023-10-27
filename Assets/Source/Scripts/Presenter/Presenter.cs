using System;
using Archer.Model;
using UnityEngine;

public class Presenter : MonoBehaviour
{
    private Transformable _model;

    private AnimationController _animationController;

    private IUpdatetable _updatetable = null;

    protected Transformable Model => _model;

    public AnimationController AnimationController => _animationController;

    private void Update() => _updatetable?.Update(Time.deltaTime);

    public void Init(Transformable model)
    {
        _model = model;

        if(_model is IUpdatetable)
            _updatetable = (IUpdatetable) _model;

        if(TryGetComponent(out AnimationController controller))
            _animationController = controller;

        _model.Rotated += OnRotated;
        _model.ChangedPosition += OnChangedPosition;
        _model.Destroying += OnDestroying;

        OnRotated();
        OnChangedPosition();
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

    private void OnChangedPosition()
    {
        transform.position = _model.Position;
    }

    private void OnDestroying()
    {
        _model.Rotated -= OnRotated;
        _model.ChangedPosition -= OnChangedPosition;
        _model.Destroying -= OnDestroying;

        DestroyCompose();
    }
}