using Archer.Model;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AnimationController))]
public class CharacterPresenter : Presenter, IGeneratable
{
    [SerializeField] private Transform _weaponPosition;
    [SerializeField] private ParticleSystem _fireParticle;
    [SerializeField] private ParticleSystem _poisonParticle;

    private ParticleSystem _currentParticle;

    private AnimationController _animationController;

    private HitBodyDetector _hitBodyDetector;
    private HitHeadDetector _hitHeadDetector;

    private float _playingEffectTime = 0;
    private float _accumulatedTime;

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

    private void Update()
    {
        if (_playingEffectTime == 0)
            return;

        _accumulatedTime += Time.deltaTime;

        if (_accumulatedTime > _playingEffectTime)
        {
            OnDeactivateSkillImpact();
            _accumulatedTime = 0;
            _playingEffectTime = 0;
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _hitBodyDetector.Hit += OnHitInBody;
        _hitHeadDetector.Hit += OnHitInHead;
        _model.SkillImpacted += OnActivateSkillImpact;
    }

    protected override void OnDestroying()
    {
        base.OnDestroying();

        _hitBodyDetector.Hit -= OnHitInBody;
        _hitHeadDetector.Hit -= OnHitInHead;
        _model.SkillImpacted -= OnActivateSkillImpact;
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

    private void OnActivateSkillImpact(float playingEffectTime, ArrowSkillType skillType)
    {
        _playingEffectTime = playingEffectTime;

        switch (skillType)
        {
            case ArrowSkillType.Default:
                return;

            case ArrowSkillType.Fire:
                PlayEffect(_fireParticle);
                break;

            case ArrowSkillType.Poison:
                PlayEffect(_poisonParticle);
                break;
        }
    }

    private void PlayEffect(ParticleSystem particleSystem)
    {
        _currentParticle = particleSystem;
        _currentParticle.Play();
    }

    private void OnDeactivateSkillImpact()
    {
        _currentParticle.Stop();
    }
}