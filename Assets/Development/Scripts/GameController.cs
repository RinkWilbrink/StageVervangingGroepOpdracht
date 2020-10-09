using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int Gold = 10;
    public static int MainTowerHP = 50;

    private void Update() {

        if ( MainTowerHP < 1 ) {
            Debug.Log("GAME OVER");
        } else {
            Debug.Log("Main Tower HP: " + MainTowerHP);
        }
    }
}
