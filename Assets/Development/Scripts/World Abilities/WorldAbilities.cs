using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldAbilities : MonoBehaviour
{
    private TMPro.TextMeshProUGUI interactionText;
    [SerializeField] private Color notEnoughManaColor;
    [SerializeField] private AudioClip buttonAudio;
    [SerializeField] private AudioManagement audioManager;
    [SerializeField] private ResourceUIManager resourceUIManager;
    [SerializeField] private Tower.TowerInteraction towerInteraction;

    private void NotEnoughMana( int manaCost, Button button ) {
        if ( GameController.Mana < manaCost )
            button.image.color = notEnoughManaColor;
        else
            button.image.color = Color.white;
    }
    private void Start() {
        InitiateCranes();
        ShowCraneFlock(false);
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
            ShowCraneFlock(true);
            Vector3 vec = new Vector3(( Camera.main.transform.position.x + screenSize + 2 ) * 2, 0,
                ( Camera.main.transform.position.z - -screenSize + 5 ));
            craneParent.transform.position = Vector3.MoveTowards(craneParent.transform.position, vec, 50 * Time.deltaTime);

            if ( thousandCranesInUseTimer < thousandCranes.speedDebuffTime /*&& craneParent.transform.position.x >= vec.x*/ ) {
                thousandCranesInUseTimer += Time.deltaTime;
                thousandCranesButton.image.color = thousandCranesActiveColor;
            } else {
                ShowCraneFlock(false);
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
    [SerializeField] private Button thousandCranesButton;
    [SerializeField] private Color thousandCranesActiveColor;
    [SerializeField] private float thousandCranesCooldown = 15;
    [SerializeField] private int thousandCranesManaCost = 8;
    private float thousandCranesTimer;
    private bool thousandCranesAbilityActive = false;
    private float thousandCranesInUseTimer;
    private bool thousandCranesInUse = false;
    public void ThousandCranes() {
        if (towerInteraction.CurrentInteractionMode == Tower.InteractionMode.PlacementMode)
            return;

        thousandCranesAbilityActive = !thousandCranesAbilityActive;

        if ( !thousandCranesInUse && thousandCranesAbilityActive && GameController.Mana >= thousandCranesManaCost && thousandCranesTimer > thousandCranesCooldown ) {
            thousandCranes.ThousandCranesAbility();
            thousandCranesInUse = true;
            craneParent.transform.position = new Vector3(( ( Camera.main.transform.position.x + screenSize + 2 ) * 2 ) * -1, 0,
                 ( Camera.main.transform.position.z + screenSize ) * -1);
        }
    }

    [SerializeField] private Sprite[] craneSprites;
    [SerializeField] private GameObject cranePrefab;
    [SerializeField] private Color[] craneColors;
    private Transform craneParent;
    private List<GameObject> craneFlockList = new List<GameObject>();
    private float screenSize;

    private void InitiateCranes() {
        float camDist = 5;
        //Vector3 screenBoundaries = Camera.main.transform.TransformPoint(Vector3.forward * camDist);
        screenSize = Camera.main.orthographicSize;
        print("Screensize" + screenSize);
        //Vector3 cranePos = new Vector3(-screenSize, screenSize, screenSize);
        //cranePos = Camera.main.WorldToScreenPoint(cranePos);

        if ( craneParent == null )
            craneParent = thousandCranes.transform.GetChild(0);

        //for ( int y = 0; y < 10; y++ ) {
        //    for ( int x = 0; x < 18; x++ ) {
        GameObject go = Instantiate(cranePrefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
        for ( int i = 0; i < go.transform.childCount; i++ )
            craneFlockList.Add(go.transform.GetChild(i).gameObject);

        //print(( screenSize * Camera.main.aspect ) - Camera.main.transform.position.x);
        //go.transform.position = new Vector3(( -screenSize * Camera.main.aspect ) + Camera.main.transform.position.x + ( x * 3 ), 10,
        //    -screenSize + Camera.main.transform.position.z + ( y * 3 ));

        go.transform.position = new Vector3(( -screenSize * Camera.main.aspect ) + Camera.main.transform.position.x, 10,
            ( -screenSize * Camera.main.aspect ) + Camera.main.transform.position.z);

        for ( int i = 0; i < craneFlockList.Count; i++ ) {
            int rand = Random.Range(0, craneSprites.Length);
            craneFlockList[i].GetComponent<SpriteRenderer>().sprite = craneSprites[rand];
            int c = Random.Range(0, craneColors.Length);
            craneFlockList[i].GetComponent<SpriteRenderer>().color = craneColors[c];
        }

        //go.transform.SetParent(craneParent);
        craneParent = go.transform;

        //craneFlockList.Add(go);

        //    }
        //}
    }

    private void ShowCraneFlock( bool showCranes ) {
        craneParent.gameObject.SetActive(showCranes);
    }

    private void ResetThousandCranes() {
        thousandCranesAbilityActive = false;
        thousandCranesInUse = false;
        thousandCranesTimer = 0;
        thousandCranesInUseTimer = 0;
        GameController.Mana -= thousandCranesManaCost;
        resourceUIManager.UpdateResourceUI();
        thousandCranesButton.image.color = Color.white;
    }

    [Space(20)]
    [SerializeField] private GameObject fireworkRocket;
    [SerializeField] private Button fireworkButton;
    [SerializeField] private Color fireworkActiveColor;
    [SerializeField] private float fireworkCooldown = 10;
    [SerializeField] private int fireworkManaCost = 5;
    private float fireworkTimer;
    private bool fireworkAbilityActive = false;
    public void FireworkRocket() {
        if ( towerInteraction.CurrentInteractionMode == Tower.InteractionMode.PlacementMode )
            return;

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
        resourceUIManager.UpdateResourceUI();
        fireworkButton.image.color = Color.white;
    }

    [Space(20)]
    [SerializeField] private GameObject ninjaDash;
    [SerializeField] private Button ninjaDashButton;
    [SerializeField] private Color ninjaDashActiveColor;
    [SerializeField] private float ninjaDashCooldown = 30;
    [SerializeField] private int ninjaDashManaCost = 15;
    private float ninjaDashTimer;
    private bool ninjaDashAbilityActive = false;
    public void NinjaDash() {
        if ( towerInteraction.CurrentInteractionMode == Tower.InteractionMode.PlacementMode )
            return;

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
        ninjaDashTimer = 0f;
        GameController.Mana -= ninjaDashManaCost;
        resourceUIManager.UpdateResourceUI();
        ninjaDashButton.image.color = Color.white;
        Debug.Log("samurai reset");
    }

    public void WorldAbilityAudio() {
        audioManager.PlayAudioClip(buttonAudio, AudioMixerGroups.SFX);
    }
}