using Archer.Model;
using UnityEngine;

namespace Archer.Presenters
{
    public class Presenter : MonoBehaviour
    {
        private SpawnedObject _model;

        private ITickable _tickable = null;

        protected SpawnedObject Model => _model;

        private void Update() => _tickable?.Tick(Time.deltaTime);

        public void Init(SpawnedObject model)
        {
            _model = model;

            if (_model is ITickable)
                _tickable = (ITickable)_model;

            OnInitialized();

            OnRotating();
            OnMoving();
        }

        protected virtual void OnInitialized()
        {
            _model.Rotating += OnRotating;
            _model.Moving += OnMoving;
            _model.Destroying += OnDestroying;
            _model.DestroyingAll += OnDestroyingAll;
        }

        protected virtual void OnDestroying()
        {
            _model.Rotating -= OnRotating;
            _model.Moving -= OnMoving;
            _model.Destroying -= OnDestroying;
        }

        private void DestroyCompose()
        {
            gameObject.SetActive(false);
            gameObject.transform.position = Vector3.zero;
        }

        private void OnRotating()
        {
            transform.rotation = _model.Rotation;
        }

        private void OnMoving()
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
}