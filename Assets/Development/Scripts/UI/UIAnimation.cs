using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private AudioClip _ButtonSound;
    [SerializeField] private AudioClip _ButtonSoundSlide;
    [SerializeField] private GameObject _BuildMenu;
    [SerializeField] private GameObject _PauseMenu;
    [SerializeField] private GameObject _SettingsMenu;
    [SerializeField] private GameObject _PauseMenuBackGround;
    private bool _BuildMenuOpen;
    private bool _PauseMenuOpen;
    private bool _SettingsMenuOpen;

    private bool _SetToClosePauseMenu;

    private void Update()
    {
        if (_SetToClosePauseMenu && !LeanTween.isTweening(_PauseMenu))
        {
            _PauseMenuBackGround.SetActive(false);
            _PauseMenuOpen = false;
            _SetToClosePauseMenu = false;
        }
    }

    public void TriggerBuildMenu()
    {
        FindObjectOfType<AudioManagement>().PlayAudioClip(_ButtonSoundSlide, AudioMixerGroups.SFX);
        if (_BuildMenuOpen)
        {
            LeanTween.moveLocalX(_BuildMenu, -5f, 0.2f).setEaseOutQuart();
            _BuildMenuOpen = false;
        }
        else
        {
            LeanTween.moveLocalX(_BuildMenu, 820f, 0.2f).setEaseOutQuart();
            _BuildMenuOpen = true;
        }
    }

    public void TriggerPauseMenu()
    {
        FindObjectOfType<AudioManagement>().PlayAudioClip(_ButtonSound, AudioMixerGroups.SFX);
        if (_PauseMenuOpen)
        {
            _SetToClosePauseMenu = true;
            LeanTween.moveLocalY(_PauseMenu, 1000f, 0.3f).setEaseInBack().setIgnoreTimeScale(true);
        }
        else
        {
            _PauseMenuBackGround.SetActive(true);
            LeanTween.moveLocalY(_PauseMenu, 0f, 0.3f).setEaseOutBack().setIgnoreTimeScale(true);
            _PauseMenuOpen = true;
        }
    }

    public void TriggerSettingsMenu()
    {
        if (_SettingsMenuOpen)
        {
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.moveLocalY(_SettingsMenu, 1000f, 0.3f).setEaseInBack().setIgnoreTimeScale(true));
            sequence.append(-0.1f);
            sequence.append(LeanTween.moveLocalY(_PauseMenu, 0f, 0.3f).setEaseOutBack().setIgnoreTimeScale(true));
            _SettingsMenuOpen = false;
        }
        else
        {
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.moveLocalY(_PauseMenu, -1000f, 0.3f).setEaseInBack().setIgnoreTimeScale(true));
            sequence.append(-0.1f);
            sequence.append(LeanTween.moveLocalY(_SettingsMenu, 0f, 0.3f).setEaseOutBack().setIgnoreTimeScale(true));
            _SettingsMenuOpen = true;
        }
    }
}
