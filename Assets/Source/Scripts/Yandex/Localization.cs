using Agava.YandexGames;

public class Localization
{
    private const string EnglishCode = "en";
    private const string RussianCode = "ru";
    private const string TurkishCode = "tr";

    public void ChangeLanguage()
    {
        PlayerData.Instance.CurrentLanguage = IdentifyLanguage();
    }

    private string IdentifyLanguage()
    {
        string languageCode = YandexGamesSdk.Environment.i18n.lang;

        string language = languageCode switch
        {
            EnglishCode => Language.ENG.ToString(),
            RussianCode => Language.RUS.ToString(),
            TurkishCode => Language.TUR.ToString(),
            _ => Language.ENG.ToString()
        };

        return language;
    }
}