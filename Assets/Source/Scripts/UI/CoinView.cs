using TMPro;
using UnityEngine;

public class CoinView : MonoBehaviour
{
    [SerializeField] private TMP_Text _valueText;

    private int _amount;

    private void OnEnable()
    {
        PlayerData.Instance.CoinChanged += OnCoinsChanged;
    }

    private void OnDisable()
    {
        PlayerData.Instance.CoinChanged -= OnCoinsChanged;
    }

    private void Start()
    {
        _amount = PlayerData.Instance.Coins;
        _valueText.text = _amount.ToString();
    }

    private void OnCoinsChanged(int amount)
    {
        _valueText.text = amount.ToString();
    }
}