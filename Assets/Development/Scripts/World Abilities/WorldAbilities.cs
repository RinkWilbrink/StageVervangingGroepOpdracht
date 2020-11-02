using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldAbilities : MonoBehaviour
{
    private TMPro.TextMeshProUGUI interactionText;
    [SerializeField] private Color notEnoughManaColor;

    private void NotEnoughMana( int manaCost, Button button ) {
        if ( GameController.Mana < manaCost )
            button.image.color = notEnoughManaColor;
        else
            button.image.color = Color.white;
    }

    private void Update() {
        //print("Mana: " + GameController.Mana);
        //print(ninjaDashTimer);
        if ( !thousandCranesInUse )
            NotEnoughMana(thousandCranesManaCost, thousandCranesButton);
        if ( !ninjaDash.activeInHierarchy )
            NotEnoughMana(ninjaDashManaCost, ninjaDashButton);
        if ( !fireworkRocket.activeInHierarchy )
            NotEnoughMana(fireworkManaCost, fireworkButton);

        if ( ninjaDashTimer < ninjaDashCooldown ) {
            ninjaDashButton.interactable = false;
            ninjaDashTimer += Time.deltaTime;
        } else {
            ninjaDashButton.interactable = true;
        }

        if ( fireworkTimer < fireworkCooldown ) {
            fireworkButton.interactable = false;
            fireworkTimer += Time.deltaTime;
        } else {
            fireworkButton.interactable = true;
        }

        if ( thousandCranesInUse ) {
            if ( thousandCranesInUseTimer < thousandCranes.speedDebuffTime ) {
                thousandCranesInUseTimer += Time.deltaTime;
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

    [Space(10)]
    [SerializeField] private ThousandCranes thousandCranes;
    [SerializeField] private Color thousandCranesActiveColor;
    [SerializeField] private Button thousandCranesButton;
    [SerializeField] private float thousandCranesCooldown = 15;
    [SerializeField] private int thousandCranesManaCost = 8;
    private float thousandCranesTimer;
    private bool thousandCranesAbilityActive = false;
    private float thousandCranesInUseTimer;
    private bool thousandCranesInUse = false;
    public void ThousandCranes() {
        thousandCranesAbilityActive = !thousandCranesAbilityActive;

        if ( !thousandCranesInUse && thousandCranesAbilityActive && GameController.Mana >= thousandCranesManaCost && thousandCranesTimer > thousandCranesCooldown ) {
            thousandCranes.ThousandCranesAbility();
            thousandCranesInUse = true;
        }
    }

    private void ResetThousandCranes() {
        thousandCranesAbilityActive = false;
        thousandCranesInUse = false;
        thousandCranesTimer = 0;
        thousandCranesInUseTimer = 0;
        GameController.Mana -= thousandCranesManaCost;
        thousandCranesButton.image.color = Color.white;
    }

    [Space(20)]
    [SerializeField] private GameObject fireworkRocket;
    [SerializeField] private Color fireworkActiveColor;
    [SerializeField] private Button fireworkButton;
    [SerializeField] private float fireworkCooldown = 10;
    [SerializeField] private int fireworkManaCost = 5;
    private float fireworkTimer;
    private bool fireworkAbilityActive = false;
    public void FireworkRocket() {
        if ( fireworkRocket.gameObject.activeInHierarchy ) {
            fireworkRocket.SetActive(false);
            fireworkAbilityActive = false;
            fireworkButton.image.color = Color.white;
        } else if ( !fireworkRocket.gameObject.activeInHierarchy && GameController.Mana >= fireworkManaCost && fireworkTimer > fireworkCooldown ) {
            fireworkRocket.SetActive(true);
            fireworkAbilityActive = true;
            fireworkButton.image.color = fireworkActiveColor;
        }
    }

    public void ResetFireworkRocket() {
        fireworkAbilityActive = false;
        fireworkRocket.SetActive(false);
        fireworkTimer = 0f;
        GameController.Mana -= fireworkManaCost;
        fireworkButton.image.color = Color.white;
    }

    [Space(20)]
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
        ninjaDash.SetActive(false);
        ninjaDashTimer = 0f;
        GameController.Mana -= ninjaDashManaCost;
        ninjaDashButton.image.color = Color.white;
        print("FINISHED NINJA DASH");
    }
}
