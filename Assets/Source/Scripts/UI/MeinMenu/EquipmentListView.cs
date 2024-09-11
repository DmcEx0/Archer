using System;
using System.Collections.Generic;
using Archer.Data;
using UnityEngine;

namespace Archer.UI
{
    public class EquipmentListView : MonoBehaviour
    {
        [SerializeField] private EquipmentSmallIconView _template;

        public event Action<EquipmentDataConfig> EquipmentSelected;

        public void Render(Transform container, IEnumerable<EquipmentDataConfig> equipmentsData)
        {
            foreach (var equipment in equipmentsData)
            {
                var presenter = Instantiate(_template, container);

                presenter.EquipmentSelected += OnEquipmentSelected;

                presenter.Render(equipment);
            }
        }

        private void OnEquipmentSelected(EquipmentDataConfig equipmentData)
        {
            EquipmentSelected?.Invoke(equipmentData);
        }
    }
}