using IJunior.TypedScenes;
using System.Collections.Generic;
using Archer.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Archer.Utils
{
    public static class LevelManager
    {
        private const int FirstLevelKey = 0;
    
        private static readonly List<int> _levelsKeys = new();
        private static int _currentIndexLevelKey = FirstLevelKey;
    
        private static readonly Dictionary<int, UnityAction> _levels = new Dictionary<int, UnityAction>
        {
            { 1, () => Level1.Load() },
            { 2, () => Level2.Load() },
            { 3, () => Level3.Load() },
            { 4, () => Level5.Load() },
        };
    
        public static void LoadNextLevel()
        {
            int currentLevel = PlayerData.Instance.Level;
    
            if (currentLevel >= _levels.Count + 1)
            {
                LoadRandomLevel();
                return;
            }
    
            if (_levels.ContainsKey(currentLevel))
            {
                _levels[currentLevel]();
            }
        }
    
        private static void LoadRandomLevel()
        {
            if (_currentIndexLevelKey == _levelsKeys.Count)
            {
                _currentIndexLevelKey = FirstLevelKey;
                _levelsKeys.Clear();
                Shaffle();
            }
    
            _levels[_levelsKeys[_currentIndexLevelKey]]();
            _currentIndexLevelKey++;
        }
    
        private static void Shaffle()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                int key = i + 1;
                _levelsKeys.Add(key);
            }
    
            int n = _levelsKeys.Count;
    
            while (n > 1)
            {
                n--;
    
                int x = Random.Range(0, n + 1);
    
                int value = _levelsKeys[x];
    
                _levelsKeys[x] = _levelsKeys[n];
                _levelsKeys[n] = value;
            }
        }
    }
}