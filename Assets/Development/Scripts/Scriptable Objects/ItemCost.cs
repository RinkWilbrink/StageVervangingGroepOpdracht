using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingCosts", menuName = "ItemCosts")]
public class ItemCost : ScriptableObject
{
    // Variables
    [SerializeField] public int ArcherTowerCost;
    [SerializeField] public int WizardTowerCost;

    [Space(6)]

    [SerializeField] public int GoldMineCost;
    [SerializeField] public int ManaWellCost;
}
