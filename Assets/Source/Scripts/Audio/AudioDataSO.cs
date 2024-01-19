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

    public void Pause()
    {
        if (_musicAudioSource == null)
            return;

        _SFXAudioSource.Pause();
        _musicAudioSource.Pause();
    }

    public void UpPause()
    {
        if (_musicAudioSource == null)
            return;

        _SFXAudioSource.UnPause();
        _musicAudioSource.UnPause();
    }

    public void SetActiveSFX(bool isSFXOn)
    {
        _SFXVolume = isSFXOn ? 1 : 0;

        _SFXAudioSource.volume = _SFXVolume;
    }

    public void SetActiveMusic(bool isMusicOn)
    {
        _musicVolume = isMusicOn ? 1 : 0;

        _musicAudioSource.volume = _musicVolume;
    }

    public void Play(Sounds sound)
    {
        if (_musicAudioSource == null && _SFXAudioSource == null)
            throw new System.Exception();

        AudioItem audio = _audioItems.FirstOrDefault(a => a.Sound == sound);

        if (audio.Type == SoundType.Music)
        {
            _musicAudioSource.volume = _musicVolume;
            _musicAudioSource.loop = true;
            _musicAudioSource.clip = audio.Clip;
            _musicAudioSource.Play();
        }

        if (audio.Type == SoundType.SFX)
        {
            _SFXAudioSource.volume = _SFXVolume;
            _SFXAudioSource.PlayOneShot(audio.Clip);
        }
    }
}