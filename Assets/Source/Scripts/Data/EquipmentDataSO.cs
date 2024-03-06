using UnityEngine;

public abstract class EquipmentDataSO : ScriptableObject
{
    [SerializeField] private Presenter _presenter;

    [Space]
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _price;
    [SerializeField] private bool _wasBought = false;

    [Space]
    [SerializeField] private int _id;

    public Presenter Presenter => _presenter;
    public string Name => _name;
    public Sprite Icon => _icon;
    public bool WasBought => _wasBought;
    public int ID => _id;
    public int Price => _price;

    public bool TryBuy()
    {
        if (PlayerData.Instance.Coins >= _price)
        {
            _wasBought = true;
            PlayerData.Instance.Coins -= _price;
        }

        return _wasBought;
    }
}