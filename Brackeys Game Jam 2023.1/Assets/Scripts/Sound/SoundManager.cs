using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectSource;
    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void ChangeMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
    
    public void ChangeMusicVolume(float value)
    {
        _musicSource.volume = value;
    }
    
    public void ChangeEffectsVolume(float value)
    {
        _effectSource.volume = value;
    }

    public void StopMusic()
    {
        _musicSource.Pause();
    }
    
    public void StopEffect()
    {
        _effectSource.Stop();
    }
    
    public void ResumeMusic()
    {
        _musicSource.Play();
    }

    public bool EffectsIsPlaying()
    {
        return _effectSource.isPlaying;
    }
}
