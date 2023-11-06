using Archer.Model;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AnimationController))]
public class CharacterPresenter : Presenter, IGeneratable
{
    [SerializeField] private Transform _weaponPosition;

    private AnimationController _animationController;

    private HitBodyDetector _hitBodyDetector;
    private HitHeadDetector _hitHeadDetector;

    public event UnityAction HitInHead;

    private Character _model => Model is Character ? Model as Character : null;

    public Transform GeneratingPoint => _weaponPosition;
    public AnimationController AnimationController => _animationController;

    private void Awake()
    {
        _animationController = GetComponent<AnimationController>();
        _hitBodyDetector = GetComponentInChildren<HitBodyDetector>();
        _hitHeadDetector = GetComponentInChildren<HitHeadDetector>();
    }

    private void OnEnable()
    {
        _hitBodyDetector.Hit += OnHitInBody;
        _hitHeadDetector.Hit += OnHitInHead;
    }

    private void OnDisable()
    {
        _hitBodyDetector.Hit -= OnHitInBody;
        _hitHeadDetector.Hit -= OnHitInHead;
    }

    private void OnHitInBody(int mainDamage, int additionalDamage, ArrowSkillType skillType)
    {
        _animationController.PlayeHitA();

        _model.TakeDamage(mainDamage, additionalDamage, skillType);
    }

    private void OnHitInHead(int damage, int additionalDamage, ArrowSkillType skillType)
    {
        HitInHead?.Invoke();
        _animationController.PlayeHitB();

        _model.TakeDamage(damage, additionalDamage, skillType);
    }
}