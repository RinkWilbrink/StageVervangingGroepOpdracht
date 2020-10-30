﻿using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData[] waves;
    [Space(10)]
    [SerializeField] private float waveCooldown = 10;

    [Space(6)]
    [SerializeField] private UpgradeUI upgradeUI;

    private WaypointManager WaypointManager;

    private WaveData currentWave;
    private int currentWaveNum;
    private int enemiesLeftToSpawn;
    private int enemiesLeftAlive;
    private float spawnNext;

    private void Start()
    {
        WaypointManager = FindObjectOfType<WaypointManager>();

        updateWave = UpdateWave(waveCooldown);
        StartCoroutine(updateWave);

    }

    //float spawnTimer;
    int spawnCurveIndex;
    private void Update()
    {
        if (enemiesLeftToSpawn > 0 && Time.time > spawnNext)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        spawnCurveIndex++;
        enemiesLeftToSpawn--;
        //Debug.Log(enemiesLeftToSpawn);

        if (currentWave.spawnIntensity.length < 1)
        {
            spawnNext = Time.time + UnityEngine.Random.Range(currentWave.minSpawnTime, currentWave.maxSpawnTime);
        }
        else
        {
            spawnNext = Time.time + currentWave.spawnIntensity.Evaluate(spawnCurveIndex);
        }

        int random = UnityEngine.Random.Range(0, 100);
        int lowestPercentage;
        int highestPercentage = 0;

        for (int i = 0; i < currentWave.enemies.Length; i++)
        {
            lowestPercentage = highestPercentage;
            highestPercentage += currentWave.enemies[i].chance;

            if (random >= lowestPercentage && random < highestPercentage)
            {

                EnemyUnit enemy = Instantiate(currentWave.enemies[i].enemy, currentWave.enemies[i].waypointManager.waypoints[0].position, Quaternion.identity);
                enemy.wayPoints = currentWave.enemies[i].waypointManager.waypoints;
                enemy.upgradeUi = upgradeUI;

                enemy.OnDeath += OnEnemyDeath;
            }
        }
    }

    IEnumerator updateWave;
    private void OnEnemyDeath()
    {
        enemiesLeftAlive--;

        if (enemiesLeftAlive <= 0)
        {
            updateWave = UpdateWave(waveCooldown);
            StartCoroutine(updateWave);
        }
    }

    private IEnumerator UpdateWave(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        currentWaveNum++;
        currentWave = waves[currentWaveNum - 1];

        enemiesLeftToSpawn = currentWave.enemyCount;
        enemiesLeftAlive = enemiesLeftToSpawn;

        GameController.Gold += currentWave.goldReward;
    }

    [Serializable]
    public struct WaveData
    {
        public int enemyCount;
        public int goldReward;
        public AnimationCurve spawnIntensity;
        public float minSpawnTime;
        public float maxSpawnTime;
        public EnemyStructure[] enemies;
    }

    [Serializable]
    public struct EnemyStructure
    {
        public EnemyUnit enemy;
        public int chance;
        public WaypointManager waypointManager;
    }
}
