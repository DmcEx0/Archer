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

        public event Action<bool> Opening;

        private void OnEnable()
        {
            _sfxToggle.isOn = _audioData.SfxIsOn;
            _musicToggle.isOn = _audioData.MusicIsOn;

            _closeButton.onClick.AddListener(OnClose);

            _sfxToggle.onValueChanged.AddListener(OnChangeStatusSfx);
            _musicToggle.onValueChanged.AddListener(OnChangeStatusMusic);

            Opening?.Invoke(false);
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
            Opening?.Invoke(true);

            gameObject.SetActive(false);

            SetTimeScale(false);
        }

        private void OnChangeStatusSfx(bool isSfxOn)
        {
            _audioData.SetActiveSfx(isSfxOn);
        }

        private void OnChangeStatusMusic(bool isMusicOn)
        {
            _audioData.SetActiveMusic(isMusicOn);
        }
    }
}