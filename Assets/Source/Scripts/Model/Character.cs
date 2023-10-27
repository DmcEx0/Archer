using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class Character : Transformable, IUpdatetable
    {
        private readonly Health _health;

        public Character(Vector3 position ,Quaternion rotation, Health health) : base(position, rotation)
        {
            _health = health;
            _health.Died += OnDied;

            MoveTo(position);
            Rotate(rotation);
        }

        public event UnityAction Died;

        public void TakeDamage(int damage)
        {
            if( damage < 0 )
                throw new System.ArgumentOutOfRangeException(nameof(damage));

            _health.TakeDamage(damage);
        }

        public void Update(float deltaTime)
        {
            MoveTo(Position);
            Rotate(Rotation);
        }

        private void OnDied() 
        {
            Destroy();
            Died?.Invoke();
            _health.Died -= OnDied;
        }
    }
}