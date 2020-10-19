using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    // Variables
    private float timer = 0f;
    private int fps = 0;

    void Update()
    {
        if(timer >= 1f)
        {
            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("FPS: {0}", fps);

            timer = 0f;
            fps = 0;
        }
        else
        {
            timer += Time.deltaTime;
            fps++;
        }
    }
}