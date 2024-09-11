using Agava.YandexGames;
using System.Collections.Generic;
using Archer.Data;
using Archer.Yandex;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class Leaderboard : MonoBehaviour
    {
        private const string AnunymousNameENG = "Anonymous";
        private const string AnunymousNameRUS = "Анонимус";
        private const string AnunymousNameTUR = "Anonim";
        private const string LeaderboardName = "ArcherLeaderboard";

        [SerializeField] private LeaderboardAuthorizedView _authorizedView;

        [SerializeField] private LeaderboardView _leaderboardView;
        [SerializeField] private Button _showLeaderboard;

        private readonly List<LeaderboardPlayer> _leaderboardPlayers = new();
        private readonly int _numberOfPlayerInLeaderboard = 4;

        private void OnEnable()
        {
            _showLeaderboard.onClick.AddListener(OnShow);
        }

        private void OnDisable()
        {
            _showLeaderboard.onClick.RemoveListener(OnShow);
        }

        public void SetPlayerScore()
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            Agava.YandexGames.Leaderboard.GetPlayerEntry(LeaderboardName, result =>
            {
                if (PlayerData.Instance.Score >= result.score)
                    Agava.YandexGames.Leaderboard.SetScore(LeaderboardName, PlayerData.Instance.Score);

                if (PlayerData.Instance.Score < result.score)
                {
                    PlayerData.Instance.Score += result.score;
                    Agava.YandexGames.Leaderboard.SetScore(LeaderboardName, PlayerData.Instance.Score);
                }
            });
        }

        private void Fill()
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            _leaderboardPlayers.Clear();

            Agava.YandexGames.Leaderboard.GetEntries(LeaderboardName, result =>
            {
                int playersAmount = Mathf.Clamp(result.entries.Length, 1, _numberOfPlayerInLeaderboard);

                for (int i = 0; i < playersAmount; i++)
                {
                    var entry = result.entries[i];

                    var rank = entry.rank;
                    var score = entry.score;
                    var name = entry.player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = GetAnunymousName(PlayerData.Instance.CurrentLanguage);

                    _leaderboardPlayers.Add(new LeaderboardPlayer(rank, name, score));
                }

                _leaderboardView.ConstructLeaderboard(_leaderboardPlayers);
            });
        }

        private void OnShow()
        {
            Authorized();
        }

        private void Authorized()
        {
            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission();
                Fill();
                _leaderboardView.gameObject.SetActive(true);
            }

            if (PlayerAccount.IsAuthorized == false)
            {
                _authorizedView.gameObject.SetActive(true);
            }
        }

        private string GetAnunymousName(string language)
        {
            switch (language)
            {
                case Language.ENG:
                    return AnunymousNameENG;
                case Language.RUS:
                    return AnunymousNameRUS;
                case Language.TUR:
                    return AnunymousNameTUR;
                default:
                    return AnunymousNameENG;
            }
        }
    }
}