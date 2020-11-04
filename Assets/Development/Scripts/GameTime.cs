using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    // Variables
    public static float deltaTime;

    private static float TimeMultiplier = 1f;
    
    void Update()
    {
        GameTime.deltaTime = Time.deltaTime * TimeMultiplier;
    }

    public static void SetGameTime(float timeScale)
    {
        TimeMultiplier = timeScale;
    }
}
