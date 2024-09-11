using System;
using UnityEngine;

namespace Archer.Model
{
    public class Character : SpawnedObject, ITickable
    {
        private readonly DamageProvider _damageProvider;

        public readonly Health Health;

        public Character(Vector3 position, Quaternion rotation, Health health) : base(position, rotation)
        {
            _damageProvider = new(this);

            Health = health;
            Health.Died += OnDied;
            _damageProvider.DamageReceived += OnApplyDamage;

            MoveTo(position);
            Rotate(rotation);
        }

        public event Action<int, int, ArrowSkillType> DamageReceived;
        public event Action<float, ArrowSkillType> SkillImpacted;
        public event Action Died;

        public void TakeDamage(int mainDamage, int additionalDamage, ArrowSkillType skillType)
        {
            if (mainDamage < 0)
                throw new ArgumentOutOfRangeException(nameof(mainDamage));

            DamageReceived?.Invoke(mainDamage, additionalDamage, skillType);
        }

        public void Tick(float deltaTime)
        {
            MoveTo(Position);
            Rotate(Rotation);
        }

        private void OnDied()
        {
            Died?.Invoke();
            Health.Died -= OnDied;
            _damageProvider.OnDestroy();
            _damageProvider.DamageReceived -= OnApplyDamage;
        }

        private void OnApplyDamage(int damage, bool isSkillImpact, float playingEffectTime, ArrowSkillType skillType)
        {
            Health.TakeDamage(damage);

            if (isSkillImpact)
                SkillImpacted?.Invoke(playingEffectTime, skillType);
        }
    }
}