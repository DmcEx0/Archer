using UnityEngine;

namespace Archer.Data
{
    [CreateAssetMenu]
    public class AnimationClipData : ScriptableObject
    {
        [SerializeField] private AnimationClip _reload;
        [SerializeField] private AnimationClip _sitIdle;

        public float ReloadLength => _reload.length;
        public float SitIdleLength => _sitIdle.length;
    }
}