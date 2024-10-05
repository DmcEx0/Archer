using System;

namespace  Archer.Utils
{
    public interface IGameSession
    {
        public void Init();
        public void Update();
        public void OnExitGame();

        public event Action EnemyDied;
        public Action<bool> LevelCompeted { get; set; }
    }
}