using System.Collections.Generic;
using UnityEngine;

namespace Archer.AI
{
    public class TargetRouter
    {
        private List<Collider> _colliders;

        public TargetRouter()
        {
            _colliders = new List<Collider>();
        }

        public Collider Target => SelectedTarget();

        public void TryAddColliderInList(Collider collider)
        {
            if (_colliders.Contains(collider))
                return;

            _colliders.Add(collider);
        }

        private Collider SelectedTarget()
        {
            int randomIndex = Random.Range(0,100);

            foreach (Collider collider in _colliders)
            {
                if (randomIndex <= 50)       // вынести шанс в отдельный код
                {
                    if (collider.TryGetComponent(out PlayerPresenter playerPresenter))
                        return collider;
                }

                if (randomIndex >= 50 && randomIndex <= 66)
                {
                    if (collider.TryGetComponent(out Target1 target1))                  
                        return collider;
                }

                if (randomIndex >= 66 && randomIndex <= 83)
                {
                    if (collider.TryGetComponent(out Target2 target2))
                        return collider;
                }

                if (randomIndex >= 83 && randomIndex <= 100)
                {
                    if (collider.TryGetComponent(out Target3 target3))
                        return collider;
                }
            }

            return _colliders[Random.Range(0, _colliders.Count)];
        }
    }
}