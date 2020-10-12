using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    // Variables
    //[SerializeField] private string UpgradeName;

    private TMPro.TextMeshProUGUI UpgradeName_Text;
    private TMPro.TextMeshProUGUI UpgradeMultiplier_Text;
    
    void Start()
    {
        foreach (var go in gameObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            if (go.name == "UpgradeName_Text")
            {
                UpgradeName_Text = go;
                //UpgradeName_Text.text = UpgradeName;
            }

            if (go.name == "UpgradeLevel_Text")
            {
                UpgradeMultiplier_Text = go;
                //UpgradeMultiplier_Text.text = "x1";
            }
        }
    }
}