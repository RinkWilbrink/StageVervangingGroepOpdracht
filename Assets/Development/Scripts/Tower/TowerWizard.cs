using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor.UIElements;
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
                default:
                    break;
            }
            
        }

        IEnumerator LightningAttack()
        {
            GameObject go = Instantiate(lightningStrike, transform.position, Quaternion.identity);

            Vector3 newPos = CurrentTarget.transform.position;

            for (float i = 0; i < 1f; i += Time.deltaTime)
            {
                if(i < LightningBetweenTime)
                {
                    go.transform.position = (transform.position + newPos) / 2;
                }
                else if (i >= LightningBetweenTime && i < LightningFinishTime)
                {
                    go.transform.position = newPos;
                    //DoDamage(newPos);

                    Collider[] cool = Physics.OverlapSphere(newPos, LightningRadius);

                    for (int x = 0; x < cool.Length; x++)
                    {
                        if (cool[x].GetComponent<EnemyUnit>())
                        {
                            //cool[x].GetComponent<EnemyUnit>().TakeDamage(SpecialDamage);
                        }
                    }

                    base.SecondairyAttack();
                }
                else if (i >= LightningFinishTime)
                {
                    go.SetActive(false);
                    i = 5f;
                }

                yield return null;
            }
        }

        private void FrostAttack()
        {
            Vector3 newPos = CurrentTarget.transform.position;

            Collider[] cool = Physics.OverlapSphere(newPos, FrostRadius);

            for (int i = 0; i < cool.Length; i++)
            {
                if (cool[i].GetComponent<EnemyUnit>())
                {
                    cool[i].GetComponent<EnemyUnit>().SlowDown(100, SlowDownTime);
                }
            }
        }

        protected override void HandleShooting()
        {
            
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