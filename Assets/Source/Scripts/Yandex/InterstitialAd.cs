using Archer.Audio;
using Archer.Utils;
using UnityEngine;

namespace Archer.Yandex
{
    public class InterstitialAd : MonoBehaviour, ITimeControllable
    {
        [SerializeField] private AudioDataConfig _audioData;
        [SerializeField] private TimeScaleSetter _timeScaleSetter;

        public void Show()
        {
            Agava.YandexGames.InterstitialAd.Show(OnOpenCallback, OnCloseCallback);
        }

        private void OnOpenCallback()
        {
            AudioHandler.Instance.Pause(this);
            _timeScaleSetter.SetGamePause(true, this);
        }

        private void OnCloseCallback(bool _)
        {
            AudioHandler.Instance.UnPause(this);
            _timeScaleSetter.SetGamePause(false, this);
        }
    }
}