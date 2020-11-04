using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButtonManager : MonoBehaviour
{
    // Variables
    [SerializeField] private Button[] buttons;
    [SerializeField] private Button SpecialAbilityButton;

    [Header("Scriptable Object")]
    [SerializeField] private ItemCost itemCosts;

    private int[] BuildingCosts;
    private int[] AbilityManaCost;

    private void Awake()
    {
        BuildingCosts = new int[5];

        BuildingCosts[0] = itemCosts.ArcherTowerCost;
        BuildingCosts[1] = itemCosts.WizardTowerCost;
        BuildingCosts[2] = itemCosts.CannonTowerCost;
        BuildingCosts[3] = itemCosts.GoldMineCost;
        BuildingCosts[4] = itemCosts.ManaWellCost;

        AbilityManaCost = new int[3];

        AbilityManaCost[0] = itemCosts.ArcherSpecialCost;
        AbilityManaCost[1] = itemCosts.WizardSpecialCost;
        AbilityManaCost[2] = itemCosts.CannonSpecialCost;

        UpdateTowerButtonUI();
    }

    public void UpdateTowerButtonUI()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(BuildingCosts[i] <= GameController.Gold)
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
            }
        }
    }
}