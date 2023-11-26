using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsWindowView : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _languageButton;
    [SerializeField] private Toggle _sfxToggle;
    [SerializeField] private Toggle _musicToggle;

    public event UnityAction<bool> SFXChanged;
    public event UnityAction<bool> MusicChanged;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
        _sfxToggle.onValueChanged.AddListener(ChangeStatusSFX);
        _musicToggle.onValueChanged.AddListener(ChangeStatusMusic);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
        _sfxToggle.onValueChanged.RemoveListener(ChangeStatusSFX);
        _musicToggle.onValueChanged.RemoveListener(ChangeStatusMusic);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void ChangeStatusSFX(bool isSFXOn)
    {
        SFXChanged?.Invoke(isSFXOn);
    }

    private void ChangeStatusMusic(bool isMusicOn)
    {
        MusicChanged?.Invoke(isMusicOn);
    }
}