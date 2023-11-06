using System;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class Character : Transformable, IUpdatetable
    {
        private DamageProvider _damageProvider;

        public readonly Health Health;

        public Character(Vector3 position, Quaternion rotation, Health health) : base(position, rotation)
        {
            _damageProvider = new(this);

            Health = health;
            Health.Died += OnDied;
            _damageProvider.DamageReceived += Damage;

            MoveTo(position);
            Rotate(rotation);
        }

        public event UnityAction<int, int, ArrowSkillType> DamageReceived;

        public event UnityAction Died;

        public void TakeDamage(int mainDamage, int additionalDamage, ArrowSkillType skillType)
        {
            if (mainDamage < 0)
                throw new ArgumentOutOfRangeException(nameof(mainDamage));

            DamageReceived?.Invoke(mainDamage, additionalDamage, skillType);
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
            _damageProvider.DamageReceived -= Damage;
        }

        private void Damage(int damage)
        {
            Health.TakeDamage(damage);
        }
    }
}