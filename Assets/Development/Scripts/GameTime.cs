using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    // Variables
    /// <summary>Time between frames, like Time.deltaTime but this can be slowed down with time scale independently from Time.deltaTime,
    /// GameTime.deltaTime is used for Game Calculation regarding time like enemy movement or attack timing.</summary>
    public static float deltaTime;

    /// <summary>Total running GameTime</summary>
    public static float time;

    private static float TimeMultiplier = 1f;
    
    void Update()
    {
        GameTime.deltaTime = Time.deltaTime * TimeMultiplier;

        time += GameTime.deltaTime;
    }

    /// <summary>Set the TimeScale for GameTime.deltaTime, This can be slowed down or set to 0 to pause the game and slow down timers and movement speed.</summary>
    /// <param name="timeScale"></param>
    public static void SetTimeScale(float timeScale)
    {
        TimeMultiplier = timeScale;
    }
}
