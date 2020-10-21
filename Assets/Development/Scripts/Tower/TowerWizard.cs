using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

namespace Tower
{
    [SelectionBase]
    public class TowerWizard : TowerCore
    {
        [Header("")]
        // Variables
        [SerializeField] private GameObject lightningStrike;

        [Header("Special Attack Timing")]
        //[SerializeField] private float LightningTime;
        [SerializeField] private float LightningBetweenTime;
        [SerializeField] private float LightningFinishTime;

        protected override void PrimairyAttack()
        {
            base.PrimairyAttack();
            Debug.Log("Wizard Primairy");
        }

        protected override void SecondairyAttack()
        {
            StartCoroutine(LightningAttack());
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
                    DoDamage(newPos);
                }
                else if (i >= LightningFinishTime)
                {
                    go.SetActive(false);
                    i = 5f;
                }

                yield return null;
            }
        }

        private void DoDamage(Vector3 newPos)
        {
            Collider[] cool = Physics.OverlapSphere(newPos, 1f);

            for (int i = 0; i < cool.Length; i++)
            {
                if (cool[i].GetComponent<EnemyUnit>())
                {
                    cool[i].GetComponent<EnemyUnit>().TakeDamage(SpecialDamage);
                }
            }

            base.SecondairyAttack();
        }

        private void OnDrawGizmos()
        {
            if (base.CurrentTarget != null)
            {
                Gizmos.DrawWireSphere(base.CurrentTarget.transform.position, 1f);
            }
        }

        protected override void HandleShooting()
        {
            //base.HandleShooting();

            if (Input.GetKeyDown(KeyCode.P))
            {
                SecondairyAttack();
            }

            //PrimairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), Mathf.CeilToInt(AttackDamage + base.UpgradedDamage), base.AttackShootingTime);
            //SecondairyAttack(CurrentTarget.GetComponent<EnemyUnit>(), Mathf.CeilToInt(SpecialDamage + base.UpgradedDamage), base.SpecialShootingTime);
        }
    }
}