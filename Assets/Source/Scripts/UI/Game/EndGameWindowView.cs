using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameWindowView : MonoBehaviour
{
    [SerializeField] private Button _meinMenuButton;

    private void OnEnable()
    {
        _meinMenuButton.onClick.AddListener(() => { SceneManager.LoadScene("Menu");});
    }

    private void OnDisable()
    {
        _meinMenuButton.onClick.RemoveListener(() => { SceneManager.LoadScene("Menu");});
    }
}
