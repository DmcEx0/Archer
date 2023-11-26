namespace Archer.Model
{
    public class RevardSystem
    {
        private int _score = 0;
        private int _coins = 0;
        public int AmountCoins { get; private set; } = 0;
        public int AmountScore { get; private set; } = 0;

        public void AddCoinsOnKill(int coins) => AmountCoins += coins;

        public void AddScoreOnKill(int score) => AmountScore += score;

        public void Reset()
        {
            AmountCoins = 0;
            AmountScore = 0;
        }
    }
}