using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class Character : Transformable
    {
        private readonly Health _health;

        public Character(Vector3 position ,Quaternion rotation, Health health) : base(position, rotation)
        {
            _health = health;
            _health.Died += OnDied;
        }

        public event UnityAction Died;

        public void TakeDamage(int damage)
        {
            if( damage < 0 )
                throw new System.ArgumentOutOfRangeException(nameof(damage));

            _health.TakeDamage(damage);
        }

        private void OnDied() 
        {
            Destroy();
            Died?.Invoke();
            _health.Died -= OnDied;
        }
    }
}