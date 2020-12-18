using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    [SelectionBase]
    public class TowerArcher : TowerCore
    {
        // Variables
        [Header("Ballista")]
        [SerializeField] private int BallistaDamage;

        [Header("Poison Darts")]
        [SerializeField] private int InitialHitDamage;
        [SerializeField] private int PoisonDamage;
        [SerializeField] private float PoisonCloudRange;
        [SerializeField] private int PoisonTimeInSeconds;

        [Header("Prefabs")]
        [SerializeField] private GameObject ballistaShot;
        [Space(6)]
        [SerializeField] private GameObject PoisonCloudSprite;
        [SerializeField] private GameObject PoisonsBombPrefab;
        [Space(6)]
        [SerializeField] private AudioClip ArrowAudioSFX;
        [SerializeField] private AudioClip BlowdartAudioSFX;
        [SerializeField] private AudioClip BallistaAudioSFX;
        [Space(3)]
        [SerializeField] private AudioClip BallistaSpecialAudioSFX;
        [SerializeField] private AudioClip PoisonCloudAudioSFX;

        public override void Init()
        {
            base.Init();
            PoisonCloudSprite.SetActive(false);
        }

        protected override void PrimaryAttack()
        {
            base.PrimaryAttack();


            if (SpecialUnlocked == SpecialAttack.Special2)
            {
                CurrentTarget.GetComponent<EnemyUnit>().PoisonDOT(PoisonDamage, PoisonTimeInSeconds);
                FindObjectOfType<AudioManagement>().PlayAudioClip(BlowdartAudioSFX, AudioMixerGroups.SFX);
            } else if ( SpecialUnlocked != SpecialAttack.Special2 || SpecialUnlocked != SpecialAttack.Special1 ) {
                FindObjectOfType<AudioManagement>().PlayAudioClip(ArrowAudioSFX, AudioMixerGroups.SFX);
            } else if ( SpecialUnlocked == SpecialAttack.Special1 ) {
                FindObjectOfType<AudioManagement>().PlayAudioClip(BallistaAudioSFX, AudioMixerGroups.SFX);
            }

            Debug.Log("Archer Primairy");
        }

        protected override void SecondaryAttack()
        {
            switch (SpecialUnlocked)
            {
                case SpecialAttack.Special1:
                    StartCoroutine(BallistaBolts());
                    break;
                case SpecialAttack.Special2:
                    StartCoroutine(PoisonCloud());
                    break;
            }

            base.SecondaryAttack();
        }

        protected override void HandleShooting()
        {
            base.HandleShooting();
        }

        /// <summary>Set where the Special Attack arrow points towards, the function overrides the base function in TowerCore.cs</summary>
        public override void SpecialAttackDirectionLookAt()
        {
            //base.LookAt();
        }

        #region Ballista Shot

        private IEnumerator BallistaBolts()
        {
            FindObjectOfType<AudioManagement>().PlayAudioClip(BallistaSpecialAudioSFX, AudioMixerGroups.SFX);

            StartCoroutine(BallistaShot(ShootOrigin.transform.position, Vector3.forward, 10f));
            StartCoroutine(BallistaShot(ShootOrigin.transform.position, Vector3.back, 10f));
            StartCoroutine(BallistaShot(ShootOrigin.transform.position, Vector3.left, 10f));
            StartCoroutine(BallistaShot(ShootOrigin.transform.position, Vector3.right, 10f));

            yield return null;

            SpecialAttackMode = false;
        }

        private IEnumerator BallistaShot(Vector3 origin, Vector3 direction, float distance)
        {
            byte index = 0;
            RaycastHit hit;
            List<Collider> list = new List<Collider>();

            while (index < 5)
            {
                if (Physics.Raycast(origin, direction, out hit, distance, 1 << 9))
                {
                    if (hit.point == null)
                    {
                        index = 5;
                    }
                    else
                    {
                        list.Add(hit.collider);
                        hit.collider.enabled = false;
                    }
                }

                index += 1;
            }

            GameObject go = Instantiate(ballistaShot, origin, Quaternion.LookRotation(direction));
            Vector3 newPos = origin += direction * distance;

            while(Vector3.Distance(go.transform.position, newPos) > 0.1f)
            {
                go.transform.position = Vector3.Lerp(go.transform.position, newPos, 6f * Time.deltaTime);

                yield return null;
            }

            //Destroy the ballistaShot Arrow when the arrow has reached the max distance it will shoot at
            Destroy(go);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].enabled = true;

                list[i].gameObject.GetComponent<EnemyUnit>().TakeDamage(BallistaDamage);
            }

            // Clean up local variables
            list = null;
        }

        #endregion

        private IEnumerator PoisonCloud()
        {
            float timer = 0f;

            PoisonCloudSprite.SetActive(true);

            FindObjectOfType<AudioManagement>().PlayAudioClip(PoisonCloudAudioSFX, AudioMixerGroups.SFX);

            while (timer < PoisonTimeInSeconds)
            {

                Collider[] EnemiesInRange = Physics.OverlapSphere(ShootOrigin.transform.position, PoisonCloudRange, 1 << 9);

                for (int i = 0; i < EnemiesInRange.Length; i++)
                {
                    EnemiesInRange[i].GetComponent<EnemyUnit>().PoisonDOT(PoisonDamage, PoisonTimeInSeconds);
                    EnemiesInRange[i].GetComponent<EnemyUnit>().TakeDamage(PoisonDamage);
                }

                timer += 1f;
                yield return new WaitForSeconds(1f);
            }

            PoisonCloudSprite.SetActive(false);
            SpecialAttackMode = false;
        }

    }
}