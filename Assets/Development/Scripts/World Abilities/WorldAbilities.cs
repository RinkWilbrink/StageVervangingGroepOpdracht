using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldAbilities : MonoBehaviour
{
    private TMPro.TextMeshProUGUI interactionText;

    private void Update() {
        //print("Mana: " + GameController.Mana);
        //print(ninjaDashTimer);

        if ( ninjaDashTimer < ninjaDashCooldown ) {
            ninjaDashButton.interactable = false;
            ninjaDashTimer += Time.deltaTime;
        } else {
            ninjaDashButton.interactable = true;
        }

        //if ( !ninjaDashAbilityActive ) {
        //    ninjaDash.gameObject.SetActive(false);
        //}
    }

    //[SerializeField] private GameObject thousandCranes;
    //private bool thousandCranesAbilityActive = false;
    //public void ThousandCranes() {
    //    thousandCranesAbilityActive = !thousandCranesAbilityActive;
    //}

    //[SerializeField] private GameObject fireworkRocket;
    //private bool fireworkAbilityActive = false;
    //public void FireworkRocket() {
    //    fireworkAbilityActive = !fireworkAbilityActive;
    //}

    [SerializeField] private GameObject ninjaDash;
    [SerializeField] private Color ninjaDashActiveColor;
    [SerializeField] private Button ninjaDashButton;
    [SerializeField] private float ninjaDashCooldown = 20;
    [SerializeField] private int ninjaDashManaCost = 10;
    private float ninjaDashTimer;
    private bool ninjaDashAbilityActive = false;
    public void NinjaDash() {
        if ( ninjaDash.gameObject.activeInHierarchy ) {
            ninjaDash.gameObject.SetActive(false);
            ninjaDashAbilityActive = false;
            ninjaDashButton.GetComponent<Image>().color = Color.white;
        } else if ( !ninjaDash.gameObject.activeInHierarchy && GameController.Mana >= ninjaDashManaCost && ninjaDashTimer > ninjaDashCooldown ) {
            ninjaDash.gameObject.SetActive(true);
            ninjaDashAbilityActive = true;
            ninjaDashButton.GetComponent<Image>().color = ninjaDashActiveColor;
        }
    }

    public void FinishedNinjaDash() {
        ninjaDashAbilityActive = false;
        ninjaDash.gameObject.SetActive(false);
        ninjaDashTimer = 0;
        GameController.Mana -= ninjaDashManaCost;
        ninjaDashButton.GetComponent<Image>().color = Color.white;
        print("FINISHED NINJA DASH");
    }
}
