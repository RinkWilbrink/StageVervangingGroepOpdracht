using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private AudioManagement _AudioManagement;
    [SerializeField] private AudioClip _ButtonSound;
    [SerializeField] private AudioClip _ButtonSoundSlide;

    [Header("In-game")]
    [SerializeField] private GameObject _BuildMenu;
    [SerializeField] private GameObject _PauseMenu;
    [SerializeField] private GameObject _SettingsMenu;
    [SerializeField] private GameObject _PauseMenuBackGround;

    [Header("MainMenu")]
    [SerializeField] private GameObject _MainMenuTitle;
    [SerializeField] private GameObject _MainMenuPlayButton;
    [SerializeField] private GameObject _MainMenuAnalyticsButton;
    [SerializeField] private GameObject _MainMenuSettingsButton;
    [SerializeField] private GameObject _MainMenuLevelBackButton;
    [SerializeField] private GameObject _MainMenuAnalyticsBackButton;
    [SerializeField] private GameObject _MainMenuSettingsBackButton;
    [SerializeField] private GameObject _MainMenuRewardButton;
    [SerializeField] private GameObject _MainMenuLevelSelectionMenu;
    [SerializeField] private GameObject _MainMenuAnalyticsMenu;
    [SerializeField] private float _MainMenuAnimationSpeed;

    private bool _BuildMenuOpen;
    private bool _PauseMenuOpen;
    private bool _SettingsMenuOpen;

    private bool _MainMenuLevelSelectionOpen;
    private bool _MainMenuSettingsMenuOpen;
    private bool _MainMenuAnalyticsMenuOpen;

    private bool _SetToClosePauseMenu;

    private void Start()
    {
        TriggerMainMenuStartAnimation();
    }

    private void Update()
    {
        if (_SetToClosePauseMenu && !LeanTween.isTweening(_PauseMenu))
        {
            _PauseMenuBackGround.SetActive(false);
            _PauseMenuOpen = false;
            _SetToClosePauseMenu = false;
        }
    }

    public void TriggerMainMenuStartAnimation()
    {
        LTSeq sequence = LeanTween.sequence();
        sequence.append(1f);
        sequence.append(LeanTween.moveLocalX(_MainMenuTitle, 0f, _MainMenuAnimationSpeed).setEaseInBack());
        sequence.append(LeanTween.rotateZ(_MainMenuTitle, 0f, _MainMenuAnimationSpeed).setEaseOutBack());
        sequence.append(-0.3f);
        sequence.append(LeanTween.scale(_MainMenuPlayButton, Vector3.one, _MainMenuAnimationSpeed).setEaseOutBack());
        sequence.append(-0.2f);
        sequence.append(LeanTween.rotateZ(_MainMenuPlayButton, 0f, _MainMenuAnimationSpeed).setEaseOutBack());
        sequence.append(-0.2f);
        sequence.append(LeanTween.moveLocalX(_MainMenuRewardButton, -850f, _MainMenuAnimationSpeed).setEaseOutBack());
        sequence.append(-0.2f);
        sequence.append(LeanTween.moveLocalX(_MainMenuAnalyticsButton, 850f, _MainMenuAnimationSpeed).setEaseOutBack());
        sequence.append(-0.2f);
        sequence.append(LeanTween.moveLocalX(_MainMenuSettingsButton, 850f, _MainMenuAnimationSpeed).setEaseOutBack());
    }

    public void OpenMainMenuUI()
    {
        LTSeq sequence = LeanTween.sequence();
        sequence.append(_MainMenuAnimationSpeed);
        sequence.append(LeanTween.scale(_MainMenuPlayButton, Vector3.one, _MainMenuAnimationSpeed).setEaseOutBack());
        sequence.append(-_MainMenuAnimationSpeed);
        sequence.append(LeanTween.moveLocalY(_MainMenuTitle, 250f, _MainMenuAnimationSpeed).setEaseInBack());
        sequence.append(-_MainMenuAnimationSpeed);
        sequence.append(LeanTween.moveLocalX(_MainMenuSettingsButton, 850f, _MainMenuAnimationSpeed).setEaseInBack());
        sequence.append(-_MainMenuAnimationSpeed);
        sequence.append(LeanTween.moveLocalX(_MainMenuAnalyticsButton, 850f, _MainMenuAnimationSpeed).setEaseInBack());
        sequence.append(-_MainMenuAnimationSpeed);
        sequence.append(LeanTween.moveLocalX(_MainMenuRewardButton, -850f, _MainMenuAnimationSpeed).setEaseInBack());
    }

    public void CloseMainMenuUI()
    {
        LTSeq sequence = LeanTween.sequence();

        sequence.append(LeanTween.scale(_MainMenuPlayButton, Vector3.zero, _MainMenuAnimationSpeed).setEaseInBack());
        sequence.append(-_MainMenuAnimationSpeed);
        sequence.append(LeanTween.moveLocalY(_MainMenuTitle, 750f, _MainMenuAnimationSpeed).setEaseInBack());
        sequence.append(-_MainMenuAnimationSpeed);
        sequence.append(LeanTween.moveLocalX(_MainMenuSettingsButton, 1200f, _MainMenuAnimationSpeed).setEaseInBack());
        sequence.append(-_MainMenuAnimationSpeed);
        sequence.append(LeanTween.moveLocalX(_MainMenuAnalyticsButton, 1200f, _MainMenuAnimationSpeed).setEaseInBack());
        sequence.append(-_MainMenuAnimationSpeed);
        sequence.append(LeanTween.moveLocalX(_MainMenuRewardButton, -1200f, _MainMenuAnimationSpeed).setEaseInBack());
    }

    public void TriggerMainMenuLevelSelection()
    {
        if (_MainMenuLevelSelectionOpen)
        {
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.moveY(_MainMenuLevelBackButton, -60f, _MainMenuAnimationSpeed).setEaseInBack());
            sequence.append(-_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_MainMenuLevelSelectionMenu, 1000f, _MainMenuAnimationSpeed).setEaseInBack());

            OpenMainMenuUI();

            _MainMenuLevelSelectionOpen = false;
        }
        else
        {
            CloseMainMenuUI();

            LTSeq sequence = LeanTween.sequence();
            sequence.append(_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_MainMenuLevelBackButton, -430f, _MainMenuAnimationSpeed).setEaseInBack());
            sequence.append(-_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_MainMenuLevelSelectionMenu, 0f, _MainMenuAnimationSpeed).setEaseInBack());

            _MainMenuLevelSelectionOpen = true;
        }
    }

    public void TriggerMainMenuAnalyticsMenu()
    {
        if (_MainMenuAnalyticsMenuOpen)
        {
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.moveLocalY(_MainMenuAnalyticsBackButton, -640f, _MainMenuAnimationSpeed).setEaseInBack());
            sequence.append(-_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_MainMenuAnalyticsMenu, 1000f, _MainMenuAnimationSpeed).setEaseInBack());

            OpenMainMenuUI();

            _MainMenuAnalyticsMenuOpen = false;
        }
        else
        {
            CloseMainMenuUI();

            LTSeq sequence = LeanTween.sequence();
            sequence.append(_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_MainMenuAnalyticsBackButton, -430f, _MainMenuAnimationSpeed).setEaseInBack());
            sequence.append(-_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_MainMenuAnalyticsMenu, 0f, _MainMenuAnimationSpeed).setEaseInBack());

            _MainMenuAnalyticsMenuOpen = true;
        }
    }

    public void TriggerMainMenuSettingsMenu()
    {
        if (_MainMenuSettingsMenuOpen)
        {
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.moveLocalY(_MainMenuSettingsBackButton, -640f, _MainMenuAnimationSpeed).setEaseInBack());
            sequence.append(-_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_SettingsMenu, 1000f, _MainMenuAnimationSpeed).setEaseInBack());

            OpenMainMenuUI();

            _MainMenuSettingsMenuOpen = false;
        }
        else
        {
            CloseMainMenuUI();

            LTSeq sequence = LeanTween.sequence();
            sequence.append(_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_MainMenuSettingsBackButton, -430f, _MainMenuAnimationSpeed).setEaseInBack());
            sequence.append(-_MainMenuAnimationSpeed);
            sequence.append(LeanTween.moveLocalY(_SettingsMenu, 0f, _MainMenuAnimationSpeed).setEaseInBack());

            _MainMenuSettingsMenuOpen = true;
        }
    }

    public void TriggerBuildMenu()
    {
        _AudioManagement.PlayAudioClip(_ButtonSoundSlide, AudioMixerGroups.SFX);
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
        _AudioManagement.PlayAudioClip(_ButtonSound, AudioMixerGroups.SFX);
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
        _AudioManagement.PlayAudioClip(_ButtonSound, AudioMixerGroups.SFX);
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
