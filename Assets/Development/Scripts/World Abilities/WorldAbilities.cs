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

        if ( thousandCranesAbilityOn ) {
            if ( thousandCranesTimerAbilityOn < thousandCranes.speedDebuffTime ) {
                thousandCranesTimerAbilityOn += Time.deltaTime;
                thousandCranesButton.image.color = thousandCranesActiveColor;
            } else {
                ResetThousandCranes();
            }
        }

        if ( thousandCranesTimer < thousandCranesCooldown ) {
            thousandCranesButton.interactable = false;
            thousandCranesTimer += Time.deltaTime;
        } else {
            thousandCranesButton.interactable = true;
        }

    }

    [SerializeField] private ThousandCranes thousandCranes;
    [SerializeField] private Color thousandCranesActiveColor;
    [SerializeField] private Button thousandCranesButton;
    [SerializeField] private float thousandCranesCooldown = 15;
    [SerializeField] private int thousandCranesManaCost = 8;
    private float thousandCranesTimer;
    private bool thousandCranesAbilityActive = false;
    private float thousandCranesTimerAbilityOn;
    private bool thousandCranesAbilityOn = false;
    public void ThousandCranes() {
        thousandCranesAbilityActive = !thousandCranesAbilityActive;

        if ( !thousandCranesAbilityOn && thousandCranesAbilityActive && GameController.Mana >= thousandCranesManaCost && thousandCranesTimer > thousandCranesCooldown ) {
            thousandCranes.ThousandCranesAbility();
            thousandCranesAbilityOn = true;
            print("oasjtaojtw");
        }
    }

    private void ResetThousandCranes() {
        thousandCranesAbilityActive = false;
        thousandCranesAbilityOn = false;
        thousandCranesTimer = 0;
        thousandCranesTimerAbilityOn = 0;
        GameController.Mana -= thousandCranesManaCost;
        thousandCranesButton.image.color = Color.white;
    }

    //[SerializeField] private GameObject fireworkRocket;
    //private bool fireworkAbilityActive = false;
    //public void FireworkRocket() {
    //    fireworkAbilityActive = !fireworkAbilityActive;
    //}

    [SerializeField] private GameObject ninjaDash;
    [SerializeField] private Color ninjaDashActiveColor;
    [SerializeField] private Button ninjaDashButton;
    [SerializeField] private float ninjaDashCooldown = 30;
    [SerializeField] private int ninjaDashManaCost = 15;
    private float ninjaDashTimer;
    private bool ninjaDashAbilityActive = false;
    public void NinjaDash() {
        if ( ninjaDash.gameObject.activeInHierarchy ) {
            ninjaDash.gameObject.SetActive(false);
            ninjaDashAbilityActive = false;
            ninjaDashButton.image.color = Color.white;
        } else if ( !ninjaDash.gameObject.activeInHierarchy && GameController.Mana >= ninjaDashManaCost && ninjaDashTimer > ninjaDashCooldown ) {
            ninjaDash.gameObject.SetActive(true);
            ninjaDashAbilityActive = true;
            ninjaDashButton.image.color = ninjaDashActiveColor;
        }
    }

    public void ResetNinjaDash() {
        ninjaDashAbilityActive = false;
        ninjaDash.gameObject.SetActive(false);
        ninjaDashTimer = 0;
        GameController.Mana -= ninjaDashManaCost;
        ninjaDashButton.image.color = Color.white;
        print("FINISHED NINJA DASH");
    }
}
