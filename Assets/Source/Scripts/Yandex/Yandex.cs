using Agava.YandexGames;
using IJunior.TypedScenes;
using System.Collections;
using Archer.Data;
using UnityEngine;

namespace Archer.Yandex
{
    public class Yandex : MonoBehaviour
    {
        private Localization _localization;

        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
            _localization = new();
        }

        private IEnumerator Start()
        {
            yield return YandexGamesSdk.Initialize(LoadPlayerData);

            _localization.ChangeLanguage();
            Menu.Load();
        }

        private void LoadPlayerData()
        {
            PlayerData.Initialize();
        }
    }
}