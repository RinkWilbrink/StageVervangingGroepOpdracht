using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioObject : PoolItem
{
    [SerializeField] private AudioSource _AudioSource;
    [SerializeField] private AudioClip _CurrentAudioClip;

    protected override void Reset()
    {
    }

    public void PlayAudio(AudioClip audioClip, AudioMixerGroup audioMixerGroup)
    {
        _CurrentAudioClip = audioClip;
        _AudioSource.clip = _CurrentAudioClip;
        _AudioSource.outputAudioMixerGroup = audioMixerGroup;
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
