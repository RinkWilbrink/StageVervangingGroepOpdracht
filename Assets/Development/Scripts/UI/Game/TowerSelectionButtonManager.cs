using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionButtonManager : MonoBehaviour
{
    // Variables
    [SerializeField] private Button[] buttons;

    [Header("Scriptable Object")]
    [SerializeField] private ItemCost itemCosts;

    private int[] cost;

    private void Awake()
    {
        cost = new int[5];

        cost[0] = itemCosts.ArcherTowerCost;
        cost[1] = itemCosts.WizardTowerCost;
        cost[2] = itemCosts.GoldMineCost;
        cost[3] = itemCosts.ManaWellCost;

        UpdateTowerButtonUI();
    }

    public void UpdateTowerButtonUI()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(cost[i] <= GameController.Gold)
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