using System;
using System.Collections.Generic;
using Archer.Presenters;
using UnityEngine;

namespace Archer.Model.FSM
{
    public class CharacterStateMachine
    {
        private readonly Dictionary<StatesType, IState> _states;
        private IState _currentState;

        public CharacterStateMachine(KeyValuePair<CharacterPresenter, Character> character,
            KeyValuePair<WeaponPresenter, Weapon> weapon, CharacterFactory characterFactory, Vector3 startPosition,
            Transform endPoint)
        {
            _states = new Dictionary<StatesType, IState>()
            {
                [StatesType.Idle] = new IdleState(this),
                [StatesType.Battle] = new BattleState(this),
                [StatesType.Die] = new DieState(this),
            };

            CharacterFactory = characterFactory;
            Character = character;
            Weapon = weapon;
            StartPosition = startPosition;
            EndPoint = endPoint;
        }

        public event Action Died;

        public Vector3 StartPosition { get; private set; }
        public Transform EndPoint { get; private set; }
        public CharacterFactory CharacterFactory { get; private set; }
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