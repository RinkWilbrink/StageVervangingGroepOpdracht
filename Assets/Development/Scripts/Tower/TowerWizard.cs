using System.Collections;
using UnityEngine;

namespace Tower
{
    [SelectionBase]
    public class TowerWizard : TowerCore
    {
        [Header("Lightning Strike Attack")]
        // Variables
        [SerializeField] private int LightningDamage;
        [SerializeField] private int LightningRadius;
        [SerializeField] private int LightningChainLimit;
        [SerializeField] private GameObject LightningUI;
        [SerializeField] private float LightningInBetweenTime;

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
            switch (SpecialUnlocked)
            {
                case SpecialAttack.Special1:
                    StartCoroutine(LightningAttack());
                    break;
                case SpecialAttack.Special2:
                    FrostAttack();
                    break;
            }

            base.SecondaryAttack();
        }

        private IEnumerator LightningAttack()
        {
            Collider collider = CurrentTarget.GetComponent<Collider>();
            Collider nextCollider = null;
            int LightningChainCount = 0;

            while (LightningChainCount < LightningChainLimit)
            {
                if (LightningChainCount < LightningChainLimit)
                {
                    if (LightningChainCount > 0)
                    {
                        collider = nextCollider;
                    }

                    Collider[] EnemiesInRange = Physics.OverlapSphere(collider.transform.position, LightningRadius, 1 << 9);

                    if (EnemiesInRange.Length > 1)
                    {
                        float B = float.MaxValue;

                        for (int y = 0; y < EnemiesInRange.Length; y++)
                        {
                            if (EnemiesInRange[y] != null)
                            {
                                float NewPotentialTargetDistance = Mathf.Sqrt(
                                    Mathf.Pow(collider.transform.position.x - EnemiesInRange[y].transform.position.x, 2f) +
                                    Mathf.Pow(collider.transform.position.z - EnemiesInRange[y].transform.position.z, 2f));

                                if (NewPotentialTargetDistance > 0 && NewPotentialTargetDistance < B)
                                {
                                    B = NewPotentialTargetDistance;

                                    nextCollider = EnemiesInRange[y];
                                }
                            }
                        }
                    }
                    else
                    {
                        LightningChainCount = LightningChainLimit + 1;
                        collider.GetComponent<EnemyUnit>().TakeDamage(LightningDamage);

                        yield return null;
                    }
                }

                if (collider != null)
                {
                    collider.GetComponent<EnemyUnit>().TakeDamage(LightningDamage);
                }

                LightningChainCount++;
                yield return new WaitForSecondsRealtime(LightningInBetweenTime);
            }

            SpecialAttackMode = false;
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

            SpecialAttackMode = false;
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