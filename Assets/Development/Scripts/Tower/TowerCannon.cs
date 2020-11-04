using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class TowerCannon : TowerCore
    {
        // Variables
        [Header("Big Bomb")]
        [SerializeField] private int ExplosionDamage;
        [SerializeField] private float ExplosionRadius;
        [SerializeField] private GameObject ExplosionPrefab;

        [Header("Fire Bomb")]
        [SerializeField] private int DamagePerSecond;
        [SerializeField] private float FireRadius;
        [SerializeField] private float FireTime;

        protected override void HandleShooting()
        {
            base.HandleShooting();
        }

        protected override void PrimaryAttack()
        {
            base.PrimaryAttack();
        }

        protected override void SecondaryAttack()
        {
            switch(SpecialUnlocked)
            {
                case SpecialAttack.Special1:
                    StartCoroutine(BigBomb());
                    break;
                case SpecialAttack.Special2:
                    StartCoroutine(FireBomb());
                    break;
            }

            base.SecondaryAttack();
        }

        public override void LookAt()
        {
            base.LookAt();
        }

        #region Special Attacks

        private IEnumerator BigBomb()
        {
            Collider[] EnemiesInRange = Physics.OverlapSphere(CurrentTarget.transform.position, ExplosionRadius, 1 << 9);
            //GameObject go = Instantiate(ExplosionPrefab, CurrentTarget.transform);

            for (int i = 0; i < EnemiesInRange.Length; i++)
            {
                EnemiesInRange[i].GetComponent<EnemyUnit>().TakeDamage(ExplosionDamage);

                yield return null;
            }

            SpecialAttackMode = false;
        }

        private IEnumerator FireBomb()
        {
            float timer = 0f;

            while(timer < FireTime)
            {
                if(CurrentTarget != null)
                {
                    Collider[] EnemiesInRange = Physics.OverlapSphere(CurrentTarget.transform.position, ExplosionRadius, 1 << 9);
                    for (int i = 0; i < EnemiesInRange.Length; i++)
                    {
                        EnemiesInRange[i].GetComponent<EnemyUnit>().TakeDamage(ExplosionDamage);

                        //yield return null;
                    }
                    timer += GameTime.deltaTime;
                }

                yield return new WaitForSeconds(1f);
            }

            SpecialAttackMode = false;
        }

        #endregion
    }
}