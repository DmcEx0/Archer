using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.AI
{
    public class TargetRouter : MonoBehaviour
    {
        // пока ни на что не влияет. В разработке.

        [SerializeField] private List<Collider> _colliders;

        public Collider Target => SetTarget();

        private Collider SetTarget()
        {
            int random = Random.Range(0,2);

            Debug.Log(random);

            Collider result = random switch
            {
                0 => _colliders[0],
                1 => _colliders[1],
                2 => null,
                _ => null
            };

            return result;
        }
    }
}