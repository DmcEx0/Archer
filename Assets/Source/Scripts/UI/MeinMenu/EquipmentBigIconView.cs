using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentBigIconView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private Image _image;
    [SerializeField] private Image _lockImage;

    private EquipmentDataSO _equipmentData;

    public event UnityAction<EquipmentDataSO> EquipmentSelected;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnEquipmentSelected);
        _closeButton.onClick.AddListener(CloseWindow);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnEquipmentSelected);
        _closeButton.onClick.RemoveListener(CloseWindow);
    }

    private void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    private void OnEquipmentSelected()
    {
        EquipmentSelected?.Invoke(_equipmentData);

        if(_equipmentData.WasBought)
            CloseWindow();

        if (_equipmentData.WasBought == false)
        {
            if(_equipmentData.TryBuy())
                CloseWindow();
        }
    }

    public void Render(EquipmentDataSO equipmentData)
    {
        _equipmentData = equipmentData;

        _tmpText.text = _equipmentData.Name;
        _image.sprite = _equipmentData.Icon;

        _lockImage.gameObject.SetActive(true);

        if (_equipmentData.WasBought)
        {
            _lockImage.gameObject.SetActive(false);
        }
    }
}
