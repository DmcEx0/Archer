using Agava.YandexGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yandex : MonoBehaviour
{
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        if (Application.isEditor)
            yield break;

        yield return YandexGamesSdk.Initialize(LoadPlayerData);
    }

    private void LoadPlayerData()
    {
        PlayerData.Initialize();
    }
}