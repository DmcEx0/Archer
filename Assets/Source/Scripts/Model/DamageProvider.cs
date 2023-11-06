using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public class DamageProvider
    {
        private const int DelayNoneType = 100;
        private const int NumberOfIterationNoneType = 0;

        private const int DelayFireTypeMillisecons = 400;
        private const int NumberOfIterationFireType = 5;

        private Character _character;

        public DamageProvider(Character character)
        {
            _character = character;
            _character.DamageReceived += TakeDamage;
        }

        public event UnityAction<int> DamageReceived;

        private async void ApplyPeriodicDamage(int damagePerIteration, int nuberOfIteration, int deltay)
        {
            for (int i = 0; i < nuberOfIteration; i++)
            {
                await Task.Delay(deltay);

                DamageReceived?.Invoke(damagePerIteration);
            }
        }

        private void ApplyInstantDamage(int damage)
        {
            DamageReceived?.Invoke(damage);
        }

        private void TakeDamage(int mainDamage, int additionalDamage, ArrowSkillType arrowSkillType)
        {
            switch (arrowSkillType)
            {
                case ArrowSkillType.None:
                    ApplyInstantDamage(mainDamage);
                    break;

                case ArrowSkillType.Fire:
                    ApplyPeriodicDamage(additionalDamage, NumberOfIterationFireType, DelayFireTypeMillisecons);
                    break;
            }
        }
    }
}