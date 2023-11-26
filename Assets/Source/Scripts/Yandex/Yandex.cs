using Agava.YandexGames;
using IJunior.TypedScenes;
using System.Collections;
using UnityEngine;

public class Yandex : MonoBehaviour
{
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize(LoadPlayerData);

        Menu.Load();
    }

    private void LoadPlayerData()
    {
        PlayerData.Initialize();
    }
}