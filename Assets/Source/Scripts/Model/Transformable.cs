using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model
{
    public abstract class Transformable
    {
        public Transformable(Vector3 position, Quaternion rotation)
        {
            Rotation = rotation;
            Position = position;
        }

        public event UnityAction Rotated;
        public event UnityAction Moved;
        public event UnityAction Destroying;
        public event UnityAction DestroyingAll;

        public Quaternion Rotation { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Forward => Rotation * Vector3.forward;

        public void Rotate(Quaternion rotation)
        {
            Rotation = rotation;
            Rotated?.Invoke();
        }

        public void MoveTo(Vector3 position)
        {
            Position = position;
            Moved?.Invoke();
        }

        public void Destroy()
        {
            Destroying?.Invoke();
        }

        public void DestroyAll()
        {
            DestroyingAll?.Invoke();
        }
    }
}