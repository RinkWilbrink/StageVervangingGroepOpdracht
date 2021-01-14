using System;
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
    [SerializeField] private float resistanceMultiplier;


    public float Health; //{ get; private set; }
    public float Speed { get; private set; }
    public int GoldReward { get; private set; }
    public int AttackDamage { get; private set; }

    public event Action OnDeath;

    public Transform[] wayPoints;
    private int waypointIndex;

    private SpriteRenderer spriteRenderer;

    private ResourceUIManager resourceUIManager;
    private SelectionButtonManager selectionButtonManager;
    private UI.UpgradeUI upgradeUI;

    public void Initialize(EnemyData e)
    {
        this.Health = e.health;
        this.Speed = e.speed;
        this.GoldReward = e.goldReward;
        this.AttackDamage = e.attackDamage;
    }


    private void Awake() {
        //wayPoints = FindObjectOfType<WaypointManager>();
        resourceUIManager = FindObjectOfType<ResourceUIManager>();
        upgradeUI = FindObjectOfType<UI.UpgradeUI>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        selectionButtonManager = FindObjectOfType<SelectionButtonManager>();

        //if ( walkSheet.Length > 0 )
        //    StartCoroutine(AnimatedWalk());

        // The values can be decided here but we need to figure out what type of enemy unit we are first
        Initialize(enemyData);
    }


    Vector3 lastPos;
    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[waypointIndex].position, Speed * GameTime.deltaTime);

        Vector3 spritePos = transform.position - lastPos;

        if ( spritePos.x >= 0 )
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        // Need to test the rotation more
        //Quaternion dir = Quaternion.LookRotation(wayPoints[waypointIndex].position - transform.position);
        //transform.rotation = Quaternion.Lerp(transform.rotation, dir, rotateSpeed * GameTime.deltaTime);

        transform.rotation = Quaternion.Euler(transform.localRotation.x, 180f, transform.rotation.z);

        //Vector3 dir = wayPoints[waypointIndex].position - transform.position;
        //print(dir);

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
            } else {
                AttackDeath();
                //GameController.MainTowerHP -= AttackDamage;
                // Do damage to the main structure
                upgradeUI.DoMainTowerDamage(AttackDamage);
            }

        lastPos = transform.position;
    }

    [SerializeField] private Sprite[] walkSheet;
    [SerializeField] private float animSpeed = .1f;

    public void TakeDamage(float damage, TowerType towerTypeWeakness)
    {
        int towerRes = (int)towerTypeWeakness + 1;

        if (towerRes + 1 < (int)TowerType.NullValue)
        {
            towerRes += 1;
        }
        else if(towerRes + 1 == (int)TowerType.NullValue)
        {
            towerRes = 0;
        }

        TowerType towerTypeResistance = (TowerType)towerRes;

        if (towerTypeWeakness != TowerType.NullValue && towerWeakness == towerTypeWeakness)
        {
            Health -= (damage * damageMultiplier);
        }
        else if (towerTypeWeakness != TowerType.NullValue && towerWeakness == towerTypeResistance)
        {
            Health -= (damage / resistanceMultiplier);
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

        DataManager.ResourcesGained(GoldReward);
        DataManager.EnemySlayed();

        resourceUIManager.UpdateResourceUI();
        selectionButtonManager.UpdateTowerButtonUI();

        Destroy(gameObject);

        if ( OnDeath != null )
            OnDeath();
    }

    private void AttackDeath() {
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
