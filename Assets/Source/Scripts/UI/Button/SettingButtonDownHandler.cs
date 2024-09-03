using UnityEngine;
using UnityEngine.EventSystems;

namespace Archer.UI
{
    public class SettingButtonDownHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private SettingsWindowView _settingsWindowView;
        public void OnPointerDown(PointerEventData eventData)
        {
            _settingsWindowView.SetTimeScale(true);
            _settingsWindowView.gameObject.SetActive(true);
        }
    }
}