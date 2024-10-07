using System.Collections.Generic;
using UnityEngine;

namespace Archer.Audio
{
    [CreateAssetMenu]
    public class AudioDataConfig : ScriptableObject
    {
        [SerializeField] private List<AudioItem> _audioItems;

        public IReadOnlyList<AudioItem> AudioItems => _audioItems;
    }
}