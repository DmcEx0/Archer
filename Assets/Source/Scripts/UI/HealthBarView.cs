using Archer.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider _healthBarSlider;
        [SerializeField] private TMP_Text _healthValueText;

        private Health _health;
        private Camera _camera;

        private float _healthBarValue;
        private int _maxHealthValue;

        private void Awake()
        {
            _camera = Camera.main;
            _healthBarValue = _healthBarSlider.value;

            transform.LookAt(new Vector3(transform.position.x, _camera.transform.position.y,
                _camera.transform.position.z));
        }

        private void Update()
        {
            _healthBarSlider.value = Mathf.Lerp(_healthBarSlider.value, _healthBarValue, 3f * Time.deltaTime);
        }

        public void Init(Health health)
        {
            _health = health;
            SetMaxHealthValue(health.Value);
            _health.ValueChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int currentValue, int maxValue)
        {
            _healthBarValue = (float)currentValue / (float)maxValue;
            SetCurrentHealthValue(currentValue);

            if (_healthBarValue <= 0)
            {
                _health.ValueChanged -= OnHealthChanged;
            }
        }

        private void SetMaxHealthValue(int healthValue)
        {
            _maxHealthValue = healthValue;
            string output = string.Format("{0}/{1}", healthValue, healthValue);
            _healthValueText.text = output;
        }

        private void SetCurrentHealthValue(int healthValue)
        {
            string output = string.Format("{0}/{1}", healthValue, _maxHealthValue);
            _healthValueText.text = output;
        }
    }
}