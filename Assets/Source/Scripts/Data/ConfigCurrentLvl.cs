using UnityEngine;

namespace Archer.Data
{
    public class ConfigCurrentLvl : MonoBehaviour
    {
        [SerializeField] private int _coinsForEnemy;
        [SerializeField] private int _scoreForEnemy;

        [Space]
        [SerializeField] private int _minHealthEnemy;
        [SerializeField] private int _maxHealthEnemy;

        [Space]
        [SerializeField] private int _playerHealth;

        public int CoinsForEnemy => _coinsForEnemy;
        public int ScoreForEnemy => _scoreForEnemy;
        public int MinHealthEnemy => _minHealthEnemy;
        public int MaxHealthEnemy => _maxHealthEnemy;
        public int PlayerHealth => _playerHealth;
    }
}