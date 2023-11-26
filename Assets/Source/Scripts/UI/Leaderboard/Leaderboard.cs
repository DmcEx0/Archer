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
            List<LeaderboardPlayer> leaderboardPlayers = new();

            Agava.YandexGames.Leaderboard.GetEntries(LeaderboardName, result =>
            {
                for (int i = 0; i < result.entries.Length; i++)
                {
                    var entry = result.entries[i];

                    var rank = entry.rank;
                    var score = entry.score;
                    var name = entry.player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = AnunymousName;

                    leaderboardPlayers.Add(new LeaderboardPlayer(rank, name, score));
                }

                _leaderboardView.ConstructLeaderboard(leaderboardPlayers);
            });
        }

        private void Show()
        {
            _leaderboardView.gameObject.SetActive(true);

            Authorize();
        }

        private void Authorize()
        {
            //PlayerAccount.Authorize(
            //    onSuccessCallback: () =>
            //    {
            //        PlayerAccount.RequestPersonalProfileDataPermission();
            //        SetPlayer();
            //        Fill();
            //    },
            //    onErrorCallback: (error) =>
            //    {
            //        _leaderboardView.gameObject.SetActive(false);
            //    });

            PlayerAccount.Authorize();

            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission();

                SetPlayer();
                Fill();
            }
        }
    }
}