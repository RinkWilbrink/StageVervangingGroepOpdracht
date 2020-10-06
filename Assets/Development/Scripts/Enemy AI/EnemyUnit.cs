using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 60;

    public int Health { get; private set; }
    public float Speed { get; private set; }
    public float Reward { get; private set; }
    public int AttackDamage { get; private set; }

    private WaypointManager WayPointManager;
    private int waypointIndex;

    private void Start() {
        WayPointManager = FindObjectOfType<WaypointManager>();
        // The values can be decided here but we need to figure out what type of enemy unit we are first
        Health = 2;
        Speed = 5;
        Reward = 6;
        AttackDamage = 1;
    }

    private void Update() {
        if ( Health < 1 )
            Destroy(gameObject);

        transform.position = Vector3.MoveTowards(transform.position, WayPointManager.waypoints[waypointIndex].position, Speed * Time.deltaTime);

        // Need to test the rotation more
        Quaternion dir = Quaternion.LookRotation(WayPointManager.waypoints[waypointIndex].position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, dir, rotateSpeed * Time.deltaTime);

        //Vector3 dir = WayPointManager.waypoints[waypointIndex].position - transform.position;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Mathf.Atan2(dir.x, dir.y) / Mathf.PI * 180, 0), 0.1f);

        if ( Vector3.Distance(transform.position, WayPointManager.waypoints[waypointIndex].position) < WayPointManager.waypointDeadZone )
            if ( waypointIndex < WayPointManager.waypoints.Length - 1 ) {
                waypointIndex++;
            } else {
                Health = 0;
                // Do damage to the main structure
            }
    }
}
