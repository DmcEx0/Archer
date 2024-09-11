using Archer.AI;
using Archer.Input;
using Archer.Presenters;
using Archer.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Archer.Model.FSM
{
    public class BattleState : IState
    {
        private readonly CharacterStateMachine _stateMachine;
        private PowerShotBarView _powerShotBarView;

        private IInputRouter _inputRouter;

        private bool _isTakenPosition = false;
        private bool _isInitialized = false;

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
            _stateMachine.Character.Value.Died -= ChangeStateToDie;
        }

        public void Update(float deltaTime)
        {
            if (_isTakenPosition)
            {
                IGeneratable weaponGeneratable = _stateMachine.Weapon.Key;
                _stateMachine.Weapon.Value.SetArrowSpawnPoint(weaponGeneratable.GeneratingPoint.position);
                _stateMachine.Character.Key.AnimationHandler.EnableIK(_stateMachine.Weapon.Key.RightHandTarget,
                    _stateMachine.Weapon.Key.LeftHandTarget, _stateMachine.Weapon.Key.ChestTarget);
                _inputRouter.Update(deltaTime);
            }
        }

        private void InitPlayer()
        {
            Initialize(new PlayerInputRouter(_stateMachine.Character.Key.AnimationHandler));

            _powerShotBarView = _stateMachine.Character.Key.GetComponentInChildren<PowerShotBarView>();
            _powerShotBarView.Init(_inputRouter as PlayerInputRouter);
        }

        private void InitEnemy()
        {
            Initialize(new EnemyInputRouter(new EnemyAI(), _stateMachine.Character.Key.AnimationHandler));
        }

        private void Initialize(IInputRouter inputRouter)
        {
            _isInitialized = true;
            _inputRouter = inputRouter;
            _inputRouter.BindWeapon(_stateMachine.Weapon.Value);
            _stateMachine.Weapon.Value.PressingUI += _inputRouter.SetGainingPowerState;
        }

        private void StartBattle()
        {
            _stateMachine.Character.Value.Rotate(_stateMachine.EndPoint.rotation);

            IGeneratable characterGeneratable = _stateMachine.Character.Key;

            _stateMachine.Weapon.Key.gameObject.SetActive(true);
            _stateMachine.Weapon.Value.MoveTo(characterGeneratable.GeneratingPoint.position);
            _stateMachine.Weapon.Value.Rotate(characterGeneratable.GeneratingPoint.rotation);

            _stateMachine.Weapon.Value.Shooting += _stateMachine.CharacterSpawner.OnShot;
            _inputRouter.OnEnable();

            _stateMachine.Character.Key.GetComponentInChildren<HealthBarView>(true).gameObject.SetActive(true);
            _stateMachine.Character.Value.Died += ChangeStateToDie;
        }

        private async void WaitForTakenPosition()
        {
            if (_stateMachine.Character.Value == null)
                return;

            _stateMachine.Character.Key.AnimationHandler.PlaySitIdle();

            if (_stateMachine.CharacterSpawner is Player)
                InitPlayer();

            if (_stateMachine.CharacterSpawner is Enemy)
                InitEnemy();

            while (_stateMachine.Character.Key.AnimationHandler.IsTakenPosition == false)
            {
                if (_stateMachine.Character.Key != null)
                    _stateMachine.Character.Value.MoveTo(
                        _stateMachine.Character.Key.AnimationHandler.GetTakenPosition(_stateMachine.StartPosition,
                            _stateMachine.EndPoint.position, Time.deltaTime));

                await UniTask.Yield();
            }

            StartBattle();

            _isTakenPosition = true;
        }

        private void ChangeStateToDie()
        {
            _stateMachine.EnterIn(StatesType.Die);
        }

        private void DisableWeapon()
        {
            if (_isInitialized)
            {
                _stateMachine.Weapon.Value.PressingUI -= _inputRouter.SetGainingPowerState;
                _inputRouter.OnDisable();

                if (_stateMachine.Character.Key is PlayerPresenter)
                    _powerShotBarView.gameObject.SetActive(false);
            }

            _stateMachine.Weapon.Value.Shooting -= _stateMachine.CharacterSpawner.OnShot;
            _stateMachine.Weapon.Value.Destroy();
        }
    }
}