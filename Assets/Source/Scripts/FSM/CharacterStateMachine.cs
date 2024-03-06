using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Model.FSM
{
    public class CharacterStateMachine
    {
        private Dictionary<StatesType, IState> _states;
        private IState _currentState;

        public CharacterStateMachine(KeyValuePair<CharacterPresenter, Character> character, KeyValuePair<WeaponPresenter, Weapon> weapon, CharactersSpawner charactersSpawner, Vector3 startPosition, Transform endPoint)
        {
            _states = new Dictionary<StatesType, IState>()
            {
                [StatesType.Idle] = new IdleState(this),
                [StatesType.Battle] = new BattleState(this),
                [StatesType.Die] = new DieState(this),
            };

            CharactersSpawner = charactersSpawner;
            Character = character;
            Weapon = weapon;
            StartPosition = startPosition;
            EndPoint = endPoint;
        }

        public event UnityAction Died;

        public Vector3 StartPosition { get; private set; }
        public Transform EndPoint { get; private set; }
        public CharactersSpawner CharactersSpawner { get; private set; }
        public KeyValuePair<CharacterPresenter, Character> Character { get; private set; }
        public KeyValuePair<WeaponPresenter, Weapon> Weapon { get; private set; }

        public void EnterIn(StatesType stateEnum)
        {
            if (_states.TryGetValue(stateEnum, out IState state))
            {
                _currentState?.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }

        public void Update(float deltaTime)
        {
            _currentState.Update(deltaTime);
        }

        public void OnDied()
        {
            Died?.Invoke();
        }
    }
}