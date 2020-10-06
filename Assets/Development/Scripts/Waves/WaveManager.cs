using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject testUnit;
    private WaypointManager WaypointManager;

    private void Start() {
        WaypointManager = FindObjectOfType<WaypointManager>();
        Debug.Log("TIP: Press Spacebar to Instantiate a test unit...");
    }

    private void Update() {
        if ( Input.GetKeyDown(KeyCode.Space) )
            Instantiate(testUnit, WaypointManager.waypoints[0].position, Quaternion.identity);
    }
}
