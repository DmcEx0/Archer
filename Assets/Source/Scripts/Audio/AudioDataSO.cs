using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class AudioDataSO : ScriptableObject
{
    [SerializeField] private List<AudioItem> _audioItems;

    private AudioSource _SFXAudioSource;
    private AudioSource _musicAudioSource;

    private float _SFXVolume;
    private float _musicVolume;

    private bool _isSFXOn;
    private bool _isMusicOn;

    public void Init(AudioSource SFXAudioSource, AudioSource MusicAudioSource)
    {
        _musicAudioSource = MusicAudioSource;
        _SFXAudioSource = SFXAudioSource;

        _SFXVolume = 1f;
        _musicVolume = 1f;

        _musicAudioSource.volume = _musicVolume;
        _SFXAudioSource.volume = _SFXVolume;
    }

    public void SetActiveSFX(bool isSFXOn)
    {
        //_isSFXOn = isSFXOn;

        if (isSFXOn == false)
            _SFXVolume = 0;
        else
            _SFXVolume = 1;

        _SFXAudioSource.volume = _SFXVolume;
    }

    public void SetActiveMusic(bool isMusicOn)
    {
        //_isMusicOn = isMusicOn;

        if (isMusicOn == false)
            _musicVolume = 0;
        else
            _musicVolume = 1;

        _musicAudioSource.volume = _musicVolume;
    }

    public void Play(Sounds sound, bool isLoop = false)
    {
        if (_musicAudioSource == null && _SFXAudioSource == null)
            throw new System.Exception();

        AudioItem audio = _audioItems.FirstOrDefault(a => a.Sound == sound);

        if (audio.Type == SoundType.Music)
        {
            _musicAudioSource.loop = isLoop;
            _musicAudioSource.PlayOneShot(audio.Clip);
        }

        if (audio.Type == SoundType.SFX)
        {
            _SFXAudioSource.PlayOneShot(audio.Clip);
        }
    }
}