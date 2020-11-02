using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tower
{
    [SelectionBase]
    public class TowerArcher : TowerCore
    {
        // Variables
        [Header("Ballista")]
        [SerializeField] private int Damage;

        [Header("Poison Darts")]
        [SerializeField] private int InitialHitDamage;
        [SerializeField] private int PoisonDamage;
        [SerializeField] private float PoisonTime;

        protected override void PrimaryAttack()
        {
            base.PrimaryAttack();

            Debug.Log("Archer Primairy");
        }

        protected override void SecondaryAttack()
        {
            switch(SpecialUnlocked)
            {
                case SpecialAttack.Special1:
                    StartCoroutine(BallistaBolts());
                    break;
                case SpecialAttack.Special2:
                    StartCoroutine(PoisonDarts());
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

        private IEnumerator BallistaBolts()
        {
            // if(Physics.RayCast(direction, length))

            yield return null;

            SpecialAttackMode = false;
        }

        private IEnumerator PoisonDarts()
        {
            List<EnemyUnit> poisonedEnemies = new List<EnemyUnit>();
            float timer = 0f;

            while(timer < PoisonTime)
            {
                yield return null;
            }

            // idk

            SpecialAttackMode = false;
        }
    }
}