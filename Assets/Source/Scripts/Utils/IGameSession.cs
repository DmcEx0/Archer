using UnityEngine.Events;

namespace  Archer.Utils
{
    public interface IGameSession
    {
        public void Init();
        public void Update();
        public void OnExitGame();

        public event UnityAction EnemyDied;
        public event UnityAction<bool> LevelCompete;
    }
}