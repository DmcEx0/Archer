using Lean.Localization;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsWindowView : MonoBehaviour
{
    [SerializeField] private AudioDataSO _audioData;

    [Space]
    [SerializeField] private Button _closeButton;

    [Space]
    [SerializeField] private Button _languageButtonRus;
    [SerializeField] private Button _languageButtonEng;
    [SerializeField] private Button _languageButtonTur;

    [Space]
    [SerializeField] private Toggle _sfxToggle;
    [SerializeField] private Toggle _musicToggle;

    public bool MusicToggleIsOn => _musicToggle.isOn;

    private void OnEnable()
    {
        _sfxToggle.isOn = _audioData.SfxIsOn;
        _musicToggle.isOn = _audioData.MusicIsOn;

        _closeButton.onClick.AddListener(Close);

        _sfxToggle.onValueChanged.AddListener(ChangeStatusSFX);
        _musicToggle.onValueChanged.AddListener(ChangeStatusMusic);

        _languageButtonEng.onClick.AddListener(SelectEngLanguage);
        _languageButtonRus.onClick.AddListener(SelectRusLanguage);
        _languageButtonTur.onClick.AddListener(SelectTurLanguage);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);

        _sfxToggle.onValueChanged.RemoveListener(ChangeStatusSFX);
        _musicToggle.onValueChanged.RemoveListener(ChangeStatusMusic);

        _languageButtonEng.onClick.RemoveListener(SelectEngLanguage);
        _languageButtonRus.onClick.RemoveListener(SelectRusLanguage);
        _languageButtonTur.onClick.RemoveListener(SelectTurLanguage);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void ChangeStatusSFX(bool isSFXOn)
    {
        _audioData.SetActiveSFX(isSFXOn);
    }

    private void ChangeStatusMusic(bool isMusicOn)
    {
        _audioData.SetActiveMusic(isMusicOn);
    }

    private void SelectRusLanguage()
    {
        LeanLocalization.SetCurrentLanguageAll(Language.RUS.ToString());
    }

    private void SelectEngLanguage()
    {
        LeanLocalization.SetCurrentLanguageAll(Language.ENG.ToString());
    }

    private void SelectTurLanguage()
    {
        LeanLocalization.SetCurrentLanguageAll(Language.TUR.ToString());
    }
}