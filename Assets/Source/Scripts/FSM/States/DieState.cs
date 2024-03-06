using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Archer.Model.FSM
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
            _stateMachine.Character.Key.AnimationController.PlayDeaht();
            _stateMachine.OnDied();

            _stateMachine.Weapon.Value.DestroyAll();

            WaitForDiskard();
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
        }

        private async void WaitForDiskard()
        {
            float offsetX = 13;

            if (_stateMachine.Character.Key is PlayerPresenter)
                offsetX *= -1;

            Vector3 endPositon = new Vector3(_stateMachine.Character.Value.Position.x + offsetX, _stateMachine.Character.Value.Position.y, _stateMachine.Character.Value.Position.z);

            while (_stateMachine.Character.Key.AnimationController.IsDiskard == false)
            {
                _stateMachine.Character.Value.MoveTo(_stateMachine.Character.Key.AnimationController.Diskard(_stateMachine.Character.Value.Position, endPositon, Time.deltaTime));

                await UniTask.Yield();
            }

            _stateMachine.Character.Value.Destroy();
        }
    }
}