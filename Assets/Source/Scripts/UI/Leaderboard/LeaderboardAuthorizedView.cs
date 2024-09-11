using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class LeaderboardAuthorizedView : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _authorizeButton;

        [SerializeField] private MainMenuView _mainMenuView;

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(CloseWindow);
            _authorizeButton.onClick.AddListener(Authorize);
            _mainMenuView.OnEnabledUIElements(false);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(CloseWindow);
            _authorizeButton.onClick.RemoveListener(Authorize);
            _mainMenuView.OnEnabledUIElements(true);
        }

        private void CloseWindow()
        {
            gameObject.SetActive(false);
        }

        private void Authorize()
        {
            gameObject.SetActive(false);

            PlayerAccount.Authorize();
        }
    }
}