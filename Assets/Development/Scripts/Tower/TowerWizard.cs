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

        protected override void PrimairyAttack()
        {
            base.PrimairyAttack();
            Debug.Log("Wizard Primairy");
        }

        protected override void SecondairyAttack()
        {
            switch (SpecialUnlocked)
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

            //go.transform.position = newPos;

            Collider[] ChainLightning = new Collider[LightningChainLimit];

            ChainLightning[0] = CurrentTarget.GetComponent<Collider>();

            for(int x = 0; x < ChainLightning.Length; x++)
            {
                Collider[] EnemiesInRange = Physics.OverlapSphere(ChainLightning[x].transform.position, LightningRadius, 1 << 9);
                if(EnemiesInRange.Length > 0)
                {
                    float B = float.MaxValue;

                    for(int y = 0; y < EnemiesInRange.Length; y++)
                    {
                        Debug.LogFormat("x ={1} | y = {0}", y, x);
                        if(EnemiesInRange[y] != null)
                        {
                            float distance = Mathf.Pow(Mathf.Sqrt(
                                (ChainLightning[x].transform.position.x - EnemiesInRange[y].transform.position.x) +
                                (ChainLightning[x].transform.position.z - EnemiesInRange[y].transform.position.z)), 2f);

                            if(distance > 0 && distance < B)
                            {
                                B = distance;

                                ChainLightning[x + 1] = EnemiesInRange[y];
                            }
                        }
                    }

                    Debug.Log(x);

                    if(ChainLightning[x] != null)
                    {
                        ChainLightning[x].GetComponent<EnemyUnit>().TakeDamage(SpecialDamage);

                        GameObject go = new GameObject();

                        go.transform.position = ChainLightning[x].transform.position;
                        go.name = string.Format("name{0}", x);
                    }
                }
                else
                {
                    x = ChainLightning.Length;
                }

                yield return null;
            }

            /*
            GameObject go = Instantiate(lightningStrike, transform.position, Quaternion.identity);

            Vector3 newPos = CurrentTarget.transform.position;

            for (float time = 1; time < 1f; time += Time.deltaTime)
            {
                if(time < LightningBetweenTime)
                {
                    go.transform.position = (transform.position + newPos) / 2;
                }
                else if (time >= LightningBetweenTime && time < LightningFinishTime)
                {
                    
                }
                else if (time >= LightningFinishTime)
                {
                    go.SetActive(false);
                    time = 5f;
                }

                yield return null;
            }*/
        }

        private void FrostAttack()
        {
            Vector3 newPos = CurrentTarget.transform.position;

            Collider[] cool = Physics.OverlapSphere(newPos, FrostRadius);

            for (int i = 0; i < cool.Length; i++)
            {
                if (cool[i].GetComponent<EnemyUnit>())
                {
                    cool[i].GetComponent<EnemyUnit>().SlowDown(0.2f, SlowDownTime);
                }
            }
        }

        protected override void HandleShooting()
        {
            //base.HandleShooting();
        }

        public void ShowSpecialAttackUI()
        {
            if (!GameObject.Find(LightningUI.name))
            {
                LightningUI.SetActive(true);
            }

            LightningUI.transform.LookAt(CurrentTarget.transform.position);
        }
    }
}