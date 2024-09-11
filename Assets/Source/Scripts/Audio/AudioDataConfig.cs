using System.Collections.Generic;
using System.Linq;
using Archer.Utils;
using UnityEngine;

namespace Archer.Audio
{
    [CreateAssetMenu]
public class AudioDataConfig : ScriptableObject
{
    [SerializeField] private List<AudioItem> _audioItems;

    private AudioSource _sfxAudioSource;
    private AudioSource _musicAudioSource;

    private float _sfxVolume;
    private float _musicVolume;

    private ITimeControllable _currentController = null;

    public bool SfxIsOn => _sfxVolume != 0;
    public bool MusicIsOn => _musicVolume != 0;

    public void Init(AudioSource sfxAudioSource, AudioSource musicAudioSource)
    {
        _musicAudioSource = musicAudioSource;
        _sfxAudioSource = sfxAudioSource;
    }

    public void Pause(ITimeControllable controller)
    {
        if (_musicAudioSource == null || _sfxAudioSource == null)
            return;

        if (_currentController == null)
            _currentController = controller;
        else
            return;

        _sfxAudioSource.Pause();
        _musicAudioSource.Pause();
    }

    public void UnPause(ITimeControllable controller)
    {
        if (_musicAudioSource == null || _sfxAudioSource == null)
            return;

        if (controller != _currentController)
            return;

        _sfxAudioSource.UnPause();
        _musicAudioSource.UnPause();

        _currentController = null;
    }

    public void SetActiveSfx(bool isSfxOn)
    {
        _sfxVolume = isSfxOn ? 1 : 0;

        _sfxAudioSource.volume = _sfxVolume;
    }

    public void SetActiveMusic(bool isMusicOn)
    {
        _musicVolume = isMusicOn ? 1 : 0;

        _musicAudioSource.volume = _musicVolume;
    }

    public void Play(Sounds sound)
    {
        if (_musicAudioSource == null || _sfxAudioSource == null)
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
            _sfxAudioSource.volume = _sfxVolume;
            _sfxAudioSource.PlayOneShot(audio.Clip);
        }
    }
}
}