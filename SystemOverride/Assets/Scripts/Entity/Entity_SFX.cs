using Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_SFX : MonoBehaviour, IPoolable
{
    AudioSource _audioSource;
    public AudioSource audioSource
    {
        get { return _audioSource; }
    }
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip, float volume)
    {
        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.Play();
    }


    public void PlayClip(AudioClip clip, bool loop, float volume)
    {
        _audioSource.loop = true;
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void OnAlloc()
    {
        _audioSource.loop = false;
        _audioSource.clip = null;
        _audioSource.volume = 0;
    }

    public void OnRelease()
    {

    }
}
