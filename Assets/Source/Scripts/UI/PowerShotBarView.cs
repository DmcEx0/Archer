using Archer.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class PowerShotBarView : MonoBehaviour
    {
        [SerializeField] private Image _powerShotSlider;

        private PlayerInputRouter _playerInputRouter;

        private void OnDisable()
        {
            if (_playerInputRouter != null)
                _playerInputRouter.PowerChanged -= OnPowerChanged;
        }

        private void Awake()
        {
            _powerShotSlider.fillAmount = 0;
        }

        private void OnPowerChanged(float value, float maxValue)
        {
            _powerShotSlider.fillAmount = value / maxValue;
        }

        public void Init(PlayerInputRouter playerInputRouter)
        {
            _playerInputRouter = playerInputRouter;
            _playerInputRouter.PowerChanged += OnPowerChanged;
        }
    }
}