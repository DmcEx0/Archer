using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public abstract class Transformable
    {
        public Transformable(Vector3 position  ,Quaternion rotation)
        {
            Rotation = rotation;
            Position = position;
        }

        public Quaternion Rotation { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Forward => Rotation * Vector3.forward;

        public event UnityAction Rotated;
        public event UnityAction ChangedPosition;
        public event UnityAction Destroying;

        protected void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
            Rotated?.Invoke();
        }

        protected void SetPosition(Vector3 position)
        {
            Position = position;
            ChangedPosition?.Invoke();
        }

        public void Destroy()
        {
            Destroying?.Invoke();
        }
    }
}