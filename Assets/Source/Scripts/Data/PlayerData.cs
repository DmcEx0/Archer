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

        private string _language;

        private int _tutorialIsComplete;

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

        public string CurrentLanguage
        {
            get => _language;
            set
            {
                _language = value;
                SaveLanguage();
            }
        }

        public bool TutorialIsComplete
        {
            get
            {
                if (_tutorialIsComplete == 0)
                    return false;
                if (_tutorialIsComplete == 1)
                    return true;

                return false;
            }
            set
            {
                if (value == false)
                    _tutorialIsComplete = 0;
                if (value == true)
                    _tutorialIsComplete = 1;

                SaveTutorialState();
            }
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

        public event Action<int> CoinChanged;

        public static void Initialize()
        {
            _instance = new GameObject("PlayerData").AddComponent<PlayerData>();
            _instance.transform.SetParent(null);
            DontDestroyOnLoad(_instance);
            _instance.LoadData();
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
            _language = PlayerPrefs.HasKey(LanguageKey) ? PlayerPrefs.GetString(LanguageKey) : DefaultLanguage;
            _tutorialIsComplete = PlayerPrefs.HasKey(TutorialIsCompleteKey)
                ? PlayerPrefs.GetInt(TutorialIsCompleteKey)
                : DefaultTutorialState;
        }

        private void SaveMoney()
        {
            SaveIntData(CoinKey, _coins);
        }

        private void SaveScore()
        {
            SaveIntData(ScoreKey, _score);
        }

        private void SaveLevel()
        {
            SaveIntData(LevelKey, _level);
        }

        private void SaveArrow()
        {
            SaveIntData(ArrowIDKey, _arrowID);
        }

        private void SaveCrossbow()
        {
            SaveIntData(CrossbowIDKey, _crossbowID);
        }

        private void SaveLanguage()
        {
            SaveStringData(LanguageKey, _language);
        }

        private void SaveTutorialState()
        {
            SaveIntData(TutorialIsCompleteKey, _tutorialIsComplete);
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