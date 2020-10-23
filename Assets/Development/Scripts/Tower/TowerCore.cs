using System.Linq;
using UnityEngine;

namespace Tower
{
    public enum SpecialAttack
    {
        Special1 = 0, Special2 = 1
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
        [SerializeField] protected int SpecialAttackThresshold;

        // Hidden Primairy Attack Variables
        [HideInInspector] protected float AttackTimer;
        [HideInInspector] protected bool CanAttack = true;

        [Header("Damage and Firerate Upgrades")]
        [SerializeField] private float DamageAddedPerLevel;
        [SerializeField] private float FireRateAddedPerLevel;
        [Space(2)]
        [SerializeField] public int TowerLevelToUnlockSpecial;
        [SerializeField] public int TowerLevel = 1;
        protected float UpgradedDamage;
        protected float UpgradedFireRate;

        protected SpecialAttack SpecialUnlocked;

        [Header("Shooting and Range")]
        [SerializeField] private GameObject ShootOrigin;
        [HideInInspector] private RaycastHit hit;

        [Header("Targets")]
        [SerializeField] protected GameObject CurrentTarget;

        // Private Variables
        [SerializeField] private float ShootingRange = 0;

        // Hits and Shooting

        private void Update()
        {
            //if (CurrentTarget != null)
            //{
            //    Debug.DrawRay(ShootOrigin.transform.position, (CurrentTarget.transform.position - ShootOrigin.transform.position).normalized, Color.red);
            //
            //    HandleAttackTiming();
            //
            //    HandleShooting();
            //}
            //else
            //{
            //    CheckTargets();
            //}

            CheckTargets();

            //HandleAttackTiming();
            //HandleShooting();

            if (Input.GetKeyDown(KeyCode.O))
            {
                PrimairyAttack();
                Debug.Log("PrimairyAttack");
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                SecondairyAttack();
                Debug.Log("SecondairyAttack");
            }
        }

        private void CheckTargets()
        {
            Collider[] enemies = Physics.OverlapSphere(ShootOrigin.transform.position, ShootingRange, 1 << 9);

            Debug.LogFormat("Lenght = {0}", enemies.Length);

            for (int i = 0; i < enemies.Length; i++)
            {
                Debug.Log(enemies[i].name);
            }

            if(enemies.Length == 0)
            {
                CurrentTarget = null;

                Debug.Log("A");
            }
            else
            {
                foreach (Collider go in enemies)
                {
                    if (go.tag == "Enemy")
                    {
                        float Distance1 = ShootingRange + 1f;

                        if (CurrentTarget != null)
                        {
                            Distance1 = Mathf.Sqrt(
                            Mathf.Pow((CurrentTarget.transform.position.x - ShootOrigin.transform.position.x), 2f) +
                            Mathf.Pow((CurrentTarget.transform.position.z - ShootOrigin.transform.position.z), 2f));
                        }

                        float Distance2 = Mathf.Sqrt(
                            Mathf.Pow((go.transform.transform.position.x - ShootOrigin.transform.position.x), 2f) +
                            Mathf.Pow((go.transform.transform.position.z - ShootOrigin.transform.position.z), 2f));

                        if (Distance2 < Distance1)
                        {
                            CurrentTarget = go.gameObject;
                        }
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
            Debug.Log("Core Secondairy");;
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
            UpgradedDamage = (DamageAddedPerLevel * (TowerLevel - 1));
            UpgradedFireRate = (FireRateAddedPerLevel * (TowerLevel - 1));
        }

        public void StartSecondairyAttack()
        {
            SecondairyAttack();
        }

        #endregion
    }
}