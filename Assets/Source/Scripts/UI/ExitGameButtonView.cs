using Archer.Utils;
using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class ExitGameButtonView : MonoBehaviour
    {
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Root _root;

        [SerializeField] private TimeScaleSetter _timeScaleSetter;

        private SettingsWindowView _settingWindowView;

        private void OnEnable()
        {
            _exitGameButton.onClick.AddListener(LoadMeinMenuScene);
        }

        private void OnDisable()
        {
            _exitGameButton.onClick.RemoveListener(LoadMeinMenuScene);
        }

        private void Awake()
        {
            _settingWindowView = GetComponent<SettingsWindowView>();
        }

        private void LoadMeinMenuScene()
        {
            _root.OnExitGame();
            _settingWindowView.SetTimeScale(false);

            Menu.Load();
        }
    }
}