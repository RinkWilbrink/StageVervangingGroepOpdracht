using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class ResourceUIManager : MonoBehaviour
{
    // Variables
    [SerializeField] private TMPro.TextMeshProUGUI GoldCount;
    [SerializeField] private TMPro.TextMeshProUGUI ManaCount;

    private void Start()
    {
        UpdateResourceUI();
    }

    public void UpdateResourceUI()
    {
        GoldCount.text = string.Format("{0}", GameController.Gold);
        ManaCount.text = string.Format("{0}", GameController.Mana);
    }
}