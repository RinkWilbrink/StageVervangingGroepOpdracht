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
        [Tooltip("The higher the number, the quicker it will throw the ball")]
        [SerializeField] private float BombThrowSpeed;

        [Header("Fire Bomb")]
        [SerializeField] private int FireDamagePerSecond;
        [SerializeField] private float FireRadius;
        [SerializeField] private float FireTime;

        [Header("Prefabs")]
        [SerializeField] private GameObject BigBombPrefab;
        [SerializeField] private GameObject ExplosionPrefab;
        [Space(6)]
        [SerializeField] private GameObject FireBombPrefab;
        [SerializeField] private GameObject FireEffectPrefab;

        public override void Init()
        {
            base.Init();
        }

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
            
            GameObject go = Instantiate(BigBombPrefab, ShootOrigin.transform.position, BigBombPrefab.transform.rotation);

            Vector3 newPos = CurrentTarget.transform.position;

            while(Vector3.Distance(go.transform.position, newPos) > 0.1f)
            {
                go.transform.position = Vector3.Lerp(go.transform.position, newPos, BombThrowSpeed * Time.deltaTime);

                yield return null;
            }

            go.transform.position = newPos;

            Collider[] EnemiesInRange = Physics.OverlapSphere(newPos, ExplosionRadius, 1 << 9);

            GameObject explosion = Instantiate(ExplosionPrefab, newPos, ExplosionPrefab.transform.rotation);

            Debug.Log("Bautista Bomb!!");

            for (int i = 0; i < EnemiesInRange.Length; i++)
            {
                EnemiesInRange[i].GetComponent<EnemyUnit>().TakeDamage(ExplosionDamage);


                yield return null;
            }

            Destroy(go);

            yield return new WaitForSeconds(2f);

            Destroy(explosion);

            SpecialAttackMode = false;
        }

        private IEnumerator FireBomb()
        {
            float timer = 0f;

            GameObject fireEffect = Instantiate(FireEffectPrefab, new Vector3(ShootOrigin.transform.position.x, 0, ShootOrigin.transform.position.z), Quaternion.LookRotation(CurrentTarget.transform.position));

            while(timer < FireTime)
            {
                if(CurrentTarget != null)
                {
                    Collider[] EnemiesInRange = Physics.OverlapSphere(CurrentTarget.transform.position, FireRadius, 1 << 9);
                    for (int i = 0; i < EnemiesInRange.Length; i++)
                    {
                        EnemiesInRange[i].GetComponent<EnemyUnit>().TakeDamage(FireDamagePerSecond);
                    }
                }
                timer += 1f;
                yield return new WaitForSeconds(1f);
            }

            SpecialAttackMode = false;
        }

        #endregion
    }
}