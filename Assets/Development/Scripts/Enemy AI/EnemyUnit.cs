﻿using System;
using System.Collections;
using UnityEngine;
using Tower;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 60;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private GameObject frostOverlayImage;
    [SerializeField] private TowerType towerWeakness;
    [SerializeField] private float damageMultiplier;

    public float Health { get; private set; }
    public float Speed { get; private set; }
    public int GoldReward { get; private set; }
    public int AttackDamage { get; private set; }

    public event Action OnDeath;

    public Transform[] wayPoints;
    private int waypointIndex;

    private ResourceUIManager resourceUIManager;
    private UI.UpgradeUI upgradeUI;

    public void Initialize(EnemyData e)
    {
        this.Health = e.health;
        this.Speed = e.speed;
        this.GoldReward = e.goldReward;
        this.AttackDamage = e.attackDamage;
    }

    private void Start()
    {
        //wayPoints = FindObjectOfType<WaypointManager>();
        resourceUIManager = FindObjectOfType<ResourceUIManager>();
        upgradeUI = FindObjectOfType<UI.UpgradeUI>();
        // The values can be decided here but we need to figure out what type of enemy unit we are first
        Initialize(enemyData);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[waypointIndex].position, Speed * GameTime.deltaTime);

        // Need to test the rotation more
        Quaternion dir = Quaternion.LookRotation(wayPoints[waypointIndex].position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, dir, rotateSpeed * GameTime.deltaTime);

        //Vector3 dir = WayPointManager.waypoints[waypointIndex].position - transform.position;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Mathf.Atan2(dir.x, dir.y) / Mathf.PI * 180, 0), 0.1f);

        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1, TowerType.NullValue);

        if (Speed < 0)
            Speed = 0;

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

        // Test
        if (Input.GetKeyDown(KeyCode.S))
            SlowDown(80f, 4f);

        if (Input.GetKeyDown(KeyCode.G))
            StartCoroutine(TakeDamageOverTime(1, 2));

        if (Vector3.Distance(transform.position, wayPoints[waypointIndex].position) < .1f)
            if (waypointIndex < wayPoints.Length - 1)
            {
                waypointIndex++;
            }
            else
            {
                Death();
                //GameController.MainTowerHP -= AttackDamage;
                // Do damage to the main structure
                upgradeUI.DoMainTowerDamage(AttackDamage);
            }
    }

    public void TakeDamage(float damage, TowerType towerType)
    {
        if (towerType != TowerType.NullValue && towerWeakness == towerType)
        {
            Health -= damage * damageMultiplier;
        }
        else
        {
            Health -= damage;
        }

        if (Health < 1)
            Death();
    }

    [NonSerialized] public int takeDamageOTTimer = 0;
    private bool takeDamageOTActive = false;
    public IEnumerator TakeDamageOverTime(int dps, int damageTime, int timeUntilDamageTaken = 1)
    {
        takeDamageOTActive = true;
        takeDamageOTTimer = 0;

        while (takeDamageOTTimer < damageTime)
        {
            Health -= dps;
            if (Health < 1)
                Death();
            yield return new WaitForSeconds(timeUntilDamageTaken);
            takeDamageOTTimer++;
        }

        takeDamageOTActive = false;
    }

    private bool slowDebuffActive = false;
    private float slowDownSpeed;
    private float slowDownTotalSpeed;
    private float slowDebuffTime;
    private float slowDebuffTimer = 0;
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
        GameController.Gold += GoldReward;
        resourceUIManager.UpdateResourceUI();

        Destroy(gameObject);

        if (OnDeath != null)
            OnDeath();
    }

    public IEnumerator FrostOverlay(float maxTimer)
    {
        float timer = 0;
        frostOverlayImage.SetActive(true);
        while (timer < maxTimer)
        {
            timer += GameTime.deltaTime;

            yield return null;
        }

        frostOverlayImage.SetActive(false);
    }

    public IEnumerator FireOverlay(float maxTimer)
    {
        yield return null;
    }

    public void PoisonDOT(int dps, int damageTime, int timeUntilDamageTaken = 1)
    {
        takeDamageOTTimer = 0;
        if (takeDamageOTActive == false)
        {
            takeDamageOTActive = true;

            StartCoroutine(TakeDamageOverTime(dps, damageTime, timeUntilDamageTaken));
        }
    }
}
