using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tower
{
    public class TowerCore : MonoBehaviour
    {
        // Variables
        [Header("Stats")]
        [SerializeField] public int AttackShootingTime;
        [SerializeField] public int AttackDamage;

        // Hidden Primairy Attack Variables
        [HideInInspector] public float AttackTimer;
        [HideInInspector] public bool CanAttack = true;

        [Space(6)]

        [SerializeField] public int SpecialShootingTime;
        [SerializeField] public int SpecialDamage;

        [Space(2)]
        [SerializeField] public int SpecialAttackThresshold;

        // Hidden Secondairy Attack Variables
        [HideInInspector] public float SpecialTimer;
        [HideInInspector] public bool CanUseSpecial = false;

        [Header("Shooting and Range")]
        [SerializeField] private GameObject ShootOrigin;
        [SerializeField] public int EnemiesInRange = 0;

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
                HandleAttackTiming();
            }
            else
            {
                CheckTargets();
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                PrimairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), AttackDamage, AttackShootingTime);
            }
            if(Input.GetKeyDown(KeyCode.O))
            {
                SecondairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), SpecialDamage, SpecialShootingTime);
            }
        }

        public virtual void HandleShooting()
        {
            Vector3 direction = (CurrentTarget.transform.position - ShootOrigin.transform.position).normalized;

            Physics.Raycast(ShootOrigin.transform.position, direction, out hit, 100f);

            Debug.DrawRay(ShootOrigin.transform.position, direction, Color.red, 1f);

            if(CanAttack)
            {
                if(CanUseSpecial)
                {
                    if(EnemiesInRange > SpecialAttackThresshold)
                    {
                        //The Attack

                        SecondairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), SpecialDamage, SpecialShootingTime);

                        return;
                    }
                }

                PrimairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), AttackDamage, AttackShootingTime);
            }
        }

        private void CheckTargets()
        {
            Collider[] enemies = Physics.OverlapSphere(ShootOrigin.transform.position, ShootingRange);
            EnemiesInRange = enemies.Length;
            foreach(Collider go in enemies)
            {
                if(go.tag == "Enemy")
                {
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
            if(AttackTimer >= AttackShootingTime)
            {
                CanAttack = true;
                AttackTimer += Time.deltaTime;
            }
            else
            {
                AttackTimer += Time.deltaTime;
            }
            if(SpecialTimer >= SpecialShootingTime)
            {
                CanUseSpecial = true;
                SpecialTimer = 0;
            }
            else
            {
                SpecialTimer += Time.deltaTime;
            }
        }

        // Virtual functions for shooting and special abilities
        public virtual void PrimairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            Debug.Log("Core Primairy");

            _target.TakeDamage(_damage);
        }
        public virtual void SecondairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            Debug.Log("Core Secondairy");

            _target.TakeDamage(_damage);
        }
    }
}