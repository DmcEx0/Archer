using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class LeaderboardAuthorizedView : MonoBehaviour
    {
        [SerializeField] private Button _exit;
        [SerializeField] private Button _authorized;

        [SerializeField] private MeinMenuView _meinMenuView;

        private void OnEnable()
        {
            _exit.onClick.AddListener(Exit);
            _authorized.onClick.AddListener(Authorized);
            _meinMenuView.EnabledUIElements(false);
        }

        private void OnDisable()
        {
            _exit.onClick.RemoveListener(Exit);
            _authorized.onClick.RemoveListener(Authorized);
            _meinMenuView.EnabledUIElements(true);
        }

        private void Exit()
        {
            gameObject.SetActive(false);
        }

        private void Authorized()
        {
            gameObject.SetActive(false);

            PlayerAccount.Authorize();
        }
    }
}