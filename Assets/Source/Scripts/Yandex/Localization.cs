using Agava.YandexGames;
using Lean.Localization;
using Unity.VisualScripting;
using UnityEngine;

public class Localization : MonoBehaviour
{
    private const string EnglishCode = "en";
    private const string RussianCode = "ru";
    private const string TurkishCode = "tr";

    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ChangeLanguage();
#endif
    }

    private void ChangeLanguage()
    {
        string languageCode = YandexGamesSdk.Environment.i18n.lang;

        string language = languageCode switch
        {
            EnglishCode => Language.ENG.ToString(),
            RussianCode => Language.RUS.ToString(),
            TurkishCode => Language.TUR.ToString(),
            _ => Language.ENG.ToString()
        };

        LeanLocalization.SetCurrentLanguageAll(language);
    }
}