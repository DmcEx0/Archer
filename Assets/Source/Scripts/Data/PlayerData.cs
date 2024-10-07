using System;
using Archer.Yandex;
using UnityEngine;

namespace Archer.Data
{
    public class PlayerData : MonoBehaviour
    {
        private static PlayerData _instance;

        private const string CoinKey = "Coin";
        private const string ScoreKey = "Score";
        private const string LevelKey = "Level";
        private const string ArrowIDKey = "ArrowID";
        private const string CrossbowIDKey = "CrossbowID";
        private const string LanguageKey = "Language";
        private const string TutorialIsCompleteKey = "TutorialIsComplete";

        private const string DefaultLanguage = Language.ENG;

        private const int DefaultTutorialState = 0;

        private const int DefaultCountCoins = 0;
        private const int DefaultScore = 0;
        private const int DefaultLevel = 1;
        private const int DefaultArrowID = 1;
        private const int DefaultCrossbowID = 1;

        private const int MaxCountLevel = 4;

        private int _tutorialIsComplete;

        public string CurrentLanguage{ get; private set; }
        public int Coins { get; private set; }
        public int Score { get; private set; }
        public int Level{ get; private set; }
        public int ArrowID{ get; private set; }
        public int CrossbowID{ get; private set; }
        public bool TutorialIsComplete => GetTutorialIsCompleteState();

        public event Action<int> CoinChanged;

        public static void Initialize()
        {
            _instance = new GameObject("PlayerData").AddComponent<PlayerData>();
            _instance.transform.SetParent(null);
            DontDestroyOnLoad(_instance);
            _instance.LoadData();
        }

        private void LoadData()
        {
            Coins = PlayerPrefs.HasKey(CoinKey) ? PlayerPrefs.GetInt(CoinKey) : DefaultCountCoins;
            Score = PlayerPrefs.HasKey(ScoreKey) ? PlayerPrefs.GetInt(ScoreKey) : DefaultScore;
            Level = PlayerPrefs.HasKey(LevelKey) ? PlayerPrefs.GetInt(LevelKey) : DefaultLevel;
            ArrowID = PlayerPrefs.HasKey(ArrowIDKey) ? PlayerPrefs.GetInt(ArrowIDKey) : DefaultArrowID;
            CrossbowID = PlayerPrefs.HasKey(CrossbowIDKey) ? PlayerPrefs.GetInt(CrossbowIDKey) : DefaultCrossbowID;
            CurrentLanguage = PlayerPrefs.HasKey(LanguageKey) ? PlayerPrefs.GetString(LanguageKey) : DefaultLanguage;
            _tutorialIsComplete = PlayerPrefs.HasKey(TutorialIsCompleteKey)
                ? PlayerPrefs.GetInt(TutorialIsCompleteKey)
                : DefaultTutorialState;
        }
        
        public void CompleteLevel()
        {
            if (Level > MaxCountLevel)
                return;

            Level += 1;
            SaveIntData(LevelKey, Level);
        }
        
        public void SetCoins(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            Coins = value;
            CoinChanged?.Invoke(Coins);
            SaveIntData(CoinKey, Coins);
        }
        
        public void SetScore(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            Score = value;
            SaveIntData(ScoreKey, Score);
        }
        
        public void SetArrowID(int value)
        {
            ArrowID = value;
            SaveIntData(ArrowIDKey, ArrowID);
        }

        public void SetCrossbowID(int value)
        {
            CrossbowID = value;
            SaveIntData(CrossbowIDKey, CrossbowID);

        }
        
        public void SetCurrentLanguage(string value)
        {
            CurrentLanguage = value;
            SaveStringData(LanguageKey, CurrentLanguage);
        }
        
        public void SetTutorialIsComplete(bool value)
        {
            if (value == false)
                _tutorialIsComplete = 0;
            if (value == true)
                _tutorialIsComplete = 1;

            SaveIntData(TutorialIsCompleteKey, _tutorialIsComplete);
        }
        
        public static PlayerData Instance
        {
            get
            {
                if (_instance == null)
                    Initialize();

                return _instance;
            }
        }
        
        private bool GetTutorialIsCompleteState()
        {
            if (_tutorialIsComplete == 0)
                return false;
            if (_tutorialIsComplete == 1)
                return true;

            return false;
        }

        private void SaveIntData(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
        
        private void SaveStringData(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }
    }
}