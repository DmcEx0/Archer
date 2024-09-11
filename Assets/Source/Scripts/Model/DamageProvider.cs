using Cysharp.Threading.Tasks;
using System;
using UnityEngine.LowLevel;

namespace Archer.Model
{
    public class DamageProvider
    {
        private const int DelayDefaultType = 0;
        private const int NumberOfIterationDefaultType = 1;

        private const float DelayFireTypeMilliseconds = 0.4f;
        private const int NumberOfIterationFireType = 4;

        private const float DelayPoisonTypeMilliseconds = 0.3f;
        private const int NumberOfIterationPoisonType = 6;

        private readonly Character _character;

        private bool _isSkillImpact;

        public DamageProvider(Character character)
        {
            _character = character;
            _character.DamageReceived += OnTakeDamage;
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelper.Initialize(ref loop, InjectPlayerLoopTimings.Minimum);
        }

        public event Action<int, bool, float, ArrowSkillType> DamageReceived;

        public void OnDestroy()
        {
            _character.DamageReceived -= OnTakeDamage;
        }

        private async void ApplyPeriodicDamage(int damagePerIteration, int numberOfIteration, float delay,
            ArrowSkillType skillType)
        {
            float playingEffectTime = (numberOfIteration * delay);

            for (int i = 0; i < numberOfIteration; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), DelayType.DeltaTime, PlayerLoopTiming.Update);

                _isSkillImpact = true;
                DamageReceived?.Invoke(damagePerIteration, _isSkillImpact, playingEffectTime, skillType);
            }
        }

        private void ApplyInstantDamage(int damage)
        {
            _isSkillImpact = false;
            DamageReceived?.Invoke(damage, _isSkillImpact, 0, ArrowSkillType.None);
        }

        private void OnTakeDamage(int mainDamage, int additionalDamage, ArrowSkillType arrowSkillType)
        {
            switch (arrowSkillType)
            {
                case ArrowSkillType.None:
                    ApplyInstantDamage(mainDamage);
                    break;

                case ArrowSkillType.Default:
                    ApplyPeriodicDamage(additionalDamage, NumberOfIterationDefaultType, DelayDefaultType,
                        ArrowSkillType.Default);
                    break;

                case ArrowSkillType.Fire:
                    ApplyPeriodicDamage(additionalDamage, NumberOfIterationFireType, DelayFireTypeMilliseconds,
                        ArrowSkillType.Fire);
                    break;

                case ArrowSkillType.Poison:
                    ApplyPeriodicDamage(additionalDamage, NumberOfIterationPoisonType, DelayPoisonTypeMilliseconds,
                        ArrowSkillType.Poison);
                    break;
            }
        }
    }
}