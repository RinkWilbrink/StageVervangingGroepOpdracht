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
        [SerializeField] protected int AttackShootingTime;
        [SerializeField] protected int AttackDamage;

        [Space(4)]

        [SerializeField] protected int SpecialShootingTime;
        [SerializeField] protected int SpecialDamage;

        [Header("Damage and Firerate Upgrades")]
        [SerializeField] private float DamageAddedPerLevel;
        [SerializeField] private float FireRateAddedPerLevel;

        [Header("Shooting and Range")]
        [SerializeField] private float ShootingRange = 0;
        [SerializeField] private GameObject ShootOrigin;
        [SerializeField] public GameObject specialDirectionUI;
        [HideInInspector] private RaycastHit hit;

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

        private void Update()
        {
            CheckTargets();
            HandleAttackTiming();
            HandleShooting();
        }

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
                        float Distance1 = ShootingRange + 1f;

                        if(CurrentTarget != null)
                        {
                            Distance1 = Mathf.Sqrt(
                                Mathf.Pow((CurrentTarget.transform.position.x - ShootOrigin.transform.position.x), 2f) +
                                Mathf.Pow((CurrentTarget.transform.position.z - ShootOrigin.transform.position.z), 2f));
                        }

                        float Distance2 = Mathf.Sqrt(
                            Mathf.Pow((go.transform.transform.position.x - ShootOrigin.transform.position.x), 2f) +
                            Mathf.Pow((go.transform.transform.position.z - ShootOrigin.transform.position.z), 2f));

                        if(Distance2 < Distance1)
                        {
                            CurrentTarget = go.gameObject;
                        }
                    }
                }
            }
        }

        private void HandleAttackTiming()
        {
            if(AttackTimer >= (AttackShootingTime - UpgradedFireRate))
            {
                CanAttack = true;
            }
            else
            {
                AttackTimer += Time.deltaTime;
            }
        }

        #region Virtual Functions

        // Virtual functions for shooting and special abilities
        protected virtual void PrimaryAttack()
        {
            Debug.Log("Core Primairy");

            CurrentTarget.GetComponent<EnemyUnit>().TakeDamage(AttackDamage);

            CanAttack = false;
            AttackTimer = 0;
        }
        protected virtual void SecondaryAttack()
        {
            //Debug.Log("Core Secondairy"); ;
        }

        protected virtual void HandleShooting()
        {
            if(CanAttack)
            {
                if(CurrentTarget != null)
                {
                    PrimaryAttack();
                }
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
            UpgradedDamage = (DamageAddedPerLevel * (TowerLevel - 1));
            UpgradedFireRate = (FireRateAddedPerLevel * (TowerLevel - 1));
        }

        public void StartSecondairyAttack()
        {
            SecondaryAttack();
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

        public virtual void LookAt()
        {
            if(CurrentTarget != null)
            {
                specialDirectionUI.transform.LookAt(CurrentTarget.transform.position);
            }
        }

        #endregion
    }
}