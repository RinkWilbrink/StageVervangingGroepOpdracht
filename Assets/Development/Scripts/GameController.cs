using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Resources
    public static int Gold = 10;
    public static int Mana = 10;

    // Other things
    public static int MainTowerHP = 50;

    private void Update() {

        if ( MainTowerHP < 1 ) {
            Debug.Log("GAME OVER");
        } else {
            //Debug.LogFormat("Main Tower HP: {0}", MainTowerHP);
        }
    }
}
