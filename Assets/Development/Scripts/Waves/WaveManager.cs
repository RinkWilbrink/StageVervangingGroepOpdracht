using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI WaveText;
    [SerializeField] private SelectionButtonManager SelectionButtonManager;
    [SerializeField] private GameObject EndScreen;
    [SerializeField] private AudioClip[] coinDropAudio;
    [SerializeField] private AudioClip[] waveAudio;
    [SerializeField] private GameObject beginWaveIcon;

    [Header("Waves")]
    [SerializeField] private int levelNummer;
    [SerializeField] private WaveData[] waves;
    [Space(10)]
    [Header("Wave Cooldown")]
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
            float spawnTime = spawnCurveIndex / SheetData.EnemyCount[currentWaveNum - 1];
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
        print("Enemies left alive: " + enemiesLeftAlive);

        if ( enemiesLeftAlive <= 0 ) {
            print("Updating wave...");
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

        //CSVWaveData waveData = ReadData(currentWaveNum - 1);
        print(SheetData.GoldReward[currentWaveNum - 1]);
        enemiesLeftToSpawn = SheetData.EnemyCount[currentWaveNum - 1];
        enemiesLeftAlive = enemiesLeftToSpawn;

        new WaitForSeconds(0.01f);

        beginWaveIcon.SetActive(false);

        for ( int i = 0; i < currentWave.enemies.Length; i++ ) {
            currentWave.enemies[i].waypointManager.spawnIndicator.SetActive(false);
        }
        GameController.Gold += SheetData.GoldReward[currentWaveNum - 1];
    }

    [Serializable]
    public struct WaveData
    {
        private int enemyCount;
        private int goldReward;
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

    // Local data reading
    private CSVWaveData ReadData( int waveIndex ) {
        string fileName = "Yokai TD - WaveData.csv";
        string filePath = Application.dataPath + "/Development/Sheet Data/" + fileName;
        string s = File.ReadAllText(filePath);
        CSVWaveData data = new CSVWaveData();

        string[] lines = s.Split('\n');
        string[] fields = lines[waveIndex].Split(',');

        data = new CSVWaveData() {
            goldReward = int.Parse(fields[0])
        };

        return data;
    }

    public struct CSVWaveData
    {
        public int goldReward;
    }
}
