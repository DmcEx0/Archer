using UnityEngine;

namespace Archer.Audio
{
    [System.Serializable]
    public class AudioItem
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private Sounds _sound;
        [SerializeField] private SoundType _type;

        [Range(0f, 1f)]
        [SerializeField] private float _volume;

        public SoundType Type => _type;
        public Sounds Sound => _sound;
        public AudioClip Clip => _clip;
    }
}