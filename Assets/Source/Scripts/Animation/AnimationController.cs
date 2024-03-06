using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator), typeof(RigBuilder))]
public class AnimationController : MonoBehaviour
{
    private const float ShootSpeed = 1f;
    private const float AimSpeed = 0.7f;
    private const float HitSpeed = 1f;
    private const float DeathSpeed = 1f;
    private const float SitInleSpeed = 1f;

    [SerializeField] private AnimationClipData _animationClipData;

    [Space]
    [SerializeField] private AnimationCurve _takenPositionCurve;
    [SerializeField] private float _takenPositionOffsetY;
    [SerializeField] private float _takenPositionTime;

    [Space]
    [SerializeField] private AnimationCurve _discardCurve;
    [SerializeField] private float _discardOffsetY;
    [SerializeField] private float _discardTime;

    [Space]
    [SerializeField] private Transform _leftHandRigTarget;
    [SerializeField] private Transform _rightHandRigTarget;
    [SerializeField] private Transform _chestRigTarget;

    private Animator _animator;

    private string _currentAnimationName;
    private string _nextAnimationName;

    private float _discardDuration;
    private float _takenPositionDuration;
    private float _reloadSpeed;

    private bool _isFinalAnimation = false;
    private bool _isTakenPosition = false;
    private bool _isDiskard = false;

    public bool IsTakenPosition => _isTakenPosition;
    public bool IsDiskard => _isDiskard;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isFinalAnimation || _currentAnimationName == null)
            return;

        AnimatorStateInfo currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);


        if (currentStateInfo.IsName(_currentAnimationName) && currentStateInfo.normalizedTime >= 0.95f)
        {
            PlayNext(_nextAnimationName);
        }
    }

    public Vector3 Diskard(Vector3 startPosition, Vector3 endPosition, float deltaTime)
    {
        float normilizeTime = _discardDuration / _discardTime;

        Vector3 nextPos = Vector3.Lerp(startPosition, endPosition, normilizeTime) + (new Vector3(0, _discardOffsetY, 0) * _discardCurve.Evaluate(normilizeTime));

        _discardDuration += deltaTime;

        if (_discardDuration >= _discardTime)
        {
            _discardDuration = 0;
            _isDiskard = true;
        }

        return nextPos;
    }

    public Vector3 TakenPosition(Vector3 startPosition, Vector3 endPostion, float deltaTime)
    {
        float normilizeTime = _takenPositionDuration / _takenPositionTime;
        Vector3 nextPos = Vector3.Lerp(startPosition, endPostion, normilizeTime) + (new Vector3(0, _takenPositionOffsetY, 0) * _takenPositionCurve.Evaluate(normilizeTime));

        _takenPositionDuration += deltaTime;

        if (_takenPositionDuration >= _takenPositionTime)
        {
            _takenPositionDuration = _takenPositionTime;
            _isTakenPosition = true;
        }

        return nextPos;
    }

    public void EnabledIK(Transform targetForRightHand, Transform targetForLeftHand, Transform targetForChest)
    {
        _leftHandRigTarget.position = targetForLeftHand.position;

        _rightHandRigTarget.position = targetForRightHand.position;

        _chestRigTarget.rotation = targetForChest.rotation;
    }

    public void PlayeHitA()
    {
        PlayeCurrentAnimation(HashAnimationNames.Hit_A_String, HitSpeed, HashAnimationNames.AimString);
    }

    public void PlayeHitB()
    {
        PlayeCurrentAnimation(HashAnimationNames.Hit_B_String, HitSpeed, HashAnimationNames.AimString);
    }

    public void PlayDeaht()
    {
        PlayeCurrentAnimation(HashAnimationNames.DeathString, DeathSpeed, HashAnimationNames.DeathString, true);
    }

    public void PlayShoot(float durationReload)
    {
        _reloadSpeed = durationReload;

        PlayeCurrentAnimation(HashAnimationNames.ShootString, ShootSpeed, HashAnimationNames.ReloadString);
    }

    public void PlaySitIdle()
    {
        float offsetTime = 0.3f;
        float speed = (_animationClipData.SitIdleLenght / _takenPositionTime) + offsetTime;

        PlayeCurrentAnimation(HashAnimationNames.SitIdleString, speed, HashAnimationNames.SitStandUpString);
    }

    private void PlayNext(string animationName)
    {
        switch (animationName)
        {
            case HashAnimationNames.ReloadString:
                PlayReload();
                break;

            case HashAnimationNames.AimString:
                PlayAim();
                break;

            case HashAnimationNames.SitIdleString:
                PlaySitIdle();
                break;

            case HashAnimationNames.SitStandUpString:
                PlaySitStandUp();
                break;
        }
    }

    private void PlaySitStandUp()
    {
        PlayeCurrentAnimation(HashAnimationNames.SitStandUpString, SitInleSpeed, HashAnimationNames.AimString);
    }

    private void PlayReload()
    {
        float speed = ((_reloadSpeed - _animationClipData.RealoadLenght) / _animationClipData.RealoadLenght) + 1;

        PlayeCurrentAnimation(HashAnimationNames.ReloadString, speed, HashAnimationNames.AimString);
    }

    private void PlayAim()
    {
        PlayeCurrentAnimation(HashAnimationNames.AimString, AimSpeed, HashAnimationNames.AimString);
    }

    private void PlayeCurrentAnimation(string animationName, float animationSpeed, string nextAnimationName, bool isFinalAnimation = false)
    {
        //if (_animator.isActiveAndEnabled == false)
        //    return;

        if (_animator == null)
            return;

        _currentAnimationName = animationName;

        _animator.speed = animationSpeed;
        _animator.Play(_currentAnimationName, 0, 0f);
        _nextAnimationName = nextAnimationName;

        _isFinalAnimation = isFinalAnimation;
    }
}