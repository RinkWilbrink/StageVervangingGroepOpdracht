using System.Collections;
using UnityEngine;

/* References
 * 
 * Splines: (For Chain Lightning)
 * https://catlikecoding.com/unity/tutorials/curves-and-splines/
*/

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
        [SerializeField] private float LightningInBetweenTime;

        [Header("Frost Attack")]
        [SerializeField] private int FrostRadius;
        [SerializeField] private float SlowDownAmount;
        [SerializeField] private float SlowDownTime;

        [Header("Prefabs")]
        [SerializeField] private GameObject LightningCloudPrefab;
        [SerializeField] private GameObject LightningBoltPrefab;
        [Space(6)]
        [SerializeField] private GameObject FrostPrefab;
        [Space(6)]
        [SerializeField] private AudioClip MagicSpellAudioSFX;
        [SerializeField] private AudioClip LightningAttackAudioSFX;
        [SerializeField] private AudioClip FrostAttackAudioSFX;
        [Space(3)]
        [SerializeField] private AudioClip FrostSpecialAudioSFX;
        [SerializeField] private AudioClip LightningSpecialAudioSFX;

        private CRSpline spline;

        /// <summary>Override of the Init(Start) function</summary>
        public override void Init()
        {
            base.Init();
        }

        protected override void PrimaryAttack()
        {
            base.PrimaryAttack();

            if ( SpecialUnlocked == SpecialAttack.Special2 ) 
                FindObjectOfType<AudioManagement>().PlayAudioClip(FrostAttackAudioSFX, AudioMixerGroups.SFX);
            else if ( SpecialUnlocked != SpecialAttack.Special2 || SpecialUnlocked != SpecialAttack.Special1 ) 
                FindObjectOfType<AudioManagement>().PlayAudioClip(MagicSpellAudioSFX, AudioMixerGroups.SFX);
            else if ( SpecialUnlocked == SpecialAttack.Special1 ) 
                FindObjectOfType<AudioManagement>().PlayAudioClip(LightningAttackAudioSFX, AudioMixerGroups.SFX);   
        }

        protected override void SecondaryAttack()
        {
            switch(SpecialUnlocked)
            {
                case SpecialAttack.Special1:
                    StartCoroutine(LightningAttack());
                    break;
                case SpecialAttack.Special2:
                    StartCoroutine(FrostAttack());
                    break;
            }

            base.SecondaryAttack();
        }

        private IEnumerator LightningAttack()
        {
            Collider collider = CurrentTarget.GetComponent<Collider>();
            Collider nextCollider = null;
            int LightningChainCount = 0;
            
            FindObjectOfType<AudioManagement>().PlayAudioClip(LightningSpecialAudioSFX, AudioMixerGroups.SFX);

            yield return new WaitForSeconds(AttackDelayTime);

            while(LightningChainCount < LightningChainLimit)
            {
                if(LightningChainCount < LightningChainLimit)
                {
                    if(LightningChainCount > 0)
                    {
                        collider = nextCollider;
                    }

                    Collider[] EnemiesInRange = Physics.OverlapSphere(collider.transform.position, LightningRadius, 1 << 9);

                    if(EnemiesInRange.Length > 1)
                    {
                        float B = float.MaxValue;

                        for(int y = 0; y < EnemiesInRange.Length; y++)
                        {
                            if(EnemiesInRange[y] != null)
                            {
                                // Check the distance of a new potential target, if its lower then the current target, that will be the new Target
                                float distance1 = Mathf.Sqrt((collider.transform.position - EnemiesInRange[y].transform.position).sqrMagnitude);

                                // Compare the distance of the Current target and the new potential target
                                if(distance1 > 0 && distance1 < B)
                                {
                                    B = distance1;

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

                if(collider != null)
                {
                    collider.GetComponent<EnemyUnit>().TakeDamage(LightningDamage);
                }

                LightningChainCount++;
                yield return new WaitForSecondsRealtime(LightningInBetweenTime);
            }

            SpecialAttackMode = false;
        }

        private IEnumerator FrostAttack()
        {
            Vector3 newPos = CurrentTarget.transform.position;

            Collider[] EnemiesWithingFrostRange = Physics.OverlapSphere(newPos, FrostRadius);

            FindObjectOfType<AudioManagement>().PlayAudioClip(FrostSpecialAudioSFX, AudioMixerGroups.SFX);

            yield return new WaitForSeconds(AttackDelayTime);

            for(int i = 0; i < EnemiesWithingFrostRange.Length; i++)
            {
                if(EnemiesWithingFrostRange[i].GetComponent<EnemyUnit>())
                {
                    EnemiesWithingFrostRange[i].GetComponent<EnemyUnit>().SlowDown(SlowDownAmount, SlowDownTime);
                    StartCoroutine(EnemiesWithingFrostRange[i].GetComponent<EnemyUnit>().FrostOverlay(SlowDownTime));
                }
            }

            SpecialAttackMode = false;
        }

        /// <summary>Handle the shooting of the Primairy Attack</summary>
        protected override void HandleShooting()
        {
            base.HandleShooting();
        }

        public override void SpecialAttackDirectionLookAt()
        {
            base.SpecialAttackDirectionLookAt();
        }
    }
}