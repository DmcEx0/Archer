using Archer.AI;
using Archer.Model;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Scripts.FSM.States
{
    public class BattleState : IState
    {
        private readonly CharacterStateMachine _stateMachine;

        private AnimationController _animationController;

        private IInputRouter _inputRouter;

        private bool _isTakenPosition = false;

        public BattleState(CharacterStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _animationController = _stateMachine.Character.Key.GetComponent<AnimationController>();

            WaitForTakenPosition();
        }

        public void Exit()
        {
            DisableWeapon();
            WaitForDiskard();
        }

        public void Update(float deltaTime)
        {
            if (_isTakenPosition)
            {
                IGeneratable weaponGeneratable = _stateMachine.Weapon.Key;
                _stateMachine.Weapon.Value.SetArrowSpawnPoint(weaponGeneratable.GeneratingPoint.position);

                _animationController.EnabledIK(_stateMachine.Weapon.Key.RightHandTarget, _stateMachine.Weapon.Key.LeftHandTarget, _stateMachine.Weapon.Key.ChestTarget);
                _inputRouter.Update(deltaTime);
            }
        }

        private void InitPlayer()
        {
            Init(new PlayerInputRouter(_animationController));

            PowerShotBarView powerShotPresenter = _stateMachine.Character.Key.GetComponentInChildren<PowerShotBarView>();
            powerShotPresenter.Init(_inputRouter as PlayerInputRouter);
        }

        private void InitEnemy()
        {
            Init(new EnemyInputRouter(new EnemyAI(), _animationController));
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
            _animationController.PlaySitIdle();

            while (_stateMachine.Character.Value.Position != _stateMachine.EndPoint.position)
            {
                _stateMachine.Character.Value.MoveTo(_animationController.TakenPosition(_stateMachine.StartPosition, _stateMachine.EndPoint.position, Time.deltaTime));

                await Task.Yield();
            }

            if (_stateMachine.CharactersSpawner is PlayerSpawner)
                InitPlayer();

            if (_stateMachine.CharactersSpawner is EnemySpawner)
                InitEnemy();

            _isTakenPosition = true;
        }

        private async void WaitForDiskard()
        {
            float randomOffsetX = Random.Range(2f, 4f);

            Vector3 endPositon = new Vector3(_stateMachine.Character.Value.Position.x + randomOffsetX, 0, _stateMachine.Character.Value.Position.z);

            while (_stateMachine.Character.Key.AnimationController.IsFinalCurve == false)
            {
                _stateMachine.Character.Value.MoveTo(_stateMachine.Character.Key.AnimationController.TakenPosition2(_stateMachine.Character.Value.Position, endPositon, Time.deltaTime));

                await Task.Yield();
            }

            DisableCharacter();
        }

        private void OnDied()
        {
            _stateMachine.EnterIn(StatesEnum.Die);
        }

        private void DisableCharacter()
        {
            _stateMachine.Character.Value.Destroy();
            _stateMachine.Character.Value.Died -= OnDied;
        }
        private void DisableWeapon()
        {
            _stateMachine.Weapon.Value.Shoted -= _stateMachine.CharactersSpawner.OnShot;
            _stateMachine.Weapon.Value.DestroyAll();
            _inputRouter.OnDisable(); ;
        }
    }
}