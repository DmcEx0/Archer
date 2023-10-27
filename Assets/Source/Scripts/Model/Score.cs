using UnityEngine;

namespace Archer.Model
{
    public class Score
    {
        private const int _numberOfScore = 10;

        public int AmountCoins { get; private set; } = 0;
        private int _score = 0;

        public void AddCoinsOnKill(int coins)
        {
            AmountCoins += coins;
        }
    }
}