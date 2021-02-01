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
    [SerializeField] private UIAnimation uiAnimation;

    [Space(10)]
    [SerializeField] private ThousandCranes thousandCranes;
    [SerializeField] private Button thousandCranesButton;
    [SerializeField] private Image thousandCranesIndicator;
    [SerializeField] private Color thousandCranesActiveColor;
    [SerializeField] private float thousandCranesCooldown;
    [SerializeField] private int thousandCranesManaCost;
    private float thousandCranesTimer;
    private bool thousandCranesAbilityActive = false;
    private float thousandCranesInUseTimer;
    private bool thousandCranesInUse = false;

    [SerializeField] private Sprite[] craneSprites;
    [SerializeField] private GameObject cranePrefab;
    [SerializeField] private Color[] craneColors;
    private Transform craneParent;
    private List<GameObject> craneFlockList = new List<GameObject>();
    private float screenSize;

    [Space(20)]
    [SerializeField] private GameObject fireworkRocket;
    public Button fireworkButton;
    [SerializeField] private Image fireworkIndicator;
    [SerializeField] private Color fireworkActiveColor;
    [SerializeField] private float fireworkCooldown;
    public int fireworkManaCost;
    public float fireworkTimer;
    private bool fireworkAbilityActive = false;

    [Space(20)]
    [SerializeField] private GameObject ninjaDash;
    public Button ninjaDashButton;
    [SerializeField] private Image ninjaDashIndicator;
    [SerializeField] private Color ninjaDashActiveColor;
    [SerializeField] private float ninjaDashCooldown;
    public int ninjaDashManaCost;
    public float ninjaDashTimer;
    private bool ninjaDashAbilityActive = false;

    private void NotEnoughMana( int manaCost, Button button ) {
        if ( GameController.Mana < manaCost )
        {
            button.image.color = notEnoughManaColor;
            button.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
        }
        else
        {
            button.image.color = Color.white;
            button.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
        }
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

        if ( ninjaDashTimer < ninjaDashCooldown ) 
        {
            ninjaDashButton.interactable = false;
            ninjaDashTimer += Time.deltaTime;
        } 
        else if ( ninjaDashTimer >= ninjaDashCooldown)
        {
            ninjaDashButton.interactable = true;
        }

        if ( fireworkTimer < fireworkCooldown ) 
        {
            fireworkButton.interactable = false;
            fireworkTimer += Time.deltaTime;
        }
        else if ( fireworkTimer >= fireworkCooldown )
        {
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

        if (!thousandCranesInUse)
        {
            thousandCranesIndicator.fillAmount = 1 / thousandCranesCooldown * thousandCranesTimer;
        }

        if (!fireworkAbilityActive)
        {
            fireworkIndicator.fillAmount = 1 / fireworkCooldown * fireworkTimer;
        }

        if (!ninjaDashAbilityActive)
        {
            ninjaDashIndicator.fillAmount = 1 / ninjaDashCooldown * ninjaDashTimer;
        }
    }

    public void ThousandCranes() {
        if (towerInteraction.CurrentInteractionMode == Tower.InteractionMode.PlacementMode)
        {
            towerInteraction.CurrentInteractionMode = Tower.InteractionMode.None;
            uiAnimation.TriggerBuildMenu();
        }

        thousandCranesAbilityActive = !thousandCranesAbilityActive;

        if ( !thousandCranesInUse && thousandCranesAbilityActive && GameController.Mana >= thousandCranesManaCost && thousandCranesTimer > thousandCranesCooldown ) {
            thousandCranes.ThousandCranesAbility();
            thousandCranesInUse = true;
            thousandCranesIndicator.fillAmount = 0;
            GameController.Mana -= thousandCranesManaCost;
            resourceUIManager.UpdateResourceUI();
            craneParent.transform.position = new Vector3(( ( Camera.main.transform.position.x + screenSize + 2 ) * 2 ) * -1, 0,
                 ( Camera.main.transform.position.z + screenSize ) * -1);
        }
    }

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
        thousandCranesButton.image.color = Color.white;
    }
    
    public void FireworkRocket() {
        if (towerInteraction.CurrentInteractionMode == Tower.InteractionMode.PlacementMode)
        {
            towerInteraction.CurrentInteractionMode = Tower.InteractionMode.None;
            uiAnimation.TriggerBuildMenu();
        }

        if ( fireworkRocket.gameObject.activeInHierarchy ) {
            fireworkRocket.SetActive(false);
            fireworkAbilityActive = false;
            fireworkButton.image.color = Color.white;
        } else if ( !fireworkRocket.gameObject.activeInHierarchy && GameController.Mana >= fireworkManaCost && fireworkTimer > fireworkCooldown ) {
            fireworkRocket.SetActive(true);
            fireworkAbilityActive = true;
            fireworkIndicator.fillAmount = 0;
            fireworkButton.image.color = fireworkActiveColor;
        }
    }

    public void ResetFireworkRocket() {
        fireworkAbilityActive = false;
        fireworkRocket.SetActive(false);
        fireworkTimer = 0f;
        fireworkButton.image.color = Color.white;
    }
    
    public void NinjaDash() {
        if ( towerInteraction.CurrentInteractionMode == Tower.InteractionMode.PlacementMode )
        {
            towerInteraction.CurrentInteractionMode = Tower.InteractionMode.None;
            uiAnimation.TriggerBuildMenu();
        }
        
        if ( ninjaDash.gameObject.activeInHierarchy ) {
            ninjaDash.gameObject.SetActive(false);
            ninjaDashAbilityActive = false;
            ninjaDashButton.image.color = Color.white;
        } else if ( !ninjaDash.gameObject.activeInHierarchy && GameController.Mana >= ninjaDashManaCost && ninjaDashTimer > ninjaDashCooldown ) {
            ninjaDash.gameObject.SetActive(true);
            ninjaDashAbilityActive = true;
            ninjaDashIndicator.fillAmount = 0;
            resourceUIManager.UpdateResourceUI();
            ninjaDashButton.image.color = ninjaDashActiveColor;
        }
    }

    public void ResetNinjaDash() {
        ninjaDashAbilityActive = false;
        ninjaDashTimer = 0f;
        ninjaDashButton.image.color = Color.white;
        //Debug.Log("samurai reset");
    }

    public void WorldAbilityAudio() {
        audioManager.PlayAudioClip(buttonAudio, AudioMixerGroups.SFX);
    }
}