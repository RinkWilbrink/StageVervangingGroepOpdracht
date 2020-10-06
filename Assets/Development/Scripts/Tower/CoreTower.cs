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

        [Header("Targets")]
        [SerializeField] public GameObject CurrentTarget;

        void Update()
        {
            if(CurrentTarget != null)
            {
                HandleAttackTiming();
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                PrimairyAttack(PrimairyDamage, PrimairyShootingTime);
            }
            if(Input.GetKeyDown(KeyCode.O))
            {
                SecondairyAttack(SecondairyDamage, SecondairyShootingTime);
            }
        }

        private void HitManager()
        {
            RaycastHit hit;
            Vector3 direction = (CurrentTarget.transform.position - ShootOrigin.transform.position).normalized;

            Physics.Raycast(ShootOrigin.transform.position, direction, out hit, 100f);

            Debug.Log(hit.collider.name);

            Debug.DrawRay(ShootOrigin.transform.position, direction, Color.red, 1f);
        }

        private void TargetManager()
        {
            //CurrentTarget = target;
        }

        public virtual void HandleAttackTiming()
        {
            Debug.Log("banaan");

            //if()
    }

        // Virtual functions for shooting and special abilities
        public virtual void PrimairyAttack(int _damage, int _attackTime)
        { Debug.Log("Core Primairy"); }
        public virtual void SecondairyAttack(int _damage, int _attackTime)
        { Debug.Log("Core Secondairy"); }
    }
}