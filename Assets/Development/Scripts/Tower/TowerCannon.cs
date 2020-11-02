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

        protected override void PrimairyAttack()
        {
            base.PrimairyAttack();
        }

        protected override void SecondairyAttack()
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