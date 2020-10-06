using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : TowerCore
{
    // Variables
    [Header("Archer Tower Stats")]
    [SerializeField] private int banaan;

    public override void PrimairyAttack(int _damage, int _attackTime)
    {
        Debug.Log("Archer Primairy");
    }

    public override void SecondairyAttack(int _damage, int _attackTime)
    {
        Debug.Log("Archer Secondairy");
    }
}
