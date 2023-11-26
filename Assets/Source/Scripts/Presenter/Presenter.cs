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

        if (_model is IUpdatetable)
            _updatetable = (IUpdatetable)_model;

        OnInitialized();

        OnRotated();
        OnMoved();
    }

    protected virtual void OnInitialized()
    {
        _model.Rotated += OnRotated;
        _model.Moved += OnMoved;
        _model.Destroying += OnDestroying;
        _model.DestroyingAll += OnDestroyingAll;
    }

    protected virtual void OnDestroying()
    {
        _model.Rotated -= OnRotated;
        _model.Moved -= OnMoved;
        _model.Destroying -= OnDestroying;
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
        if (this != null)
            transform.position = _model.Position;
    }

    private void OnDestroyingAll()
    {
        OnDestroying();
        DestroyCompose();

        _model.DestroyingAll -= OnDestroyingAll;
    }
}