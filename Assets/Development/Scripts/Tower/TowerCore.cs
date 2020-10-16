using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tower
{
    public class TowerCore : MonoBehaviour
    {
        // Variables
        [Header("Stats")]
        [SerializeField] public int AttackShootingTime;
        [SerializeField] public int AttackDamage;

        [Space(4)]

        [SerializeField] public int SpecialShootingTime;
        [SerializeField] public int SpecialDamage;
        [SerializeField] public int SpecialAttackThresshold;

        // Hidden Primairy Attack Variables
        [HideInInspector] public float AttackTimer;
        [HideInInspector] public bool CanAttack = true;

        [Header("Damage and Firerate Upgrades")]
        [SerializeField] public float DamageAddedPerLevel;
        [SerializeField] public float FireRateAddedPerLevel;
        [SerializeField] public int DamageLevel = 0;
        [SerializeField] public int FireRateLevel = 0;
        private float UpgradedDamage;
        private float UpgradedFireRate;

        // Hidden Secondairy Attack Variables
        [HideInInspector] public float SpecialTimer;
        [HideInInspector] public bool CanUseSpecial = false;

        [Header("Shooting and Range")]
        [SerializeField] private GameObject ShootOrigin;

        [HideInInspector] public int EnemiesInRange = 0;
        [HideInInspector] public RaycastHit hit;

        [Header("Targets")]
        [SerializeField] public GameObject CurrentTarget;

        // Private Variables
        [SerializeField] private float ShootingRange = 0;

        // Hits and Shooting

        void Update()
        {
            if(CurrentTarget != null)
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

            foreach(Collider go in enemies)
            {
                if(go.tag == "Enemy")
                {
                    EnemiesInRange += 1;

                    if(CurrentTarget == null)
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

                    if(Distance2 < Distance1)
                    {
                        CurrentTarget = go.gameObject;
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
            if(SpecialTimer >= (SpecialShootingTime - UpgradedFireRate))
            {
                CanUseSpecial = true;
            }
            else
            {
                SpecialTimer += Time.deltaTime;
            }
        }

        #region Virtual Functions

        // Virtual functions for shooting and special abilities
        public virtual void PrimairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            Debug.Log("Core Primairy");

            _target.TakeDamage(_damage);

            CanAttack = false;
            AttackTimer = 0;
        }
        public virtual void SecondairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            Debug.Log("Core Secondairy");

            _target.TakeDamage(_damage);

            CanUseSpecial = false;
            SpecialTimer = 0;
        }
        public virtual void HandleShooting()
        {
            if(CanAttack)
            {
                if(CanUseSpecial)
                {
                    if(EnemiesInRange > SpecialAttackThresshold)
                    {
                        //The Attack

                        SecondairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), Mathf.CeilToInt(SpecialDamage + UpgradedDamage), SpecialShootingTime);

                        return;
                    }
                }

                PrimairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), Mathf.CeilToInt(AttackDamage + UpgradedDamage), AttackShootingTime);
            }
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