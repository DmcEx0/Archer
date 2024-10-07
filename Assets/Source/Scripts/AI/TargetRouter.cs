using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Archer.AI
{
    public class TargetRouter
    {
        private readonly Dictionary<Collider, float> _targets;

        public TargetRouter()
        {
            _targets = new();
        }

        public KeyValuePair<Collider, float> Target => _targets.ElementAt(Random.Range(0, _targets.Count));

        public int CurrentNumberOfTargets => _targets.Count;

        public void TryAddTargets(Collider targetCollider, float rotationX)
        {
            if (_targets.ContainsKey(targetCollider))
                return;

            _targets.Add(targetCollider, rotationX);
        }
    }
}