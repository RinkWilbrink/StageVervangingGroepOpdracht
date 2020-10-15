using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData[] waves;
    [Space(10)]
    //[SerializeField] private EnemyUnit testUnit;
    [SerializeField] private float waveCooldown = 10;

    private WaypointManager WaypointManager;

    private WaveData currentWave;
    private int currentWaveNum;
    private int enemiesLeftToSpawn;
    private int enemiesLeftAlive;
    private float spawnNext;

    private void Start() {
        WaypointManager = FindObjectOfType<WaypointManager>();

        Debug.Log("TIP: Press Spacebar to Instantiate a test unit...");

        updateWave = UpdateWave(waveCooldown);
        StartCoroutine(updateWave);
    }

    //float spawnTimer;
    float spawnCurveIndex;
    private void Update() {
        print("Wave: " + currentWaveNum);

        if ( /*Input.GetKeyDown(KeyCode.Space) && */ enemiesLeftToSpawn > 0 && Time.time > spawnNext ) {
            SpawnEnemy();
        }

        //spawnTimer += Time.deltaTime;
        //spawnNext = currentWave.curve.Evaluate(spawnTimer);
        //Debug.Log(spawnNext);
        //print(currentWave.curve.Evaluate(Time.time));
    }

    private void SpawnEnemy() {
        //yield return new WaitForSeconds(currentWave.curve.Evaluate(Time.timeSinceLevelLoad));
        spawnCurveIndex++;
        //int enemiesToSpawn = (int)currentWave.curve.Evaluate(spawnCurveIndex);

        //for ( int e = 0; e < enemiesToSpawn; e++ ) {
        enemiesLeftToSpawn--;
        Debug.Log(enemiesLeftToSpawn);

        if ( currentWave.spawnIntensity.length < 1 ) {
            spawnNext = Time.time + UnityEngine.Random.Range(currentWave.minSpawnTime, currentWave.maxSpawnTime);
        } else if ( currentWave.normalizeCurve && currentWave.spawnIntensity.length >= 1 ) {
            float spawnTime = spawnCurveIndex / currentWave.enemyCount;
            spawnNext = Time.time + currentWave.spawnIntensity.Evaluate(spawnTime);
            Debug.Log("Next Spawn: " + currentWave.spawnIntensity.Evaluate(spawnTime));
        } else if ( !currentWave.normalizeCurve && currentWave.spawnIntensity.length >= 1 ) {
            spawnNext = Time.time + currentWave.spawnIntensity.Evaluate((int)spawnCurveIndex);
            Debug.Log("Next Spawn: " + ( spawnNext - Time.time ));
        }

        //print("Time until next spawn: " + currentWave.curve.Evaluate(Time.time));

        int random = UnityEngine.Random.Range(0, 100);
        int lowestPercentage;
        int highestPercentage = 0;

        for ( int i = 0; i < currentWave.enemies.Length; i++ ) {
            //currentWave.enemies[currentWaveNum - 1].chance
            lowestPercentage = highestPercentage;
            highestPercentage += currentWave.enemies[i].chance;

            if ( random >= lowestPercentage && random < highestPercentage ) {
                //print("Random: " + random);
                //print("Lowest: " + lowestPercentage);
                //print("Highest: " + highestPercentage);

                EnemyUnit enemy = Instantiate(currentWave.enemies[i].enemy, currentWave.enemies[i].waypointManager.waypoints[0].position, Quaternion.identity);
                enemy.wayPoints = currentWave.enemies[i].waypointManager.waypoints;

                enemy.OnDeath += OnEnemyDeath;
            }
        }
        //}
    }

    IEnumerator updateWave;
    private void OnEnemyDeath() {
        enemiesLeftAlive--;

        if ( enemiesLeftAlive <= 0 ) {
            updateWave = UpdateWave(waveCooldown);
            StartCoroutine(updateWave);
        }
    }

    private IEnumerator UpdateWave( float waitTime ) {
        yield return new WaitForSeconds(waitTime);

        currentWaveNum++;
        currentWave = waves[currentWaveNum - 1];

        enemiesLeftToSpawn = currentWave.enemyCount;
        enemiesLeftAlive = enemiesLeftToSpawn;

        //spawnTimer = 0f;

        GameController.Gold += currentWave.goldReward;
    }

    [Serializable]
    public struct WaveData
    {
        public int enemyCount;
        public int goldReward;
        public AnimationCurve spawnIntensity;
        public bool normalizeCurve;
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
