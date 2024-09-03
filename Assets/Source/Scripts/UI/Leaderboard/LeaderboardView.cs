using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Archer.UI
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Transform _container;
        [SerializeField] private LeaderboardElement _leaderboardElementPrefab;

        [SerializeField] private MeinMenuView _meinMenuView;

        private List<LeaderboardElement> _spawnedElements = new();

        public event UnityAction<bool> OnOpened;

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(Close);
            _meinMenuView.EnabledUIElements(false);

            OnOpened?.Invoke(false);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(Close);
            _meinMenuView.EnabledUIElements(true);

            OnOpened?.Invoke(true);
        }

        public void ConstructLeaderboard(List<LeaderboardPlayer> leaderboardPlayers)
        {
            ClearLeaderboard();

            foreach (var player in leaderboardPlayers)
            {
                LeaderboardElement leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _container);

                leaderboardElementInstance.Initialize(player.Name, player.Score, player.Rank);

                _spawnedElements.Add(leaderboardElementInstance);
            }
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void ClearLeaderboard()
        {
            foreach (var element in _spawnedElements)
            {
                Destroy(element.gameObject);
            }

            _spawnedElements = new();
        }
    }
}