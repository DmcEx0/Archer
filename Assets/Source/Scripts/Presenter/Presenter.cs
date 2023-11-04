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

        if (_model is IUpdatetable)
            _updatetable = (IUpdatetable)_model;

        if (TryGetComponent(out AnimationController controller))
        {
            _animationController = controller;
            _animationController.Init(GetComponent<Animator>());
        }

        _model.Rotated += OnRotated;
        _model.Moved += OnMoved;
        _model.Destroying += OnDestroying;
        _model.DestroyingAll += OnDestroyingAll;

        OnRotated();
        OnMoved();
    }

    private void DestroyCompose()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
    }

    private void OnRotated()
    {
        transform.rotation = _model.Rotation;
    }

    private void OnMoved()
    {
        transform.position = _model.Position;
    }

    private void OnDestroying()
    {
        _model.Rotated -= OnRotated;
        _model.Moved -= OnMoved;
        _model.Destroying -= OnDestroying;
    }

    private void OnDestroyingAll()
    {
        OnDestroying();
        DestroyCompose();

        _model.DestroyingAll -= OnDestroyingAll;
    }
}