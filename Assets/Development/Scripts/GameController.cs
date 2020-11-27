﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Resources
    public static int Gold = 15;
    public static int Mana = 10;
    public static int Gems;

    private void Awake() {
        Gems = PlayerPrefs.GetInt("Gems");
    }

    //public static bool GameIsPaused = false;

    // Other things
    public static int MainTowerHP = 50;
}
