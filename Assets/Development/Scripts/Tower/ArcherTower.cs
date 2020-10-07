using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    [SelectionBase]
    public class ArcherTower : TowerCore
    {
        // Variables

        public override void PrimairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            base.PrimairyAttack(_target, _damage, _attackTime);

            Debug.Log("Archer Primairy");
        }

        public override void SecondairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            Debug.Log("Archer Secondairy");
        }
    }
}