using Agava.YandexGames;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Scripts.UI.Liderboard
{
    public class Leaderboard : MonoBehaviour
    {
        private const string AnunymousName = "Anonymous";
        private const string LeaderboardName = "ArcherLeaderboard";

        [SerializeField] private LeaderboardView _leaderboardView;
        [SerializeField] private Button _showLeaderboard;
        //[SerializeField] private LeaderboardElement _playerRanking;

        private List<LeaderboardPlayer> _leaderboardPlayers = new();
        private readonly int _numberOfPlayerInLeaderboard = 4;

        private void OnEnable()
        {
            _showLeaderboard.onClick.AddListener(Show);
        }

        private void OnDisable()
        {
            _showLeaderboard.onClick.RemoveListener(Show);
        }

        public void SetPlayer()
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            Agava.YandexGames.Leaderboard.GetPlayerEntry(LeaderboardName, _ =>
            {
                Agava.YandexGames.Leaderboard.SetScore(LeaderboardName, PlayerData.Instance.Score);
            });
        }

        private void Fill()
        {
            _leaderboardPlayers.Clear();

            if (PlayerAccount.IsAuthorized == false)
                return;

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
                        name = AnunymousName;

                    _leaderboardPlayers.Add(new LeaderboardPlayer(rank, name, score));
                }

                _leaderboardView.ConstructLeaderboard(_leaderboardPlayers);
            });
        }

        private void Show()
        {
            _leaderboardView.gameObject.SetActive(true);

            Authorized();
        }

        private void Authorized()
        {
            PlayerAccount.Authorize(
                onSuccessCallback: () =>
                {
                    PlayerAccount.RequestPersonalProfileDataPermission();
                    SetPlayer();
                    Fill();
                },
                onErrorCallback: (error) =>
                {
                    _leaderboardView.gameObject.SetActive(false);
                });
        }
    }
}