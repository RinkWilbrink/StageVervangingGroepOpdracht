using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class TowerCore : MonoBehaviour
    {
        // Variables
        [Header("Stats")]
        [SerializeField] private int PrimairyShootingTime;
        private float PrimairyTimer;
        [SerializeField] private int PrimairyDamage;

        [Space(10)]

        [SerializeField] private int SecondairyShootingTime;
        private float SecondairyTimer;
        [SerializeField] private int SecondairyDamage;

        [Header("a")]
        [SerializeField] private GameObject ShootOrigin;
        [SerializeField] private float ShootingRange = 0;

        [Header("Targets")]
        [SerializeField] public GameObject CurrentTarget;

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
                PrimairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), PrimairyDamage, PrimairyShootingTime);
            }
            if(Input.GetKeyDown(KeyCode.O))
            {
                SecondairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), SecondairyDamage, SecondairyShootingTime);
            }
        }

        private void HitManager()
        {
            
        }

        private void CheckTargets()
        {
            Collider[] enemies = Physics.OverlapSphere(ShootOrigin.transform.position, ShootingRange);
            foreach(Collider go in enemies)
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

        public virtual void HandleAttackTiming()
        {
            RaycastHit hit;
            Vector3 direction = (CurrentTarget.transform.position - ShootOrigin.transform.position).normalized;

            Physics.Raycast(ShootOrigin.transform.position, direction, out hit, 100f);

            Debug.Log(hit.collider.name);

            Debug.DrawRay(ShootOrigin.transform.position, direction, Color.red, 1f);

            float Health = hit.collider.gameObject.GetComponent<EnemyUnit>().Health;
        }

        // Virtual functions for shooting and special abilities
        public virtual void PrimairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            Debug.Log("Core Primairy");
        }
        public virtual void SecondairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            Debug.Log("Core Secondairy");
        }
    }
}