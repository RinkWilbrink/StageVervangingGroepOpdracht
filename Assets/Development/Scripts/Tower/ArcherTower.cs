using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class ArcherTower : TowerCore
    {
        // Variables

        public override void PrimairyAttack(int _damage, int _attackTime)
        {
            Debug.Log("Archer Primairy");
        }

        public override void SecondairyAttack(int _damage, int _attackTime)
        {
            Debug.Log("Archer Secondairy");
        }
    }
}