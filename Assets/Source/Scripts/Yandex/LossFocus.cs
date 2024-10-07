using UnityEngine;
using Agava.WebUtility;
using Archer.Audio;
using Archer.Utils;

namespace Archer.Yandex
{
    public class LossFocus : MonoBehaviour, ITimeControllable
    {
        [SerializeField] private AudioDataConfig _audioData;
        [SerializeField] private TimeScaleSetter _timeScaleSetter;

        private void OnEnable()
        {
            Application.focusChanged += OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
        }

        private void OnDisable()
        {
            Application.focusChanged -= OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
        }

        private void OnInBackgroundChangeApp(bool inApp)
        {
            PauseGame(!inApp);
            
            if (inApp)
                AudioHandler.Instance.UnPause(this); 
            else
                AudioHandler.Instance.Pause(this);
        }

        private void OnInBackgroundChangeWeb(bool isBackground)
        {
            PauseGame(isBackground);

            if (isBackground)
                AudioHandler.Instance.Pause(this);
            else
                AudioHandler.Instance.UnPause(this);
        }

        private void PauseGame(bool value)
        {
            _timeScaleSetter.SetGamePause(value, this);
        }
    }
}