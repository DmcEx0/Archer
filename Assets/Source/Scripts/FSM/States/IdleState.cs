using Archer.UI;

namespace Archer.Model.FSM
{
    public class IdleState : IState
    {
        private readonly CharacterStateMachine _stateMachine;
        private HealthBarView _healthBarView;

        public IdleState(CharacterStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            if (_healthBarView == null)
            {
                _healthBarView = _stateMachine.Character.Key.GetComponentInChildren<HealthBarView>();
                _healthBarView.Init(_stateMachine.Character.Value.Health);
            }

            _healthBarView.gameObject.SetActive(false);
        }

        public void Exit()
        {

        }

        public void Update(float deltaTime)
        {

        }
    }
}