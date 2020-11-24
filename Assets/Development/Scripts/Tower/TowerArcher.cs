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

        public override void LookAt()
        {
            //base.LookAt();
        }

        #region Ballista Shot

        private IEnumerator BallistaBolts()
        {
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