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

                    break;
                case SpecialAttack.Special2:

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
    }
}