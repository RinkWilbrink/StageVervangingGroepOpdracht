using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTower : TowerCore
{
    // Variables

    public override void PrimairyAttack(int _damage, int _attackTime)
    {
        Debug.Log("Wizard Primairy");
    }

    public override void SecondairyAttack(int _damage, int _attackTime)
    {
        Debug.Log("Wizard Secondairy");
    }

    public override void HandleAttackTiming()
    {
        Debug.Log("Wizard Banaan");
    }
}
