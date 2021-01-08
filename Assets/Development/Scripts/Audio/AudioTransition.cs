using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTransition : MonoBehaviour
{
    private static AudioTransition _Instance;

    private void Awake()
    {
        Debug.Log("test");
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(_Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
