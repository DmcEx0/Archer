using Agava.YandexGames;
using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    private static PlayerData s_instance;

    private const string _coinKey = "Coin";
    private const string _scoreKey = "Score";

    private const int _defaultCountCoins = 10;
    private const int _defaultScore = 0;

    private int _coins;
    private int _score;

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

    private void LoadData()
    {
        _coins = PlayerPrefs.HasKey(_coinKey) ? PlayerPrefs.GetInt(_coinKey) : _defaultCountCoins;
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt(_coinKey, _coins);
        PlayerPrefs.Save();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt(_scoreKey, _score);
        PlayerPrefs.Save();
    }
}