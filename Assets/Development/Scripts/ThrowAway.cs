using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAway : MonoBehaviour
{
    [SerializeField] bool SaveGame;

    void Start()
    {
        DataManager.LevelComplete(1);
    }
}
