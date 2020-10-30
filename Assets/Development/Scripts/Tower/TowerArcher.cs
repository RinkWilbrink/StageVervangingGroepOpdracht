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
            base.SecondaryAttack();

            Debug.Log("Archer Secondairy");
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