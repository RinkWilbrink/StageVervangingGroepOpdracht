using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    [SelectionBase]
    public class TowerWizard : TowerCore
    {
        // Variables

        public override void PrimairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            base.PrimairyAttack(_target, _damage, _attackTime);
            Debug.Log("Wizard Primairy");
        }

        public override void SecondairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            base.SecondairyAttack(_target, _damage, _attackTime);

            Debug.Log("Wizard Secondairy");
        }

        public override void HandleShooting()
        {
            base.HandleShooting();
        }
    }
}