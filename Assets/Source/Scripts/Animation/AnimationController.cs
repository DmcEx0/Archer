using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private const float ShootSpeed = 1f;
    private const float AimSpeed = 0.7f;
    private const float HitSpeed = 1f;
    private const float DeathSpeed = 1f;
    private const float SitInleSpeed = 1f;

    [SerializeField] private AnimationClipData _animationClipData;
    [SerializeField] private AnimationCurve _changePositionCurve;
    [SerializeField] private float _offsetTime;

    [Space]
    [SerializeField] private Transform _leftHandRigTarget;
    [SerializeField] private Transform _rightHandRigTarget;
    [SerializeField] private Transform _chestRigTarget;

    private Animator _animator;

    private string _currentAnimationName;
    private string _nextAnimationName;

    private float _duration;
    private float _reloadSpeed;

    private bool _isFinalAnimation = false;
    private bool _isFinalCurve = false;

    public bool IsFinalCurve => _isFinalCurve;

    private void Update()
    {
        if (_isFinalAnimation)
            return;

        if (_currentAnimationName == null)
            return;

        AnimatorStateInfo currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (currentStateInfo.IsName(_currentAnimationName) && currentStateInfo.normalizedTime >= 0.95f)
        {
            PlayNext(_nextAnimationName);
        }
    }

    public void Init(Animator animator)
    {
        _animator = animator;
    }
    private float _dur;
    public Vector3 TakenPosition2(Vector3 startPosition, Vector3 endPostion, float deltaTime)
    {
        _isFinalCurve = false;
        float normilizeTime = _dur / _offsetTime;
        Vector3 nextPos = Vector3.Lerp(startPosition, endPostion, normilizeTime) + (new Vector3(0, 3f, 0) * _changePositionCurve.Evaluate(normilizeTime));

        _dur += deltaTime;

        if (_dur >= _offsetTime)
        {
            _dur = 0;
            _isFinalCurve = true;
        }

        return nextPos;
    }

    public Vector3 TakenPosition(Vector3 startPosition, Vector3 endPostion, float deltaTime)
    {
        float normilizeTime = _duration / _offsetTime;
        Vector3 nextPos = Vector3.Lerp(startPosition, endPostion, normilizeTime) + (new Vector3(0, 3f, 0) * _changePositionCurve.Evaluate(normilizeTime));

        _duration += deltaTime;

        if (_duration >= _offsetTime)
        {
            _duration = _offsetTime;
        }

        return nextPos;
    }

    public void EnabledIK(Transform targetForRightHand, Transform targetForLeftHand, Transform targetForChest)
    {
        _leftHandRigTarget.position = targetForLeftHand.position;

        _rightHandRigTarget.position = targetForRightHand.position;

        _chestRigTarget.rotation = targetForChest.rotation;
    }

    public void SetTargetsForHands(Transform targetForRightHand, Transform targetForLeftHand, Transform targetForChest)
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
        float speed = (_animationClipData.SitIdleLenght / _offsetTime) + offsetTime;

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
        float speed = _animationClipData.RealoadLenght / _reloadSpeed;
        PlayeCurrentAnimation(HashAnimationNames.ReloadString, speed, HashAnimationNames.AimString);
    }

    private void PlayAim()
    {
        PlayeCurrentAnimation(HashAnimationNames.AimString, AimSpeed, HashAnimationNames.AimString);
    }

    private void PlayeCurrentAnimation(string animationName, float animationSpeed, string nextAnimationName, bool isFinalAnimation = false)
    {
        if (_animator.isActiveAndEnabled == false)
            return;

        _currentAnimationName = animationName;

        _animator.Play(_currentAnimationName, 0, 0f);
        _animator.speed = animationSpeed;

        _nextAnimationName = nextAnimationName;

        _isFinalAnimation = isFinalAnimation;
    }
}