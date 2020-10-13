using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    // Variables
    //[SerializeField] private string UpgradeName;
    [SerializeField] private string IDK1;
    [SerializeField] private string IDK2;

    //[HideInInspector] public TMPro.TextMeshProUGUI UpgradeName_Text;
    [HideInInspector] public TMPro.TextMeshProUGUI UpgradeMultiplier_Text;
    [HideInInspector] public TMPro.TextMeshProUGUI UpgradeCost_Text;
    
    void Awake()
    {
        foreach (var go in gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            //if (go.name == "UpgradeName_Text")
            //{
            //    UpgradeName_Text = go;
            //}

            if (go.name == "Upgrade_Level")
            {
                UpgradeMultiplier_Text = go;
            }
            if(go.name == "UpgradeCost")
            {
                UpgradeCost_Text = go;
            }
        }
    }
}