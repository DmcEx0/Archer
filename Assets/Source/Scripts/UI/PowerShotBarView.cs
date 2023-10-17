using Archer.Model;
using UnityEngine;
using UnityEngine.UI;

public class PowerShotBarView : MonoBehaviour
{
    [SerializeField] private Image _powerShotSlider;

    private PlayerInputRouter _playerInputRouter;
    private Camera _camera;

    private float _value;

    private void OnDisable()
    {
        _playerInputRouter.PowerChanged -= OnPowerChanged;
    }

    private void Awake()
    {
        _camera = Camera.main;
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
