using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private const float _shootDuration = 1f;
    private const float _aimDuration = 0.3f;

    [SerializeField] private Transform _leftHandRigTarget;
    [SerializeField] private Transform _rightHandRigTarget;
    [SerializeField] private Transform _chestRigTarget;

    private Animator _animator;

    private string _currentAnimationName;
    private string _nextAnimationName;

    private float _duration;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        PlayNext(HashAnimationNames.AimString);
    }

    private void Update()
    {
        AnimatorStateInfo currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);


        if (currentStateInfo.IsName(_currentAnimationName) && currentStateInfo.normalizedTime >= 1f)
        {
            PlayNext(_nextAnimationName);
        }
    }

    public void SetTargetsForHands(Transform targetForRightHand, Transform targetForLeftHand, Transform targetForChest)
    {
        _leftHandRigTarget.position = targetForLeftHand.position;

        _rightHandRigTarget.position = targetForRightHand.position;

        _chestRigTarget.rotation = targetForChest.rotation;
    }

    public void PlayeHitA()
    {
        _currentAnimationName = HashAnimationNames.Hit_A_String;

        _animator.Play(_currentAnimationName);

        _nextAnimationName = HashAnimationNames.AimString;
    }

    public void PlayeHitB()
    {
        _currentAnimationName = HashAnimationNames.Hit_B_String;

        _animator.Play(_currentAnimationName);

        _nextAnimationName = HashAnimationNames.AimString;
    }

    public void PlayShoot(float durationReload)
    {
        _duration = durationReload;

        _currentAnimationName = HashAnimationNames.ShootString;
        _animator.Play(_currentAnimationName, 0, 0f);
        _animator.speed = _shootDuration;
        _nextAnimationName = HashAnimationNames.ReloadString;
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
        }
    }

    private void PlayReload()
    {
        _currentAnimationName = HashAnimationNames.ReloadString;

        _animator.Play(_currentAnimationName, 0, 0f);

        _animator.speed = 1f / (_duration - _shootDuration);

        _nextAnimationName = HashAnimationNames.AimString;
    }

    private void PlayAim()
    {
        _currentAnimationName = HashAnimationNames.AimString;
        _animator.Play(_currentAnimationName, 0, 0f);
        _animator.speed = _aimDuration;
        _nextAnimationName = _currentAnimationName;
    }
}