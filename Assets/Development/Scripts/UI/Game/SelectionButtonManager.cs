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
        BuildingCosts = new int[4];

        BuildingCosts[0] = itemCosts.ArcherTowerCost;
        BuildingCosts[1] = itemCosts.WizardTowerCost;
        BuildingCosts[2] = itemCosts.GoldMineCost;
        BuildingCosts[3] = itemCosts.ManaWellCost;

        AbilityManaCost = new int[2];

        AbilityManaCost[0] = itemCosts.ArcherSpecialCost;
        AbilityManaCost[1] = itemCosts.WizardSpecialCost;

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

        //int b = int.MaxValue;
        //for (int i = 0; i < AbilityManaCost.Length; i++)
        //{
        //    if(AbilityManaCost[i] < b)
        //    {
        //        b = AbilityManaCost[i];
        //    }
        //}
        //
        //SpecialAbilityButton
    }
}