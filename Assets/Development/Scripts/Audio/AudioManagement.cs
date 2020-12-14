using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum AudioMixerGroups { Music, SFX, Voiceline}

public class AudioManagement : MonoBehaviour
{
    private AudioMixerGroups _AudioMixerGroups;
    private Pooling _Pooling;
    [SerializeField] private AudioClip _Clip; //DEBUG
    [SerializeField] private AudioMixer _MasterMixer;
    [SerializeField] private AudioMixerGroup _AudioMixerGroupMusic;
    [SerializeField] private AudioMixerGroup _AudioMixerGroupSFX;
    [SerializeField] private AudioMixerGroup _AudioMixerGroupVoiceline;

    private void Start()
    {
        _Pooling = GetComponent<Pooling>();
    }

    private void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            PlayAudioClip(_Clip, AudioMixerGroups.SFX);
        }
    }

    public void PlayAudioClip(AudioClip audioClip, AudioMixerGroups audiomixergroups)
    {
        switch (audiomixergroups)
        {
            case AudioMixerGroups.Music:
                _Pooling.InstantiateItem(audioClip, _AudioMixerGroupMusic);
                break;
            case AudioMixerGroups.SFX:
                _Pooling.InstantiateItem(audioClip, _AudioMixerGroupSFX);
                break;
            case AudioMixerGroups.Voiceline:
                _Pooling.InstantiateItem(audioClip, _AudioMixerGroupVoiceline);
                break;
        }
    }

    public void SetMusicVolume(float musicVolume)
    {
        _MasterMixer.SetFloat("MusicVolume", musicVolume);
    }
    
    public void SetSFXVolume(float sfxVolume)
    {
        _MasterMixer.SetFloat("SFXVolume", sfxVolume);
    }

    public void SetVoicelineVolume(float voicelineVolume)
    {
        _MasterMixer.SetFloat("VoicelineVolume", voicelineVolume);
    }
}
