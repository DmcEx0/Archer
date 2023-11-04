using Archer.Model;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Slider _healthBarSlider;

    private Health _health;
    private Camera _camera;

    private float _value;

    private void Awake()
    {
        _camera = Camera.main;
        _value = _healthBarSlider.value;

        //transform.LookAt(new Vector3(transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
    }

    private void Update()
    {
        _healthBarSlider.value = Mathf.Lerp(_healthBarSlider.value, _value, 1.5f * Time.deltaTime);
    }

    private void OnHealthChanged(int currentValue, int maxValue)
    {
        _value = (float)currentValue / (float)maxValue;

        if (_value <= 0)
        {
            _health.ValueChanged -= OnHealthChanged;
        }
    }

    public void Init(Health health)
    {
        _health = health;
        _health.ValueChanged += OnHealthChanged;
    }
}