using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsWindowView : MonoBehaviour, ITimeControllable
{
    [SerializeField] private TimeScaleSetter _timeScaleSetter;

    [SerializeField] private AudioDataSO _audioData;

    [Space]
    [SerializeField] private Button _closeButton;

    [Space]
    [SerializeField] private Toggle _sfxToggle;
    [SerializeField] private Toggle _musicToggle;

    public event UnityAction<bool> OnOpened;

    private void OnEnable()
    {
        _sfxToggle.isOn = _audioData.SfxIsOn;
        _musicToggle.isOn = _audioData.MusicIsOn;

        _closeButton.onClick.AddListener(Close);

        _sfxToggle.onValueChanged.AddListener(ChangeStatusSFX);
        _musicToggle.onValueChanged.AddListener(ChangeStatusMusic);

        OnOpened?.Invoke(false);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);

        _sfxToggle.onValueChanged.RemoveListener(ChangeStatusSFX);
        _musicToggle.onValueChanged.RemoveListener(ChangeStatusMusic);
    }

    public void SetTimeScale(bool isPause)
    {
        _timeScaleSetter.SetGamePause(isPause, this);
    }

    private void Close()
    {
        OnOpened?.Invoke(true);

        gameObject.SetActive(false);

        SetTimeScale(false);
    }

    private void ChangeStatusSFX(bool isSFXOn)
    {
        _audioData.SetActiveSFX(isSFXOn);
    }

    private void ChangeStatusMusic(bool isMusicOn)
    {
        _audioData.SetActiveMusic(isMusicOn);
    }
}