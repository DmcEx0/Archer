using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentListView : MonoBehaviour
{
    [SerializeField] private EquipmentSmallIconView _template;

    public event UnityAction<EquipmentDataSO> EquipmentSelected;

    public void Render(Transform container ,IEnumerable<EquipmentDataSO> equipmentsData)
    {
        foreach (var equipment in equipmentsData)
        {
            var presenter = Instantiate(_template, container);

            presenter.EquipmentSelected += OnEquipmentSelected;

            presenter.Render(equipment);
        }
    }

    private void OnEquipmentSelected(EquipmentDataSO equipmentData)
    {
        EquipmentSelected?.Invoke(equipmentData);
    }
}
