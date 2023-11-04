using System;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class Character : Transformable, IUpdatetable
    {
        public readonly Health Health;

        public Character(Vector3 position, Quaternion rotation, Health health) : base(position, rotation)
        {
            Health = health;
            Health.Died += OnDied;

            MoveTo(position);
            Rotate(rotation);
        }

        public event UnityAction Died;

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new System.ArgumentOutOfRangeException(nameof(damage));

            Health.TakeDamage(damage);
        }

        public void Update(float deltaTime)
        {
            MoveTo(Position);
            Rotate(Rotation);
        }

        private void OnDied()
        {
            Died?.Invoke();
            Health.Died -= OnDied;
        }
    }
}