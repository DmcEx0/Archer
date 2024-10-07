using Archer.Data;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Archer.Animations
{
    [RequireComponent(typeof(Animator), typeof(RigBuilder))]
    public class AnimationHandler : MonoBehaviour
    {
        private const float ShootSpeed = 1f;
        private const float AimSpeed = 0.7f;
        private const float HitSpeed = 1f;
        private const float DeathSpeed = 1f;
        private const float SitIdleSpeed = 1f;

        [SerializeField] private AnimationClipData _animationClipData;

        [Space] [SerializeField] private AnimationCurve _takenPositionCurve;
        [SerializeField] private float _takenPositionOffsetY;
        [SerializeField] private float _takenPositionTime;

        [Space] [SerializeField] private AnimationCurve _discardCurve;
        [SerializeField] private float _discardOffsetY;
        [SerializeField] private float _discardTime;

        [Space] [SerializeField] private Transform _leftHandRigTarget;
        [SerializeField] private Transform _rightHandRigTarget;
        [SerializeField] private Transform _chestRigTarget;

        private Animator _animator;

        private string _currentAnimationName;
        private string _nextAnimationName;

        private float _discardDuration;
        private float _takenPositionDuration;
        private float _reloadSpeed;

        private bool _isFinalAnimation = false;

        public bool IsTakenPosition { get; private set; }
        public bool IsDiscard { get; private set; }

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

        public Vector3 GetDiscardPosition(Vector3 startPosition, Vector3 endPosition, float deltaTime)
        {
            float normalizeTime = _discardDuration / _discardTime;

            Vector3 nextPos = Vector3.Lerp(startPosition, endPosition, normalizeTime) +
                              (new Vector3(0, _discardOffsetY, 0) * _discardCurve.Evaluate(normalizeTime));

            _discardDuration += deltaTime;

            if (_discardDuration >= _discardTime)
            {
                _discardDuration = 0;
                IsDiscard = true;
            }

            return nextPos;
        }

        public Vector3 GetTakenPosition(Vector3 startPosition, Vector3 endPosition, float deltaTime)
        {
            float normalizeTime = _takenPositionDuration / _takenPositionTime;
            Vector3 nextPos = Vector3.Lerp(startPosition, endPosition, normalizeTime) +
                              (new Vector3(0, _takenPositionOffsetY, 0) * _takenPositionCurve.Evaluate(normalizeTime));

            _takenPositionDuration += deltaTime;

            if (_takenPositionDuration >= _takenPositionTime)
            {
                _takenPositionDuration = _takenPositionTime;
                IsTakenPosition = true;
            }

            return nextPos;
        }

        public void EnableIK(Transform targetForRightHand, Transform targetForLeftHand, Transform targetForChest)
        {
            _leftHandRigTarget.position = targetForLeftHand.position;

            _rightHandRigTarget.position = targetForRightHand.position;

            _chestRigTarget.rotation = targetForChest.rotation;
        }

        public void PlayHitA()
        {
            PlayCurrentAnimation(AnimationConstants.HitAString, HitSpeed, AnimationConstants.AimString);
        }

        public void PlayHitB()
        {
            PlayCurrentAnimation(AnimationConstants.HitBString, HitSpeed, AnimationConstants.AimString);
        }

        public void PlayDeath()
        {
            PlayCurrentAnimation(AnimationConstants.DeathString, DeathSpeed, AnimationConstants.DeathString, true);
        }

        public void PlayShoot(float durationReload)
        {
            _reloadSpeed = durationReload;

            PlayCurrentAnimation(AnimationConstants.ShootString, ShootSpeed, AnimationConstants.ReloadString);
        }

        public void PlaySitIdle()
        {
            float offsetTime = 0.3f;
            float speed = (_animationClipData.SitIdleLength / _takenPositionTime) + offsetTime;

            PlayCurrentAnimation(AnimationConstants.SitIdleString, speed, AnimationConstants.SitStandUpString);
        }

        private void PlayNext(string animationName)
        {
            switch (animationName)
            {
                case AnimationConstants.ReloadString:
                    PlayReload();
                    break;

                case AnimationConstants.AimString:
                    PlayAim();
                    break;

                case AnimationConstants.SitIdleString:
                    PlaySitIdle();
                    break;

                case AnimationConstants.SitStandUpString:
                    PlaySitStandUp();
                    break;
            }
        }

        private void PlaySitStandUp()
        {
            PlayCurrentAnimation(AnimationConstants.SitStandUpString, SitIdleSpeed, AnimationConstants.AimString);
        }

        private void PlayReload()
        {
            float speed = ((_reloadSpeed - _animationClipData.ReloadLength) / _animationClipData.ReloadLength) + 1;

            PlayCurrentAnimation(AnimationConstants.ReloadString, speed, AnimationConstants.AimString);
        }

        private void PlayAim()
        {
            PlayCurrentAnimation(AnimationConstants.AimString, AimSpeed, AnimationConstants.AimString);
        }

        private void PlayCurrentAnimation(string animationName, float animationSpeed, string nextAnimationName,
            bool isFinalAnimation = false)
        {
            if (_animator == null)
                return;

            _currentAnimationName = animationName;

            _animator.speed = animationSpeed;
            _animator.Play(_currentAnimationName, 0, 0f);
            _nextAnimationName = nextAnimationName;

            _isFinalAnimation = isFinalAnimation;
        }
    }
}