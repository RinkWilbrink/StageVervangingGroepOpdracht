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

        public override void HandleShooting()
        {
            base.HandleShooting();
        }

        public override void PrimairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            base.PrimairyAttack(_target, _damage, _attackTime);
        }

        public override void SecondairyAttack(EnemyUnit _target, int _damage, int _attackTime)
        {
            //base.SecondairyAttack(_target, _damage, _attackTime);

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