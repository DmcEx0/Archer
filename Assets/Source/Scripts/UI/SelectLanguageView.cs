using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class SelectLanguageView : MonoBehaviour
{
    [SerializeField] private Button _languageButtonRus;
    [SerializeField] private Button _languageButtonEng;
    [SerializeField] private Button _languageButtonTur;

    private void OnEnable()
    {
        _languageButtonEng.onClick.AddListener(SelectEngLanguage);
        _languageButtonRus.onClick.AddListener(SelectRusLanguage);
        _languageButtonTur.onClick.AddListener(SelectTurLanguage);
    }

    private void OnDisable()
    {
        _languageButtonEng.onClick.RemoveListener(SelectEngLanguage);
        _languageButtonRus.onClick.RemoveListener(SelectRusLanguage);
        _languageButtonTur.onClick.RemoveListener(SelectTurLanguage);
    }

    private void SelectRusLanguage()
    {
        SetLanguage(Language.RUS.ToString());
    }

    private void SelectEngLanguage()
    {
        SetLanguage(Language.ENG.ToString());
    }

    private void SelectTurLanguage()
    {
        SetLanguage(Language.TUR.ToString());
    }

    private void SetLanguage(string language)
    {
        PlayerData.Instance.CurrentLanguage = language;
        LeanLocalization.SetCurrentLanguageAll(language);
    }
}
