using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Transform _container;
        [SerializeField] private LeaderboardElement _leaderboardElementPrefab;

        [SerializeField] private MainMenuView _mainMenuView;

        private List<LeaderboardElement> _spawnedElements = new();

        public event Action Opening;
        public event Action Closing;

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(Close);
            _mainMenuView.OnDisableUiElements();

            Closing?.Invoke();
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(Close);
            _mainMenuView.OnEnabledUiElements();

            Opening?.Invoke();
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