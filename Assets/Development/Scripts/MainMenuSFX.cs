using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSFX : MonoBehaviour
{
    [SerializeField] private AudioManagement audioManagement;
    [SerializeField] private AudioClip[] buttonAudio;

    public void ButtonAudio(int i) {
        audioManagement.PlayAudioClip(buttonAudio[i], AudioMixerGroups.SFX);
    }
}
