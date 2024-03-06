using UnityEngine;
using Agava.WebUtility;

public class LossFocus : MonoBehaviour, ITimeControllable
{
    [SerializeField] private AudioDataSO _audioData;
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
        //Debug.Log("inApp = " + inApp);
        if (inApp)
            _audioData.UnPause(this);
        else
            _audioData.Pause(this);
    }

    private void OnInBackgroundChangeWeb(bool isBackground)
    {
        PauseGame(isBackground);
        //Debug.Log("isBackground = " + isBackground);

        if (isBackground)
            _audioData.Pause(this);
        else
            _audioData.UnPause(this);
    }

    private void PauseGame(bool value)
    {

        //Debug.Log(value);
        _timeScaleSetter.SetGamePause(value, this);
    }
}