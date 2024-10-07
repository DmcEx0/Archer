using System;
using Archer.Audio;
using Archer.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class SettingsWindowView : MonoBehaviour, ITimeControllable
    {
        [SerializeField] private TimeScaleSetter _timeScaleSetter;

        [SerializeField] private AudioDataConfig _audioData;

        [Space] [SerializeField] private Button _closeButton;

        [Space] [SerializeField] private Toggle _sfxToggle;
        [SerializeField] private Toggle _musicToggle;

        public event Action Opening;
        public event Action Closing;

        private void OnEnable()
        {
            _sfxToggle.isOn = AudioHandler.Instance.SfxIsOn;
            _musicToggle.isOn = AudioHandler.Instance.MusicIsOn;

            _closeButton.onClick.AddListener(OnClose);

            _sfxToggle.onValueChanged.AddListener(OnChangeStatusSfx);
            _musicToggle.onValueChanged.AddListener(OnChangeStatusMusic);

            Closing?.Invoke();
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClose);

            _sfxToggle.onValueChanged.RemoveListener(OnChangeStatusSfx);
            _musicToggle.onValueChanged.RemoveListener(OnChangeStatusMusic);
        }

        public void SetTimeScale(bool isPause)
        {
            _timeScaleSetter.SetGamePause(isPause, this);
        }

        private void OnClose()
        {
            Opening?.Invoke();

            gameObject.SetActive(false);

            SetTimeScale(false);
        }

        private void OnChangeStatusSfx(bool isSfxOn)
        {
            AudioHandler.Instance.SetActiveSfx(isSfxOn);
        }

        private void OnChangeStatusMusic(bool isMusicOn)
        {
            AudioHandler.Instance.SetActiveMusic(isMusicOn);
        }
    }
}