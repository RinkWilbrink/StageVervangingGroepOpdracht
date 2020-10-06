using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : TowerCore
{
    // Variables

    public override void PrimairyAttack()
    {
        Debug.Log("Archer Primairy");
    }

    public override void SecondairyAttack()
    {
        Debug.Log("Archer Secondairy");
    }
}
