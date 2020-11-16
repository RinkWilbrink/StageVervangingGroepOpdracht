using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    // Variables
    public static float deltaTime;

    public static float time;

    private static float TimeMultiplier = 1f;
    
    void Update()
    {
        GameTime.deltaTime = Time.deltaTime * TimeMultiplier;

        time += GameTime.deltaTime;
    }

    public static void SetTimeScale(float timeScale)
    {
        TimeMultiplier = timeScale;
    }
}
