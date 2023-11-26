using System.Threading.Tasks;
using UnityEngine.Events;

namespace Archer.Model
{
    public class DamageProvider
    {
        private const int NumberMillisecondsPerSecond = 1000;

        private const int DelayDefaultType = 0;
        private const int NumberOfIterationDefaultType = 1;

        private const int DelayFireTypeMillisecons = 400;
        private const int NumberOfIterationFireType = 5;

        private const int DelayPoisonTypeMillisecons = 300;
        private const int NumberOfIterationPoisonType = 10;

        private Character _character;

        private bool _isSkillImpact;

        public DamageProvider(Character character)
        {
            _character = character;
            _character.DamageReceived += TakeDamage;
        }

        public event UnityAction<int, bool, float, ArrowSkillType> DamageReceived;

        public void OnDestroy()
        {
            _character.DamageReceived -= TakeDamage;
        }

        private async void ApplyPeriodicDamage(int damagePerIteration, int nuberOfIteration, int deltay, ArrowSkillType skillType)
        {
            float playingEffectTime = (nuberOfIteration * deltay) / NumberMillisecondsPerSecond;

            for (int i = 0; i < nuberOfIteration; i++)
            {
                await Task.Delay(deltay);

                _isSkillImpact = true;
                DamageReceived?.Invoke(damagePerIteration, _isSkillImpact, playingEffectTime, skillType);
            }
        }

        private void ApplyInstantDamage(int damage)
        {
            _isSkillImpact = false;
            DamageReceived?.Invoke(damage, _isSkillImpact, 0, ArrowSkillType.None);
        }

        private void TakeDamage(int mainDamage, int additionalDamage, ArrowSkillType arrowSkillType)
        {
            switch (arrowSkillType)
            {
                case ArrowSkillType.None:
                    ApplyInstantDamage(mainDamage);
                    break;

                case ArrowSkillType.Default:
                    ApplyPeriodicDamage(additionalDamage, NumberOfIterationDefaultType, DelayDefaultType, ArrowSkillType.Default);
                    break;

                case ArrowSkillType.Fire:
                    ApplyPeriodicDamage(additionalDamage, NumberOfIterationFireType, DelayFireTypeMillisecons, ArrowSkillType.Fire);
                    break;

                case ArrowSkillType.Poison:
                    ApplyPeriodicDamage(additionalDamage, NumberOfIterationPoisonType, DelayPoisonTypeMillisecons, ArrowSkillType.Poison);
                    break;
            }
        }
    }
}