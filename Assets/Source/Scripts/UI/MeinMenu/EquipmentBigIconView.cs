using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentBigIconView : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _closeButton;

    [Space]
    [SerializeField] private Image _image;
    [SerializeField] private Image _lockImage;

    [Space]
    [SerializeField] private TMP_Text _equipmentPrice;

    [Space]
    [SerializeField] private LeanLocalizedTextMeshProUGUI _TMPLocalization;

    private EquipmentDataSO _equipmentData;

    public event UnityAction<EquipmentDataSO> EquipmentSelected;
    public event UnityAction<bool> OnOpened;

    private void OnEnable()
    {
        _equipButton.onClick.AddListener(OnEquipmentSelected);
        _buyButton.onClick.AddListener(TryBuy);

        _closeButton.onClick.AddListener(Close);
        OnOpened?.Invoke(false);
    }

    private void OnDisable()
    {
        _equipButton.onClick.RemoveListener(OnEquipmentSelected);
        _buyButton.onClick.RemoveListener(TryBuy);

        _closeButton.onClick.RemoveListener(Close);
    }

    public void Render(EquipmentDataSO equipmentData)
    {
        _equipmentData = equipmentData;

        EnableRequiredButton();

        _TMPLocalization.TranslationName = LeanLocalization.CurrentTranslations[_equipmentData.Name].Name;
        _image.sprite = _equipmentData.Icon;

        _lockImage.gameObject.SetActive(true);

        if (_equipmentData.WasBought)
        {
            _lockImage.gameObject.SetActive(false);
        }
    }

    private void EnableRequiredButton()
    {
        if (_equipmentData.WasBought)
        {
            _buyButton.gameObject.SetActive(false);
            _equipButton.gameObject.SetActive(true);
        }
        else
        {
            _buyButton.gameObject.SetActive(true);
            _equipButton.gameObject.SetActive(false);

            _equipmentPrice.text = _equipmentData.Price.ToString();
        }
    }

    private void Close()
    {
        gameObject.SetActive(false);
        OnOpened?.Invoke(true);
    }

    private void OnEquipmentSelected()
    {
        EquipmentSelected?.Invoke(_equipmentData);

        Close();
    }

    private void TryBuy()
    {
        if (_equipmentData.WasBought == false)
        {
            if (_equipmentData.TryBuy())
                Close();
        }
    }
}
