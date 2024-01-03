using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Archer.AI
{
    public class TargetRouter
    {
        private List<Collider> _colliders;

        private List<float> _rotationsX;

        private Targets _target;

        public TargetRouter()
        {
            _colliders = new List<Collider>();
            _rotationsX = new List<float>();
        }

        public Collider Target => GetTarget();

        public void TryAddColliderInList(Collider collider)
        {
            if (_colliders.Contains(collider))
                return;

            _colliders.Add(collider);
        }

        public void SaveWeaponRotation(float rotationX)
        {
            _rotationsX.Add(rotationX);
        }

        private void SelectRandomTarget()
        {
            int randomIndex = Random.Range(0, 100);

            if (randomIndex <= 50)
            {
                _target = Targets.Body;
            }

            if (randomIndex >= 50 && randomIndex <= 66)
            {
                _target = Targets.Head;
            }

            if (randomIndex >= 66 && randomIndex <= 83)
            {
                _target = Targets.Target1;
            }

            if (randomIndex >= 83 && randomIndex <= 100)
            {
                _target = Targets.Target2;
            }
        }

        private Collider GetTarget()
        {
            SelectRandomTarget();

            Collider collider1 = _target switch
            {
                Targets.Body => _colliders.FirstOrDefault(col => col.TryGetComponent(out HitBodyDetector body)),
                Targets.Head => _colliders.FirstOrDefault(col => col.TryGetComponent(out HitHeadDetector head)),
                Targets.Target1 => _colliders.FirstOrDefault(col => col.TryGetComponent(out Target1 target1)),
                Targets.Target2 => _colliders.FirstOrDefault(col => col.TryGetComponent(out Target2 target2)),
                _ => _colliders[Random.Range(0, _colliders.Count)]
            };

            return _colliders[Random.Range(0, _colliders.Count)];
        }
    }
}