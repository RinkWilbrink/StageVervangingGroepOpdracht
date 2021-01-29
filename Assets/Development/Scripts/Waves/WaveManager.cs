using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI WaveText;
    [SerializeField] private SelectionButtonManager SelectionButtonManager;
    [SerializeField] private GameObject EndScreen;
    [SerializeField] private TextMeshProUGUI FinalScoreText;
    [SerializeField] private TextMeshProUGUI GoldScoreText;
    [SerializeField] private TextMeshProUGUI HealthScoreText;
    [SerializeField] private int GoldMultiplier;
    [SerializeField] private int HealthMultiplier;
    [SerializeField] private AudioClip[] coinDropAudio;
    [SerializeField] private AudioClip[] waveAudio;
    [SerializeField] private GameObject beginWaveIcon;
    [SerializeField] private GameObject enemyList;

    [Header("Waves")]
    [SerializeField] private int levelNummer;
    [SerializeField] private WaveData[] waves;
    [Space(10)]
    [SerializeField] private float waveCooldown = 10;

    private WaveData currentWave;
    [SerializeField] private int currentWaveNum;
    private int enemiesLeftToSpawn;
    private int enemiesLeftAlive;
    private float spawnNext;

    private void Start() {
        currentWave = waves[currentWaveNum];
        updateWave = UpdateWave(waveCooldown);
        StartCoroutine(updateWave);
    }

    //float spawnTimer;
    private float spawnCurveIndex;
    private void Update() {
        //Debug.LogError(currentWaveNum);
        //print("Enemies left to spawn: " + enemiesLeftToSpawn);
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
        float lowestPercentage;
        float highestPercentage = 0;

        for ( int i = 0; i < currentWave.enemies.Length; i++ ) {
            lowestPercentage = highestPercentage;
            highestPercentage += currentWave.enemies[i].chance;

            if ( random >= lowestPercentage && random < highestPercentage ) {

                EnemyUnit enemy = Instantiate(currentWave.enemies[i].enemy, currentWave.enemies[i].waypointManager.waypoints[0].position, Quaternion.identity, enemyList.transform);
                enemy.wayPoints = currentWave.enemies[i].waypointManager.waypoints;

                enemy.OnDeath += OnEnemyDeath;
            }
        }
    }

    private IEnumerator updateWave;
    private void OnEnemyDeath() {
        enemiesLeftAlive--;
        //print("Enemies left alive: " + enemiesLeftAlive);

        if ( enemiesLeftAlive <= 0 ) {
            //print("Updating wave...");
            updateWave = UpdateWave(waveCooldown);

            FindObjectOfType<AudioManagement>().PlayAudioClip(waveAudio[1], AudioMixerGroups.SFX);

            GameController.Gold += currentWave.goldReward;
            FindObjectOfType<ResourceUIManager>().UpdateResourceUI();
            SelectionButtonManager.UpdateTowerButtonUI();

            StartCoroutine(updateWave);
        }
    }

    private IEnumerator UpdateWave( float waitTime ) {
        currentWaveNum++;
        if ( currentWaveNum > waves.Length ) {
            EndScreen.SetActive(true);
            GoldScoreText.text = GameController.Gold.ToString() + " x " + GoldMultiplier;
            HealthScoreText.text = GameController.MainTowerHP.ToString() + " x " + HealthMultiplier;
            FinalScoreText.text = ((GameController.Gold * GoldMultiplier) + (GameController.MainTowerHP * HealthMultiplier)).ToString();

            DataManager.LevelComplete(levelNummer);

            Time.timeScale = 0;
        } else {
            WaveText.text = string.Format("{0}", currentWaveNum);
            currentWave = waves[currentWaveNum - 1];

            beginWaveIcon.SetActive(true);

            for ( int i = 0; i < currentWave.enemies.Length; i++ ) {
                currentWave.enemies[i].waypointManager.spawnIndicator.SetActive(true);
            }
        }

        yield return new WaitForSeconds(waitTime);

        FindObjectOfType<AudioManagement>().PlayAudioClip(waveAudio[0], AudioMixerGroups.SFX);

        enemiesLeftToSpawn = currentWave.enemyCount;
        enemiesLeftAlive = enemiesLeftToSpawn;

        new WaitForSeconds(0.01f);

        beginWaveIcon.SetActive(false);

        for ( int i = 0; i < currentWave.enemies.Length; i++ ) {
            currentWave.enemies[i].waypointManager.spawnIndicator.SetActive(false);
        }
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
        public float chance;
        public WaypointManager waypointManager;
    }
}
