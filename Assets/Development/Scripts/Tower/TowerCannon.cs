using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class TowerCannon : TowerCore
    {
        // Variables

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
            //base.SecondairyAttack();

            switch (SpecialUnlocked)
            {
                case SpecialAttack.Special1:

                    break;
                case SpecialAttack.Special2:

                    break;
            }
        }

        public override void LookAt()
        {
            base.LookAt();
        }
    }
}