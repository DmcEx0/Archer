using UnityEngine;

namespace Archer.Audio
{
    [System.Serializable]
    public class AudioItem
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private Sounds _sound;
        [SerializeField] private SoundType _type;

        public SoundType Type => _type;
        public Sounds Sound => _sound;
        public AudioClip Clip => _clip;
    }
}