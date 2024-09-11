using Archer.Utils;
using UnityEngine;

namespace Archer.Model
{
    public class Arrow : SpawnedObject, ITickable
    {
        private readonly float _lifetime = 4f;
        private readonly bool _isActivatedSkill;

        private float _accumulatedTime;

        private Vector3 _velocity;

        public Arrow(Vector3 position, Quaternion rotation, Vector3 velocity, bool isActivatedSkill) : base(position, rotation)
        {
            _velocity = velocity;
            _isActivatedSkill = isActivatedSkill;

            MoveTo(position);
            Rotate(rotation);
        }

        public int Damage { get; private set; }
        public int AdditionalDamage { get; private set; }
        public ArrowSkillType SkillType { get; private set; }

        public void SetDamage(int mainDamage, int additionalDamage, ArrowSkillType skillType)
        {
            Damage = mainDamage;
            AdditionalDamage = additionalDamage;

            if (_isActivatedSkill == false)
                SkillType = ArrowSkillType.None;
            else
                SkillType = skillType;
        }

        public void Tick(float deltaTime)
        {
            MoveTo(Position + BallisticsRouter.GetCalculatedPosition(ref _velocity, deltaTime));

            Rotate(Quaternion.LookRotation(_velocity));

            _accumulatedTime += deltaTime;

            if (_accumulatedTime >= _lifetime)
            {
                DestroyAll();
            }
        }
    }
}