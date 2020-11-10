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

        [Header("Fire Bomb")]
        [SerializeField] private int DamagePerSecond;
        [SerializeField] private float FireRadius;
        [SerializeField] private float FireTime;

        [Header("Prefabs")]
        [SerializeField] private GameObject BigBombPrefab;
        [SerializeField] private GameObject FireBombPrefab;

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
            GameObject go = Instantiate(BigBombPrefab, ShootOrigin.transform.position, BigBombPrefab.transform.rotation);

            while(Vector3.Distance(go.transform.position, CurrentTarget.transform.position) > 0.05f)
            {
                go.transform.position = Vector3.Lerp(go.transform.position, CurrentTarget.transform.position, 2f * GameTime.deltaTime);

                yield return null;
            }

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