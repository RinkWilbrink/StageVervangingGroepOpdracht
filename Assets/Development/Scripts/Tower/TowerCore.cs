using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tower
{
    public enum SpecialAttack
    {
        None = 0, Special1 = 1, Special2 = 2
    }

    public enum TowerType
    {
        ArcherTower = 0, WizardTower = 1, CannonTower = 2
    }

    public class TowerCore : MonoBehaviour
    {
        // Variables
        [Header("Stats")]
        [SerializeField] protected float AttackShootingTime;
        [SerializeField] protected float AttackDamage;

        [HideInInspector] protected bool SpecialAttackMode = false;

        [Header("Damage and Firerate Upgrades")]
        [SerializeField] private float DamageAddedPerLevel;
        [SerializeField] private float FireRateAddedPerLevel;

        [Header("Shooting and Range")]
        [SerializeField] private float ShootingRange = 0;
        [SerializeField] protected GameObject ShootOrigin;
        [SerializeField] public GameObject specialDirectionUI;
        [SerializeField] public GameObject Bullet;
        [HideInInspector] private RaycastHit hit;
        [SerializeField] public Transform FirePoint;
        [Header("Upgrades and Special Abilities")]
        [SerializeField] public int TowerLevelToUnlockSpecial;
        [HideInInspector] public int TowerLevel = 1;
        [HideInInspector] public int TowerSpecialLevel = 0;
        protected float UpgradedDamage;
        protected float UpgradedFireRate;

        [SerializeField] public TowerType towerType;
        [HideInInspector] public SpecialAttack SpecialUnlocked;

        [Header("Targets")]
        [SerializeField] protected GameObject CurrentTarget;

        [Header("Sprites And Art")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] NoSpecialModeSprites;
        [SerializeField] private Sprite[] Special1ModeSprites;
        [SerializeField] private Sprite[] Special2ModeSprites;

        // Hidden Primairy Attack Variables
        [HideInInspector] protected float AttackTimer;
        [HideInInspector] protected bool CanAttack = true;

        // Init function gets called when the tower gets destroyed
        public virtual void Init()
        {

        }

        private void Update()
        {
            CheckTargets();
            HandleAttackTiming();
            HandleShooting();
        }

        // Look for targets, set the target closest as CurrentTarget
        private void CheckTargets()
        {
            Collider[] enemies = Physics.OverlapSphere(ShootOrigin.transform.position, ShootingRange, 1 << 9);

            if(enemies.Length == 0)
            {
                CurrentTarget = null;
            }
            else
            {
                foreach(Collider go in enemies)
                {
                    if(go.tag == "Enemy")
                    {
                        float Distance1 = ShootingRange;

                        if(CurrentTarget != null)
                        {
                            Distance1 = Mathf.Sqrt((CurrentTarget.transform.position - ShootOrigin.transform.position).sqrMagnitude);
                        }

                        float Distance2 = Mathf.Sqrt((go.transform.position - ShootOrigin.transform.position).sqrMagnitude);

                        if(Distance2 < Distance1)
                        {
                            CurrentTarget = go.gameObject;
                        }
                    }
                }
            }
        }

        // Handle the attack timing for the primairy attack
        private void HandleAttackTiming()
        {
            if (SpecialAttackMode == false)
            {
                if (AttackTimer >= (AttackShootingTime - UpgradedFireRate))
                {
                    CanAttack = true;
                }
                else
                {
                    AttackTimer += Time.deltaTime;
                }
            }
        }

        #region Virtual Functions

        protected virtual void PrimaryAttack()
        {
            CurrentTarget.GetComponent<EnemyUnit>().TakeDamage(AttackDamage);

            CanAttack = false;
            AttackTimer = 0;
        }
        protected virtual void SecondaryAttack()
        {
            specialDirectionUI.SetActive(false);
        }

        protected virtual void HandleShooting()
        {
            if(CanAttack)
            {
                if(CurrentTarget != null)
                {
                    PrimaryAttack();
                    Shoot();
                }
            }
        }
        
        void Shoot()
        {
            GameObject bulletGO =(GameObject)Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
            Projectile bullet = bulletGO.GetComponent<Projectile>();

            if(bullet != null)
            {
                bullet.seek(CurrentTarget);
            }
        }

        protected void PayMana(int _cost)
        {
            GameController.Mana -= _cost;
        }

        #endregion

        #region Public Functions

        public void UpdateDamageValues()
        {
            AttackDamage += DamageAddedPerLevel;
            AttackShootingTime += FireRateAddedPerLevel;
            //UpgradedDamage = (DamageAddedPerLevel * (TowerLevel - 1));
            //UpgradedFireRate = (FireRateAddedPerLevel * (TowerLevel - 1));
        }

        public void StartSecondairyAttack()
        {
            SecondaryAttack();

            SpecialAttackMode = true;
            AttackTimer = 0;
        }

        /// <summary>Set a new sprite</summary>
        public void SetNewSprite()
        {
            switch(SpecialUnlocked)
            {
                case SpecialAttack.None:
                    if(TowerLevel <= NoSpecialModeSprites.Length)
                    {
                        spriteRenderer.sprite = NoSpecialModeSprites[TowerLevel - 1];
                    }
                    break;
                case SpecialAttack.Special1:
                    if(TowerSpecialLevel < Special1ModeSprites.Length)
                    {
                        spriteRenderer.sprite = Special1ModeSprites[TowerSpecialLevel];
                    }
                    break;
                case SpecialAttack.Special2:
                    if(TowerSpecialLevel < Special2ModeSprites.Length)
                    {
                        spriteRenderer.sprite = Special2ModeSprites[TowerSpecialLevel];
                    }
                    break;
            }
        }

        public virtual void SpecialAttackDirectionLookAt()
        {
            if(CurrentTarget != null)
            {
                specialDirectionUI.transform.LookAt(CurrentTarget.transform.position);
            }
        }

        #endregion
    }
}