using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : PoolItem
{
    [SerializeField] private AudioSource _AudioSource;
    [SerializeField] private AudioClip _CurrentAudioClip;

    protected override void Reset()
    {
    }

    public void PlayAudio(AudioClip audioClip)
    {
        _CurrentAudioClip = audioClip;
        _AudioSource.clip = _CurrentAudioClip;
        _AudioSource.Play();
    }

    private void Update()
    {
        if (!_AudioSource.isPlaying)
        {
            ReturnToPool();
        }
    }
}
