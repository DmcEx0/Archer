using System;
using UnityEngine;

namespace Archer.Model
{
    public abstract class SpawnedObject
    {
        public SpawnedObject(Vector3 position, Quaternion rotation)
        {
            Rotation = rotation;
            Position = position;
        }

        public event Action Rotating;
        public event Action Moving;
        public event Action Destroying;
        public event Action DestroyingAll;

        public Quaternion Rotation { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Forward => Rotation * Vector3.forward;

        public void Rotate(Quaternion rotation)
        {
            Rotation = rotation;
            Rotating?.Invoke();
        }

        public void MoveTo(Vector3 position)
        {
            Position = position;
            Moving?.Invoke();
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