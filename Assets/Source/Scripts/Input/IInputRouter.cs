using Archer.Model;
using UnityEngine;

public interface IInputRouter 
{
    public IInputRouter BindWeapon(Weapon weapon);
    public void OnEnable();
    public void OnDisable();
    public void Update(float deltaTime);
}