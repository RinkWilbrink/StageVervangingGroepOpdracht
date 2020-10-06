using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public float waypointDeadZone = .5f;
    public Transform[] waypoints;

    private void Start() {
        waypoints = new Transform[transform.childCount];
        for ( int i = 0; i < waypoints.Length; i++ ) {
            waypoints[i] = transform.GetChild(i);
        }
    }
}
