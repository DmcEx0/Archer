using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillButtonView : MonoBehaviour
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

    private float _value = 1f;
    private bool _isSkillActivated = false;

    private void OnEnable()
    {
        _button.onClick.AddListener(ActivateSkill);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ActivateSkill);
    }

    private void Start()
    {
        _deactivateColor = _mainImage.color;
        _activateColor = new Color(_mainImage.color.r, _mainImage.color.g, _mainImage.color.b, AlphaValue);

        ResetButton();
    }

    public void OnCooldownChanged()
    {
        _value = Mathf.Clamp(_value - Step, MinValue, MaxValue);
        _cooldownImage.fillAmount = _value;

        if (_cooldownImage.fillAmount == 0)
        {
            _button.enabled = true;
            _mainImage.color = _deactivateColor;
        }
    }

    public void ResetButton()
    {
        _value = 1f;
        _cooldownImage.fillAmount = _value;

        _mainImage.color = _deactivateColor;

        _button.enabled = false;
        _isSkillActivated = false;
    }

    public bool GetActivatedStatus()
    {
        if (_isSkillActivated == true)
            ResetButton();

        return _isSkillActivated;
    }

    private void ActivateSkill()
    {
        if (_isSkillActivated == true)
        {
            _isSkillActivated = false;
            _mainImage.color = _deactivateColor;
        }
        else if (_isSkillActivated == false)
        {
            _isSkillActivated = true;
            _mainImage.color = _activateColor;
        }
    }
}