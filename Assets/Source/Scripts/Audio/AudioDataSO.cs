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

    public bool SfxIsOn
    {
        get
        {
            if (_SFXVolume == 1)
                return true;
            if (_SFXVolume == 0)
                return false;

            return true;
        }
    }

    public bool MusicIsOn
    {
        get
        {
            if (_musicVolume == 1)
                return true;
            if (_musicVolume == 0)
                return false;

            return true;
        }
    }

    public void Init(AudioSource SFXAudioSource, AudioSource MusicAudioSource)
    {
        _musicAudioSource = MusicAudioSource;
        _SFXAudioSource = SFXAudioSource;
    }

    public void SetActiveSFX(bool isSFXOn)
    {
        if (isSFXOn == false)
            _SFXVolume = 0;
        else
            _SFXVolume = 1;

        _SFXAudioSource.volume = _SFXVolume;
    }

    public void SetActiveMusic(bool isMusicOn)
    {
        if (isMusicOn == false)
            _musicVolume = 0;
        else
            _musicVolume = 1;

        _musicAudioSource.volume = _musicVolume;
    }

    public void Play(Sounds sound)
    {
        if (_musicAudioSource == null && _SFXAudioSource == null)
            throw new System.Exception();

        AudioItem audio = _audioItems.FirstOrDefault(a => a.Sound == sound);

        if (audio.Type == SoundType.Music)
        {
            _musicAudioSource.loop = true;
            _musicAudioSource.PlayOneShot(audio.Clip);
            _musicAudioSource.volume = _musicVolume;
        }

        if (audio.Type == SoundType.SFX)
        {
            _SFXAudioSource.PlayOneShot(audio.Clip);
            _SFXAudioSource.volume = _SFXVolume;
        }
    }
}