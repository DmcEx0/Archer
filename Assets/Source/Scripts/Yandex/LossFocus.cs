using UnityEngine;
using Agava.WebUtility;
using static Agava.YandexGames.YandexGamesEnvironment;

public class LossFocus : MonoBehaviour
{
    [SerializeField] private AudioDataSO _audioData;
    [SerializeField] private SettingsWindowView _settingsWindowView;

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
        PauseGame(inApp);

        if (inApp == false)
            _audioData.Pause();
        else
            _audioData.UpPause();
    }

    private void OnInBackgroundChangeWeb(bool isBackground)
    {
        PauseGame(isBackground);

        if (isBackground == false)
            _audioData.Pause();
        else
            _audioData.UpPause();
    }

    private void PauseGame(bool value)
    {
        Time.timeScale = value ? 1 : 0;
    }
}