using Archer.Model;
using UnityEngine;

namespace Archer.Model.FSM
{
    public class IdleState : IState
    {
        private readonly CharacterStateMachine _stateMachine;

        public IdleState(CharacterStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            HealthBarView enemyHealthBart = _stateMachine.Character.Key.GetComponentInChildren<HealthBarView>();
            enemyHealthBart.Init(_stateMachine.Character.Value.Health);

            enemyHealthBart.gameObject.SetActive(false);
        }

        public void Exit()
        {

        }

        public void Update(float deltaTime)
        {

        }
    }
}