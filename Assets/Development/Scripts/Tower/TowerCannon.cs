using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    enum SpecialAttack
    {
        Special1 = 0, Special2 = 1, Special3 = 2
    }

    public class TowerCannon : TowerCore
    {
        // Variables
        SpecialAttack SpecialUnlocked;

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
                case SpecialAttack.Special3:

                    break;
            }
        }
    }
}