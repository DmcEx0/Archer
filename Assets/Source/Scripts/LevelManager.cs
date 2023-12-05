using IJunior.TypedScenes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager
{
    private Dictionary<int, UnityAction> _levels = new Dictionary<int, UnityAction>
    {
        { 1, () => Level1.Load() },
        { 2, () => Level2.Load() },
        { 3, () => Level3.Load() },
        { 4, () => Level4.Load() },
        { 5, () => Level5.Load() },
    };

    public void LoadNextLevel()
    {
        int currentLevel = PlayerData.Instance.Level;

        if (currentLevel + 1 > 5)
            LoadRandomLevel();

        if (_levels.ContainsKey(currentLevel))
        {
            _levels[currentLevel]();
        }
    }

    private void LoadRandomLevel()
    {
        int randomLevel = Random.Range(0, 6);

        _levels[randomLevel]();
    }
}