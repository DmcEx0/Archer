using System;
using Archer.Animations;
using Archer.Model;
using Archer.TargetComponent;
using UnityEngine;

namespace Archer.Presenters
{
    [RequireComponent(typeof(AnimationHandler))]
public class CharacterPresenter : Presenter, IGeneratable
{
    [SerializeField] private Transform _weaponPosition;
    [SerializeField] private ParticleSystem _fireParticle;
    [SerializeField] private ParticleSystem _poisonParticle;

    private ParticleSystem _currentParticle;

    private AnimationHandler _animationHandler;

    private HitBodyDetector _hitBodyDetector;
    private HitHeadDetector _hitHeadDetector;

    private float _playingEffectTime = 0;
    private float _accumulatedTime;

    public event Action GettingHitInHead;

    private Character CharacterModel => Model is Character ? Model as Character : null;
    public Transform GeneratingPoint => _weaponPosition;
    public AnimationHandler AnimationHandler => _animationHandler;

    private void Awake()
    {
        _animationHandler = GetComponent<AnimationHandler>();
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

        _hitBodyDetector.GettingHit += OnGettingHitInBody;
        _hitHeadDetector.GettingHit += OnGettingHitInHead;
        CharacterModel.SkillImpacted += OnActivateSkillImpact;
    }

    protected override void OnDestroying()
    {
        base.OnDestroying();

        _hitBodyDetector.GettingHit -= OnGettingHitInBody;
        _hitHeadDetector.GettingHit -= OnGettingHitInHead;
        CharacterModel.SkillImpacted -= OnActivateSkillImpact;
    }

    private void OnGettingHitInBody(int mainDamage, int additionalDamage, ArrowSkillType skillType)
    {
        _animationHandler.PlayHitA();

        CharacterModel.TakeDamage(mainDamage, additionalDamage, skillType);
    }

    private void OnGettingHitInHead(int damage, int additionalDamage, ArrowSkillType skillType)
    {
        GettingHitInHead?.Invoke();
        _animationHandler.PlayHitB();

        CharacterModel.TakeDamage(damage, additionalDamage, skillType);
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

        if (_currentParticle != null)
            _currentParticle.Play();
    }

    private void OnDeactivateSkillImpact()
    {
        _currentParticle.Stop();
    }
}
}