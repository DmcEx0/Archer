using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Archer.UI
{
    public class SkillButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float Step = 0.34f;
        private const float MinValue = 0f;
        private const float MaxValue = 1f;
        private const float AlphaValue = 0.6f;

        [SerializeField] private Button _button;
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private Image _mainImage;

        private Color _activateColor;
        private Color _deactivateColor;

        private float _defaultFillAmount = 1f;
        private bool _isSkillActivated = false;

        public event UnityAction<bool> OnUIPressed;

        private void Start()
        {
            _deactivateColor = _mainImage.color;
            _activateColor = new Color(_mainImage.color.r, _mainImage.color.g, _mainImage.color.b, AlphaValue);

            ResetButton();
        }

        public void OnCooldownChanged()
        {
            _defaultFillAmount = Mathf.Clamp(_defaultFillAmount - Step, MinValue, MaxValue);
            _cooldownImage.fillAmount = _defaultFillAmount;

            if (_cooldownImage.fillAmount == 0)
            {
                _button.enabled = true;
                _mainImage.color = _deactivateColor;
            }
        }

        public void ResetButton()
        {
            _defaultFillAmount = 1f;
            _cooldownImage.fillAmount = _defaultFillAmount;

            _mainImage.color = _deactivateColor;

            _button.enabled = false;
            _isSkillActivated = false;
        }

        public bool GetActivatedStatus()
        {
            return _isSkillActivated;
        }

        private void ActivateSkill()
        {
            if (_isSkillActivated == true)
            {
                _isSkillActivated = false;
                _mainImage.color = _deactivateColor;
            }

            if (_isSkillActivated == false && _cooldownImage.fillAmount == 0)
            {
                _isSkillActivated = true;
                _mainImage.color = _activateColor;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnUIPressed?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUIPressed?.Invoke(false);
            ActivateSkill();
        }
    }
}