using Lean.Localization;
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
    [SerializeField] private LeanLocalizedTextMeshProUGUI _TMPLocalization;

    private EquipmentDataSO _equipmentData;

    public event UnityAction<EquipmentDataSO> EquipmentSelected;
    public event UnityAction<bool> WindowClose;

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

    public void Render(EquipmentDataSO equipmentData)
    {
        _equipmentData = equipmentData;

        SetEquipmentName(_equipmentData.Name);
        _image.sprite = _equipmentData.Icon;

        _lockImage.gameObject.SetActive(true);

        if (_equipmentData.WasBought)
        {
            _lockImage.gameObject.SetActive(false);
        }
    }

    private void CloseWindow()
    {
        gameObject.SetActive(false);
        WindowClose?.Invoke(true);
    }

    private void OnEquipmentSelected()
    {
        EquipmentSelected?.Invoke(_equipmentData);

        if (_equipmentData.WasBought)
            CloseWindow();

        if (_equipmentData.WasBought == false)
        {
            if (_equipmentData.TryBuy())
                CloseWindow();
        }
    }

    private void SetEquipmentName(string equipName)
    {
        switch (equipName)
        {
            case "Arrow1":
                _TMPLocalization.TranslationName = LeanLocalization.CurrentTranslations[equipName].Name;
                break;

            case "Arrow2":
                _TMPLocalization.TranslationName = LeanLocalization.CurrentTranslations[equipName].Name;
                break;

            case "Arrow3":
                _TMPLocalization.TranslationName = LeanLocalization.CurrentTranslations[equipName].Name;
                break;

            case "Crossbow1":
                _TMPLocalization.TranslationName = LeanLocalization.CurrentTranslations[equipName].Name;
                break;

            case "Crossbow2":
                _TMPLocalization.TranslationName = LeanLocalization.CurrentTranslations[equipName].Name;
                break;
        }
    }
}
