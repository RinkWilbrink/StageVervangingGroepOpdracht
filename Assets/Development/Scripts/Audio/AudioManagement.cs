using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    private Pooling _Pooling;
    [SerializeField] private AudioClip _Clip; //DEBUG

    private void Start()
    {
        _Pooling = GetComponent<Pooling>();
    }

    private void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            PlayAudioClip(_Clip);
        }
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        _Pooling.InstantiateItem(gameObject.transform, audioClip);
    }
}
