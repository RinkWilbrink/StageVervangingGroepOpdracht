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
        [SerializeField] private GameObject OilBombPrefab;
        [SerializeField] private GameObject OilSpillPrefab;
        [Space(6)]
        [SerializeField] private AudioClip CannonAudioSFX;
        [SerializeField] private AudioClip FireCannonAudioSFX;
        [Space(3)]
        [SerializeField] private AudioClip BigBombSpecialAudioSFX;

        public float vuurtijd = 10f;

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

            if (SpecialUnlocked == SpecialAttack.Special2 )
                FindObjectOfType<AudioManagement>().PlayAudioClip(FireCannonAudioSFX, AudioMixerGroups.SFX);
            else if (SpecialUnlocked != SpecialAttack.Special1 || SpecialUnlocked != SpecialAttack.Special2 ) 
                FindObjectOfType<AudioManagement>().PlayAudioClip(CannonAudioSFX, AudioMixerGroups.SFX);
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

        /// <summary>Set where the Special Attack arrow points towards, the function overrides the base function in TowerCore.cs</summary>
        public override void SpecialAttackDirectionLookAt()
        {
            base.SpecialAttackDirectionLookAt();
        }

        #region Special Attacks

        private IEnumerator BigBomb()
        {
            
            
            if(CurrentTarget != null)
            {
                GameObject BombBullet = Instantiate(BigBombPrefab, ShootOrigin.transform.position, BigBombPrefab.transform.rotation);

                FindObjectOfType<AudioManagement>().PlayAudioClip(BigBombSpecialAudioSFX, AudioMixerGroups.SFX);


                Vector3 newPos = CurrentTarget.transform.position;
                while (Vector3.Distance(BombBullet.transform.position, newPos) > 0.1f)
                {
                    BombBullet.transform.position = Vector3.Lerp(BombBullet.transform.position, newPos, BombThrowSpeed * GameTime.deltaTime);

                    yield return null;
                }

                BombBullet.transform.position = newPos;

                Collider[] EnemiesInRange = Physics.OverlapSphere(newPos, ExplosionRadius, 1 << 9);
                GameObject explosion = Instantiate(ExplosionPrefab, newPos, ExplosionPrefab.transform.rotation);

                for (int i = 0; i < EnemiesInRange.Length; i++)
                {
                    EnemiesInRange[i].GetComponent<EnemyUnit>().TakeDamage(ExplosionDamage, towerType);


                    yield return null;
                }

                Destroy(BombBullet);

                yield return new WaitForSeconds(1.3f);

                Destroy(explosion);
                SpecialAttackMode = false;

            } else
            {
                GameController.Mana += 2;
                SpecialAttackMode = false;
                StopCoroutine(BigBomb());
            }

            
        }

        private IEnumerator FireBomb()
        {
            //als current target != null schiet oil spill op target
            if (CurrentTarget != null)
            {
                float timer = 0f;

                GameObject OilBall = Instantiate(OilBombPrefab, ShootOrigin.transform.position, OilBombPrefab.transform.rotation);

                Vector3 EnemyPosition = CurrentTarget.transform.position;

                while (Vector3.Distance(OilBall.transform.position, EnemyPosition) > 0.1f)
                {
                    OilBall.transform.position = Vector3.Lerp(OilBall.transform.position, EnemyPosition, BombThrowSpeed * GameTime.deltaTime);

                    yield return null;
                }

                OilBall.transform.position = EnemyPosition;

                GameObject OilSpill = Instantiate(OilSpillPrefab, EnemyPosition, OilSpillPrefab.transform.rotation);

                Destroy(OilBall);
                SpecialAttackMode = false;
                yield return new WaitForSeconds(FireTime);

                Destroy(OilSpill);
            } else
            {
                GameController.Mana += 2;
                SpecialAttackMode = false;
                StopCoroutine(FireBomb());
            }

        }

        #endregion

    }
}