using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameWindowView : MonoBehaviour
{
    [SerializeField] private Button _meinMenuButton;
    [SerializeField] private TMP_Text _goldText;

    private void OnEnable()
    {
        _meinMenuButton.onClick.AddListener(() => { SceneManager.LoadScene("Menu"); });
    }

    public void SetAmountCoins(int amountCoins)
    {
        _goldText.text = $"+{amountCoins}";
    }

    private void OnDisable()
    {
        _meinMenuButton.onClick.RemoveListener(() => { SceneManager.LoadScene("Menu"); });
    }
}
