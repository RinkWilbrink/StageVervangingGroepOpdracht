using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace Tower
{
    [SelectionBase]
    public class TowerWizard : TowerCore
    {
        [Header("Lightning Strike Attack")]
        // Variables
        [SerializeField] private int LightningDamage;
        [SerializeField] private GameObject lightningStrike;
        [SerializeField] private int LightningRadius;
        [SerializeField] private int LightningChainLimit;
        [SerializeField] private GameObject LightningUI;

        [Header("Frost Attack")]
        [SerializeField] private int FrostRadius;
        [SerializeField] private float SlowDownTime;

        [Header("Special Attack Timing")]
        [SerializeField] private float LightningBetweenTime;
        [SerializeField] private float LightningFinishTime;

        protected override void PrimaryAttack()
        {
            base.PrimaryAttack();
            Debug.Log("Wizard Primairy");
        }

        protected override void SecondaryAttack()
        {
            switch(SpecialUnlocked)
            {
                case SpecialAttack.Special1:
                    StartCoroutine(LightningAttack());
                    break;
                case SpecialAttack.Special2:
                    FrostAttack();
                    break;
            }

        }

        IEnumerator LightningAttack()
        {
            Vector3 newPos = CurrentTarget.transform.position;
            Collider col = CurrentTarget.GetComponent<Collider>();
            Collider nextCol = null;
            int LightningChainCount = 0;

            while(LightningChainCount < LightningChainLimit)
            {
                if(LightningChainCount < LightningChainLimit)
                {
                    if(LightningChainCount > 0)
                    {
                        col = nextCol;
                    }

                    Collider[] EnemiesInRange = Physics.OverlapSphere(col.transform.position, LightningRadius, 1 << 9);

                    if(EnemiesInRange.Length > 1)
                    {
                        float B = float.MaxValue;

                        for(int y = 0; y < EnemiesInRange.Length; y++)
                        {
                            if(EnemiesInRange[y] != null)
                            {
                                float distance = Mathf.Sqrt(
                                    Mathf.Pow(col.transform.position.x - EnemiesInRange[y].transform.position.x, 2f) +
                                    Mathf.Pow(col.transform.position.z - EnemiesInRange[y].transform.position.z, 2f));

                                if(distance > 0 && distance < B)
                                {
                                    B = distance;

                                    nextCol = EnemiesInRange[y];
                                }
                            }
                        }
                    }
                    else
                    {
                        LightningChainCount = LightningChainLimit + 1;

                        col.GetComponent<EnemyUnit>().TakeDamage(LightningDamage);

                        yield return null;
                    }
                }

                if(col != null)
                {
                    col.GetComponent<EnemyUnit>().TakeDamage(LightningDamage);
                }

                LightningChainCount++;
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }

        private void FrostAttack()
        {
            Vector3 newPos = CurrentTarget.transform.position;

            Collider[] cool = Physics.OverlapSphere(newPos, FrostRadius);

            for(int i = 0; i < cool.Length; i++)
            {
                if(cool[i].GetComponent<EnemyUnit>())
                {
                    cool[i].GetComponent<EnemyUnit>().SlowDown(0.2f, SlowDownTime);
                }
            }
        }

        protected override void HandleShooting()
        {
            base.HandleShooting();
        }

        public override void LookAt()
        {
            base.LookAt();
        }
    }
}