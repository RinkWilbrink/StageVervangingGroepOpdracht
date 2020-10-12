using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 60;
    [SerializeField] private EnemyData enemyData;

    public int Health { get; private set; }
    public float Speed { get; private set; }
    public float GoldReward { get; private set; }
    public int AttackDamage { get; private set; }

    public event Action OnDeath;

    private WaypointManager WayPointManager;
    private int waypointIndex;

    public void Initialize( EnemyData e ) {
        this.Health = e.health;
        this.Speed = e.speed;
        this.GoldReward = e.goldReward;
        this.AttackDamage = e.attackDamage;
    }

    private void Start() {
        WayPointManager = FindObjectOfType<WaypointManager>();
        // The values can be decided here but we need to figure out what type of enemy unit we are first
        Initialize(enemyData);
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, WayPointManager.waypoints[waypointIndex].position, Speed * Time.deltaTime);

        // Need to test the rotation more
        Quaternion dir = Quaternion.LookRotation(WayPointManager.waypoints[waypointIndex].position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, dir, rotateSpeed * Time.deltaTime);

        //Vector3 dir = WayPointManager.waypoints[waypointIndex].position - transform.position;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Mathf.Atan2(dir.x, dir.y) / Mathf.PI * 180, 0), 0.1f);

        if ( Input.GetKeyDown(KeyCode.E) )
            TakeDamage(1);

        if ( Vector3.Distance(transform.position, WayPointManager.waypoints[waypointIndex].position) < WayPointManager.waypointDeadZone )
            if ( waypointIndex < WayPointManager.waypoints.Length - 1 ) {
                waypointIndex++;
            } else {
                Death();
                GameController.MainTowerHP -= AttackDamage;
                // Do damage to the main structure
            }
    }

    public void TakeDamage( int damage ) {
        Health -= damage;

        if ( Health < 1 )
            Death();
    }

    private void Death() {
        Destroy(gameObject);

        if ( OnDeath != null )
            OnDeath();
    }
}
