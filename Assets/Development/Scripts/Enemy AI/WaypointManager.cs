using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public float waypointDeadZone = .5f;
    public Transform[] waypoints;

    private void Start() {
        if ( waypoints.Length < 1 ) {
            waypoints = new Transform[transform.childCount];

            for ( int i = 0; i < waypoints.Length; i++ ) {
                waypoints[i] = transform.GetChild(i);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        for ( int i = 0; i < waypoints.Length - 1; i++ ) {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
