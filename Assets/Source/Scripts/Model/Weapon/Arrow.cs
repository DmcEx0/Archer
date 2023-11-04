using UnityEngine;

namespace Archer.Model
{
    public class Arrow : Transformable, IUpdatetable
    {
        private readonly float _lifetime = 4f;
        private float _accumulatedTime;

        private Vector3 _velocity;

        public Arrow(Vector3 position, Quaternion rotation, Vector3 velocity) : base(position, rotation)
        {
            _velocity = velocity;
            MoveTo(position);
            Rotate(rotation);
        }

        public int Damage { get; private set; }

        public void SetDamage(int damage)
        {
            Damage = damage;
        }

        public void Update(float deltaTime)
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