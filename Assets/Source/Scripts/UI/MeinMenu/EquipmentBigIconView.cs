using System;
using Archer.Data;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class EquipmentBigIconView : MonoBehaviour
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _closeButton;

        [Space] [SerializeField] private Image _image;
        [SerializeField] private Image _lockImage;

        [Space] [SerializeField] private TMP_Text _equipmentPrice;

        [Space] [SerializeField] private LeanLocalizedTextMeshProUGUI _tmpLocalization;

        private EquipmentDataConfig _equipmentData;

        public event Action<EquipmentDataConfig> EquipmentSelected;
        public event Action Opening;
        public event Action Closing;

        private void OnEnable()
        {
            _equipButton.onClick.AddListener(OnEquipmentSelected);
            _buyButton.onClick.AddListener(OnTryBuy);

            _closeButton.onClick.AddListener(OnClose);
            Closing?.Invoke();
        }

        private void OnDisable()
        {
            _equipButton.onClick.RemoveListener(OnEquipmentSelected);
            _buyButton.onClick.RemoveListener(OnTryBuy);

            _closeButton.onClick.RemoveListener(OnClose);
        }

        public void Render(EquipmentDataConfig equipmentData)
        {
            _equipmentData = equipmentData;

            EnableRequiredButton();

            _tmpLocalization.TranslationName = LeanLocalization.CurrentTranslations[_equipmentData.Name].Name;
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

        private void OnClose()
        {
            gameObject.SetActive(false);
            Opening?.Invoke();
        }

        private void OnEquipmentSelected()
        {
            EquipmentSelected?.Invoke(_equipmentData);

            OnClose();
        }

        private void OnTryBuy()
        {
            if (_equipmentData.WasBought == false)
            {
                if (_equipmentData.TryBuy())
                    OnClose();
            }
        }
    }
}