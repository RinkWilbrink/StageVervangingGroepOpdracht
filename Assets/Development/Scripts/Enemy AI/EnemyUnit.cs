﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public Transform[] wayPoints;
    private int waypointIndex;

    public void Initialize(EnemyData e)
    {
        this.Health = e.health;
        this.Speed = e.speed;
        this.GoldReward = e.goldReward;
        this.AttackDamage = e.attackDamage;
    }

    private void Start()
    {
        // The values can be decided here but we need to figure out what type of enemy unit we are first
        Initialize(enemyData);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[waypointIndex].position, Speed * GameTime.deltaTime);

        // Need to test the rotation more
        Quaternion dir = Quaternion.LookRotation(wayPoints[waypointIndex].position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, dir, rotateSpeed * GameTime.deltaTime);

        if (Speed < 0)
        {
            Speed = 0;
        }

        if (slowDebuffActive)
        {
            slowDebuffTimer += GameTime.deltaTime;

            if (slowDebuffTimer > slowDebuffTime)
            {
                Speed += slowDownSpeed;
                slowDebuffTimer = 0f;
                slowDebuffActive = false;
            }
        }
        if (Vector3.Distance(transform.position, wayPoints[waypointIndex].position) < .1f)
        {
            if (waypointIndex < wayPoints.Length - 1)
            {
                waypointIndex++;
            }
            else
            {
                Death();
                GameController.MainTowerHP -= AttackDamage;
                // Do damage to the main structure
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health < 1)
            Death();
    }

    [NonSerialized] public int takeDamageOTTimer = 0;
    private bool takeDamageOTActive = false;

    public void PoisonDOT(int dps, int damageTime, int timeUntilDamageTaken = 1)
    {
        takeDamageOTTimer = 0;
        if (takeDamageOTActive == false)
        {
            takeDamageOTActive = true;
            
            StartCoroutine(TakeDamageOverTime(dps, damageTime, timeUntilDamageTaken));
        }
    }
    private IEnumerator TakeDamageOverTime(int dps, int damageTime, int timeUntilDamageTaken = 1)
    {
        while (takeDamageOTTimer < damageTime)
        {
            Health -= dps;
            yield return new WaitForSeconds(timeUntilDamageTaken);
            takeDamageOTTimer += (1 * (int)GameTime.deltaTime);
        }

        takeDamageOTActive = false;

        if (Health < 1)
        {
            Death();
        }
    }

    bool slowDebuffActive = false;
    float slowDownSpeed;
    float slowDownTotalSpeed;
    float slowDebuffTime;
    float slowDebuffTimer = 0;
    public void SlowDown(float speedDebuff, float time)
    {
        if (Speed > slowDownTotalSpeed)
        {
            slowDownSpeed = (speedDebuff / 100) * Speed;

            print((speedDebuff / 100) * Speed);
            Speed -= (speedDebuff / 100) * Speed;

            slowDownTotalSpeed = Speed;
            slowDebuffTime = time;

            slowDebuffActive = true;
        }
    }

    private void Death()
    {
        Destroy(gameObject);

        if (OnDeath != null)
        {
            OnDeath();
        }
    }
}
