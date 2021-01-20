using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum AudioMixerGroups { Music, SFX, Voiceline}

public class AudioManagement : MonoBehaviour
{
    private AudioMixerGroups _AudioMixerGroups;
    private Pooling _Pooling;
    private GameObject _MusicAudioObject;
    [SerializeField] private AudioClip _MusicTrack0;
    [SerializeField] private AudioClip _MusicTrack1;
    [SerializeField] private AudioMixer _MasterMixer;
    [SerializeField] private AudioMixerGroup _AudioMixerGroupMusic;
    [SerializeField] private AudioMixerGroup _AudioMixerGroupSFX;
    [SerializeField] private AudioMixerGroup _AudioMixerGroupVoiceline;
    [SerializeField] private Slider _MusicVolumeSlider;
    [SerializeField] private Slider _SFXVolumeSlider;
    [SerializeField] private Slider _VoiceVolumeSlider;

    private void Start()
    {
        //SET VOLUME TOSLIDER VALUES
        _Pooling = GetComponent<Pooling>();
        _MusicAudioObject = GameObject.Find("MusicAudioObject");

        if (_MusicAudioObject != null)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                _MusicAudioObject.GetComponent<AudioSource>().clip = _MusicTrack0;
            }
            else
            {
                _MusicAudioObject.GetComponent<AudioSource>().clip = _MusicTrack1;
            }
        }

        if (_MusicAudioObject != null)
        {
            _MusicAudioObject.GetComponent<AudioSource>().Play();
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
        _MasterMixer.SetFloat("MusicVolume", Mathf.Log10 (musicVolume) * 20);
    }

    public void EnableMusicLowPass()
    {
        _MasterMixer.SetFloat("MusicLowPass", 600);
    }
    
    public void DisableMusicLowPass()
    {
        _MasterMixer.SetFloat("MusicLowPass", 22000);
    }
    
    public void SetSFXVolume(float sfxVolume)
    {
        _MasterMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }

    public void SetVoicelineVolume(float voicelineVolume)
    {
        _MasterMixer.SetFloat("VoicelineVolume", Mathf.Log10(voicelineVolume) * 20);
    }
}
