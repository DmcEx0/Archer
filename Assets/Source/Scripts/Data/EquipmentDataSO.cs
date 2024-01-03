using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentDataSO : ScriptableObject
{
    [SerializeField] private Presenter _presenter;

    [Space]
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _price;
    [SerializeField] private bool _wasBought = false;

    public Presenter Presenter => _presenter;
    public string Name => _name;
    public Sprite Icon => _icon;
    public bool WasBought => _wasBought;

    public bool TryBuy()
    {
        if (PlayerData.Instance.Coins >= _price)
        {
            _wasBought = true;
            PlayerData.Instance.Coins -= _price;

            return _wasBought;
        }

        return _wasBought;
    }
}