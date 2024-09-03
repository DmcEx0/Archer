using Archer.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Archer.UI
{
    public class EquipmentSmallIconView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        private EquipmentDataSO _equipmentData;

        public event UnityAction<EquipmentDataSO> EquipmentSelected;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnEquipmentSelected);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnEquipmentSelected);
        }

        private void OnEquipmentSelected()
        {
            EquipmentSelected?.Invoke(_equipmentData);
        }

        public void Render(EquipmentDataSO equipmentData)
        {
            _equipmentData = equipmentData;

            _image.sprite = _equipmentData.Icon;
        }
    }
}