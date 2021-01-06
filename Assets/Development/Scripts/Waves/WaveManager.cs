using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI WaveText;
    [SerializeField] private GameObject EndScreen;
    [SerializeField] private AudioClip[] coinDropAudio;
    [SerializeField] private AudioClip[] waveAudio;
    [SerializeField] private GameObject beginWaveIcon;

    [Header("Waves")]
    [SerializeField] private WaveData[] waves;
    [Space(10)]
    [SerializeField] private float waveCooldown = 10;

    private WaveData currentWave;
    private int currentWaveNum;
    private int enemiesLeftToSpawn;
    private int enemiesLeftAlive;
    private float spawnNext;

    private void Start() {
        updateWave = UpdateWave(waveCooldown);
        StartCoroutine(updateWave);
    }

    //float spawnTimer;
    private float spawnCurveIndex;
    private void Update() {
        //Debug.LogError(currentWaveNum);

        //if ( GameTime.deltaTime > 0 ) {
        if ( enemiesLeftToSpawn > 0 && Time.time > spawnNext ) {
            SpawnEnemy();
        }
        //}
    }

    private void SpawnEnemy() {
        spawnCurveIndex++;
        enemiesLeftToSpawn--;
        //Debug.Log(enemiesLeftToSpawn);

        if ( currentWave.spawnIntensity.length < 1 ) {
            spawnNext = Time.time + UnityEngine.Random.Range(currentWave.minSpawnTime, currentWave.maxSpawnTime);
        } else if ( currentWave.normalizeCurve && currentWave.spawnIntensity.length >= 1 ) {
            float spawnTime = spawnCurveIndex / currentWave.enemyCount;
            spawnNext = Time.time + currentWave.spawnIntensity.Evaluate(spawnTime);
            //Debug.Log("Next Spawn: " + currentWave.spawnIntensity.Evaluate(spawnTime));
        } else if ( !currentWave.normalizeCurve && currentWave.spawnIntensity.length >= 1 ) {
            spawnNext = Time.time + currentWave.spawnIntensity.Evaluate((int)spawnCurveIndex);
            //Debug.Log("Next Spawn: " + (spawnNext - Time.time));
        }

        int random = UnityEngine.Random.Range(0, 100);
        int lowestPercentage;
        int highestPercentage = 0;

        for ( int i = 0; i < currentWave.enemies.Length; i++ ) {
            lowestPercentage = highestPercentage;
            highestPercentage += currentWave.enemies[i].chance;

            if ( random >= lowestPercentage && random < highestPercentage ) {

                EnemyUnit enemy = Instantiate(currentWave.enemies[i].enemy, currentWave.enemies[i].waypointManager.waypoints[0].position, Quaternion.identity);
                enemy.wayPoints = currentWave.enemies[i].waypointManager.waypoints;

                enemy.OnDeath += OnEnemyDeath;
            }
        }
    }

    private IEnumerator updateWave;
    private void OnEnemyDeath() {
        enemiesLeftAlive--;

        if ( enemiesLeftAlive <= 0 ) {
            updateWave = UpdateWave(waveCooldown);

            FindObjectOfType<AudioManagement>().PlayAudioClip(waveAudio[1], AudioMixerGroups.SFX);

            GameController.Gold += currentWave.goldReward;
            FindObjectOfType<ResourceUIManager>().UpdateResourceUI();

            StartCoroutine(updateWave);
        }
    }

    private IEnumerator UpdateWave( float waitTime ) {
        currentWaveNum++;

        beginWaveIcon.SetActive(true);


        if ( currentWaveNum > waves.Length ) {
            EndScreen.SetActive(true);

            Time.timeScale = 0;
        }

        yield return new WaitForSeconds(waitTime);

        currentWave = waves[currentWaveNum - 1];

        WaveText.text = string.Format("{0}", currentWaveNum);
        FindObjectOfType<AudioManagement>().PlayAudioClip(waveAudio[0], AudioMixerGroups.SFX);

        enemiesLeftToSpawn = currentWave.enemyCount;
        enemiesLeftAlive = enemiesLeftToSpawn;

        new WaitForSeconds(0.01f);

        beginWaveIcon.SetActive(false);
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
