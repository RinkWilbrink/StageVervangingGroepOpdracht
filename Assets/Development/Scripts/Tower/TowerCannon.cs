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
                    BigBomb();
                    break;
                case SpecialAttack.Special2:
                    FireBomb();
                    break;
            }

            base.SecondaryAttack();
        }

        public override void LookAt()
        {
            base.LookAt();
        }

        #region Special Attacks

        private void BigBomb()
        {

        }

        private void FireBomb()
        {

        }

        #endregion
    }
}