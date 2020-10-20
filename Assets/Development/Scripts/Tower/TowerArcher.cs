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

        protected override void PrimairyAttack()
        {
            base.PrimairyAttack();

            Debug.Log("Archer Primairy");
        }

        protected override void SecondairyAttack()
        {
            base.SecondairyAttack();

            Debug.Log("Archer Secondairy");
        }

        protected override void HandleShooting()
        {
            base.HandleShooting();
        }
    }
}