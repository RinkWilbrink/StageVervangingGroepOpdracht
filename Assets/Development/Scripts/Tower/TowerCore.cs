using UnityEngine;

namespace Tower
{
    public class TowerCore : MonoBehaviour
    {
        // Variables
        [Header("Stats")]
        [SerializeField] protected int AttackShootingTime;
        [SerializeField] protected int AttackDamage;

        [Space(4)]

        [SerializeField] protected int SpecialShootingTime;
        [SerializeField] protected int SpecialDamage;
        [SerializeField] protected int SpecialAttackThresshold;

        // Hidden Primairy Attack Variables
        [HideInInspector] protected float AttackTimer;
        [HideInInspector] protected bool CanAttack = true;

        [Header("Damage and Firerate Upgrades")]
        [SerializeField] private float DamageAddedPerLevel;
        [SerializeField] private float FireRateAddedPerLevel;
        [SerializeField] public int DamageLevel = 0;
        [SerializeField] public int FireRateLevel = 0;
        protected float UpgradedDamage;
        protected float UpgradedFireRate;

        // Hidden Secondairy Attack Variables
        [HideInInspector] public bool CanUseSpecial = false;

        [Header("Shooting and Range")]
        [SerializeField] private GameObject ShootOrigin;

        [HideInInspector] private int EnemiesInRange = 0;
        [HideInInspector] private RaycastHit hit;

        [Header("Targets")]
        [SerializeField] protected GameObject CurrentTarget;

        // Private Variables
        [SerializeField] private float ShootingRange = 0;

        // Hits and Shooting

        private void Update()
        {
            if (CurrentTarget != null)
            {
                Debug.DrawRay(ShootOrigin.transform.position, (CurrentTarget.transform.position - ShootOrigin.transform.position).normalized, Color.red);

                HandleAttackTiming();

                HandleShooting();
            }
            else
            {
                CheckTargets();
            }
        }


        private void CheckTargets()
        {
            Collider[] enemies = Physics.OverlapSphere(ShootOrigin.transform.position, ShootingRange);
            EnemiesInRange = 0;

            foreach (Collider go in enemies)
            {
                if (go.tag == "Enemy")
                {
                    EnemiesInRange += 1;

                    if (CurrentTarget == null)
                    {
                        CurrentTarget = go.gameObject;

                        return;
                    }

                    float Distance1 = Mathf.Pow(Mathf.Sqrt(
                        (CurrentTarget.transform.position.x - ShootOrigin.transform.position.x) +
                        (CurrentTarget.transform.position.z - ShootOrigin.transform.position.z)), 2);

                    float Distance2 = Mathf.Pow(Mathf.Sqrt(
                        (go.transform.transform.position.x - ShootOrigin.transform.position.x) +
                        (go.transform.transform.position.z - ShootOrigin.transform.position.z)), 2);

                    if (Distance2 < Distance1)
                    {
                        CurrentTarget = go.gameObject;
                    }
                }
            }
        }

        private void HandleAttackTiming()
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

        #region Virtual Functions

        // Virtual functions for shooting and special abilities
        protected virtual void PrimairyAttack()
        {
            Debug.Log("Core Primairy");

            CurrentTarget.GetComponent<EnemyUnit>().TakeDamage(AttackDamage);

            CanAttack = false;
            AttackTimer = 0;
        }
        protected virtual void SecondairyAttack()
        {
            Debug.Log("Core Secondairy");

            CanUseSpecial = false;
        }

        protected virtual void HandleShooting()
        {
            if (CanAttack)
            {
                PrimairyAttack();
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
            UpgradedDamage = (DamageAddedPerLevel * DamageLevel);
            UpgradedFireRate = (FireRateLevel * FireRateAddedPerLevel);
        }

        #endregion
    }
}