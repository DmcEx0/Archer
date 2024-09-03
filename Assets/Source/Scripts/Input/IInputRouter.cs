using Archer.Model;
using System;
using UnityEngine.Events;

namespace Archer.Input
{
    public interface IInputRouter
    {
        public IInputRouter BindWeapon(Weapon weapon);
        public void OnEnable();
        public void OnDisable();
        public void Update(float deltaTime);

        public void CanGainingPower(bool isCanNot);
    }
}