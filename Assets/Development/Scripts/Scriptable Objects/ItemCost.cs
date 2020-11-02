using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCosts", menuName = "ItemCosts")]
public class ItemCost : ScriptableObject
{
    // Variables
    [SerializeField] public int ArcherTowerCost;
    [SerializeField] public int WizardTowerCost;

    [Space(6)]

    [SerializeField] public int GoldMineCost;
    [SerializeField] public int ManaWellCost;

    [Header("Mana Costs")]
    [SerializeField] public int WizardSpecialCost;
    [SerializeField] public int ArcherSpecialCost;
}
