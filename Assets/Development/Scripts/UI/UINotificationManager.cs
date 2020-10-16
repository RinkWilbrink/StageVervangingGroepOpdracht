using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UINotificationManager : MonoBehaviour
{
    // Variables
    [SerializeField] private GameObject NotificationObject;
    private float Timer = 0f;

    private void Start()
    {
        NotificationObject.SetActive(false);
    }

    public void OpenGoldNotification()
    {
        NotificationObject.SetActive(true);
        Timer += Time.deltaTime;
    }

    private void Update()
    {
        if(Timer > 0)
        {
            if(Timer >= 2f)
            {
                NotificationObject.SetActive(false);
                Timer = 0;
            }
            else
            {
                Timer += Time.deltaTime;
            }

        }
    }
}