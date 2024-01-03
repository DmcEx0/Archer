using UnityEngine;
using Agava.WebUtility;

public class LossFocus : MonoBehaviour
{
    [SerializeField] private AudioDataSO _audioData;

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
    }

    private void OnInBackgroundChangeWeb(bool isBackground)
    {
        _audioData.SetActiveSFX(isBackground);
        _audioData.SetActiveMusic(isBackground);
        PauseGame(isBackground);
    }

    private void PauseGame(bool value)
    {
        Time.timeScale = value ? 0 : 1;
    }
}