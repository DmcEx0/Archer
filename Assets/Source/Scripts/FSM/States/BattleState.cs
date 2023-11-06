using Archer.AI;
using System.Threading.Tasks;
using UnityEngine;

namespace Archer.Model.FSM
{
    public class BattleState : IState
    {
        private readonly CharacterStateMachine _stateMachine;

        private IInputRouter _inputRouter;

        private bool _isTakenPosition = false;

        public BattleState(CharacterStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            WaitForTakenPosition();
        }

        public void Exit()
        {
            DisableWeapon();
            _stateMachine.Character.Value.Died -= OnDied;
        }

        public void Update(float deltaTime)
        {
            if (_isTakenPosition)
            {
                IGeneratable weaponGeneratable = _stateMachine.Weapon.Key;
                _stateMachine.Weapon.Value.SetArrowSpawnPoint(weaponGeneratable.GeneratingPoint.position);

                _stateMachine.Character.Key.AnimationController.EnabledIK(_stateMachine.Weapon.Key.RightHandTarget, _stateMachine.Weapon.Key.LeftHandTarget, _stateMachine.Weapon.Key.ChestTarget);
                _inputRouter.Update(deltaTime);
            }
        }

        private void InitPlayer()
        {
            Init(new PlayerInputRouter(_stateMachine.Character.Key.AnimationController));

            PowerShotBarView powerShotPresenter = _stateMachine.Character.Key.GetComponentInChildren<PowerShotBarView>();
            powerShotPresenter.Init(_inputRouter as PlayerInputRouter);
        }

        private void InitEnemy()
        {
            Init(new EnemyInputRouter(new EnemyAI(), _stateMachine.Character.Key.AnimationController));
        }

        private void Init(IInputRouter inputRouter)
        {
            _stateMachine.Character.Value.Rotate(_stateMachine.EndPoint.rotation);

            IGeneratable characterGeneratable = _stateMachine.Character.Key as IGeneratable;

            _stateMachine.Weapon.Key.gameObject.SetActive(true);
            _stateMachine.Weapon.Value.MoveTo(characterGeneratable.GeneratingPoint.position);
            _stateMachine.Weapon.Value.Rotate(characterGeneratable.GeneratingPoint.rotation);

            _inputRouter = inputRouter;

            _inputRouter.BindWeapon(_stateMachine.Weapon.Value);
            _stateMachine.Weapon.Value.Shoted += _stateMachine.CharactersSpawner.OnShot;
            _inputRouter.OnEnable();

            _stateMachine.Character.Key.GetComponentInChildren<HealthBarView>(true).gameObject.SetActive(true);

            _stateMachine.Character.Value.Died += OnDied;
        }

        private async void WaitForTakenPosition()
        {
            if (_stateMachine.Character.Value == null)
                return;

            _stateMachine.Character.Key.AnimationController.PlaySitIdle();

            while (_stateMachine.Character.Key.AnimationController.IsTakenPosition == false)
            {
                _stateMachine.Character.Value.MoveTo(_stateMachine.Character.Key.AnimationController.TakenPosition(_stateMachine.StartPosition, _stateMachine.EndPoint.position, Time.deltaTime));

                await Task.Yield();
            }

            if (_stateMachine.CharactersSpawner is PlayerSpawner)
                InitPlayer();

            if (_stateMachine.CharactersSpawner is EnemySpawner)
                InitEnemy();

            _isTakenPosition = true;
        }

        private void OnDied()
        {
            _stateMachine.EnterIn(StatesType.Die);
        }

        private void DisableWeapon()
        {
            _stateMachine.Weapon.Value.Shoted -= _stateMachine.CharactersSpawner.OnShot;
            _stateMachine.Weapon.Value.Destroy();
            _inputRouter.OnDisable();
        }
    }
}