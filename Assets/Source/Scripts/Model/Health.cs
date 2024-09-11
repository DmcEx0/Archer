using System;
using UnityEngine;

namespace Archer.Model
{
    public class Health
    {
        private readonly int _minValue = 0;
        private readonly int _maxValue;

        private int _currentValue;

        public Health(int maxValue)
        {
            _currentValue = maxValue;
            _maxValue = maxValue;
        }

        public event Action<int, int> ValueChanged;
        public event Action Died;

        public int Value => _currentValue;

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            _currentValue = Mathf.Clamp(_currentValue - damage, _minValue, _maxValue);

            ValueChanged?.Invoke(_currentValue, _maxValue);
            TryDie();
        }

        private void TryDie()
        {
            if (_currentValue == 0)
                Died?.Invoke();
        }
    }
}