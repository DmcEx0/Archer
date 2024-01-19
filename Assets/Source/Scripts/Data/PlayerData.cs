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
    private const string ArrowIDKey = "ArrowID";
    private const string CrossbowIDKey = "CrossbowID";

    private const int DefaultCountCoins = 10;
    private const int DefaultScore = 0;
    private const int DefaultLevel = 1;
    private const int DefaultArrowID = 1;
    private const int DefaultCrossbowID = 1;

    private const int MaxCountLevel = 5;

    private int _coins;
    private int _score;
    private int _level;
    private int _arrowID;
    private int _crossbowID;

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

    public int ArrowID
    {
        get => _arrowID;
        set
        {
            _arrowID = value;
            SaveArrow();
        }
    }
    public int CrossbowID
    {
        get => _crossbowID;
        set
        {
            _crossbowID = value;
            SaveCrossbow();
        }
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
            return;

        _level += 1;
        SaveLevel();
    }

    private void LoadData()
    {
        _coins = PlayerPrefs.HasKey(CoinKey) ? PlayerPrefs.GetInt(CoinKey) : DefaultCountCoins;
        _score = PlayerPrefs.HasKey(ScoreKey) ? PlayerPrefs.GetInt(ScoreKey) : DefaultScore;
        _level = PlayerPrefs.HasKey(LevelKey) ? PlayerPrefs.GetInt(LevelKey) : DefaultLevel;
        _arrowID = PlayerPrefs.HasKey(ArrowIDKey) ? PlayerPrefs.GetInt(ArrowIDKey) : DefaultArrowID;
        _crossbowID = PlayerPrefs.HasKey(CrossbowIDKey) ? PlayerPrefs.GetInt(CrossbowIDKey) : DefaultCrossbowID;
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

    private void SaveArrow()
    {
        PlayerPrefs.SetInt(ArrowIDKey, _arrowID);
        PlayerPrefs.Save();
    }

    private void SaveCrossbow()
    {
        PlayerPrefs.SetInt(CrossbowIDKey, _crossbowID);
        PlayerPrefs.Save();
    }
}