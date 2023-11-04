using UnityEngine;

namespace Assets.Source.Scripts.FSM.States
{
    public class DieState : IState
    {
        private readonly CharacterStateMachine _stateMachine;

        public DieState(CharacterStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _stateMachine.Character.Key.GetComponentInChildren<HealthBarView>().gameObject.SetActive(false);

            _stateMachine.OnDied();
        }

        public void Exit()
        {

        }

        public void Update(float deltaTime)
        {

        }
    }
}