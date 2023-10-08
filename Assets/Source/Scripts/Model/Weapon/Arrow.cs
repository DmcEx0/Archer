using UnityEngine;

namespace Archer.Model
{
    public class Arrow : Transformable, IUpdatetable
    {
        private readonly float _lifetime = 4f;
        private float _accumulatedTime;

        private Vector3 _velocity;

        public Arrow(Vector3 position, Vector3 velocity) : base(position ,Quaternion.identity)
        {
            _velocity = velocity;
        }

        public int Damage { get; private set; }

        public void Init(Vector3 position, Quaternion rotation, int damage)
        {
            Damage = damage;

            SetPosition(position);
            SetRotation(rotation);
        }

        public void Update(float deltaTime)
        {
            SetPosition(Position + BallisticsRouter.GetCalculatedPosition(ref _velocity, deltaTime));

            SetRotation(Quaternion.LookRotation(_velocity));

            _accumulatedTime += deltaTime;

            if (_accumulatedTime >= _lifetime) 
            {
                Destroy();
            }
        }
    }
}