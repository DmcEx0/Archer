using Agava.YandexGames;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    private static PlayerData s_instance;

    private const string CoinKey = "Coin";
    private const string ScoreKey = "Score";
    private const string LevelKey = "Level";

    private const int DefaultCountCoins = 10;
    private const int DefaultScore = 0;
    private const int DefaultLevel = 1;

    private const int MaxCountLevel = 5;

    private int _coins;
    private int _score;
    private int _level;

    public int Coins
    {
        get { return _coins; }
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _coins = value;
            CoinChanged?.Invoke(_coins);
            SaveMoney();
        }
    }

    public int Score
    {
        get { return _score; }
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _score = value;
            SaveScore();
        }
    }

    public int Level
    {
        get { return _level; }
    }

    public static PlayerData Instance
    {
        get
        {
            if (s_instance == null)
                Initialize();

            return s_instance;
        }
    }

    public event UnityAction<int> CoinChanged;

    public static void Initialize()
    {
        s_instance = new GameObject("PlayerData").AddComponent<PlayerData>();
        s_instance.transform.SetParent(null);
        DontDestroyOnLoad(s_instance);
        s_instance.LoadData();
    }

    public void CompleteLevel()
    {
        if (_level > MaxCountLevel)
            throw new InvalidOperationException();

        _level += 1;
        SaveLevel();
    }

    private void LoadData()
    {
        _coins = PlayerPrefs.HasKey(CoinKey) ? PlayerPrefs.GetInt(CoinKey) : DefaultCountCoins;
        _score = PlayerPrefs.HasKey(ScoreKey) ? PlayerPrefs.GetInt(ScoreKey) : DefaultScore;
        _level = PlayerPrefs.HasKey(LevelKey) ? PlayerPrefs.GetInt(LevelKey) : DefaultLevel;
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt(CoinKey, _coins);
        PlayerPrefs.Save();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt(ScoreKey, _score);
        PlayerPrefs.Save();
    }

    private void SaveLevel()
    {
        PlayerPrefs.SetInt(LevelKey, _level);
        PlayerPrefs.Save();
    }
}