using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Tower
{
    enum Types
    {
        Tower = 0, ResourceBuilding = 1, Destroy = 2
    }
    public enum InteractionMode
    {
        None = 0, PlacementMode = 1, UpgradeMode = 2, SpecialAbilitySelectMode = 3
    }

    public class TowerInteraction : MonoBehaviour
    {
        // Variables
        [SerializeField] private Camera camera;
        [Space(6)]
        [Header("Tower Buildings")]
        [SerializeField] private Transform TowerParent;
        [SerializeField] private GameObject[] TowerList;
        private int TowerSelectedIndex;
        private GameObject[] TowerPrefablist;

        [Header("Resource Building")]
        [SerializeField] private Transform BuildingParent;
        [SerializeField] private GameObject[] BuildingList;
        private int BuildingSelectedIndex;
        private GameObject[] BuildingPrefablist;

        [Space(6)]

        [SerializeField] private UINotificationManager notificationManager;

        [Space(6)]
        [SerializeField] private GameObject ResourceCollectButtonPrefabGold;
        [SerializeField] private GameObject ResourceCollectButtonPrefabMana;

        [Header("Building Collection Button Parents")]
        [SerializeField] private Transform GoldButtonParent;
        [SerializeField] private Transform ManaButtonParent;

        [Space(6)]
        [SerializeField] private Button[] SelectionButtons;
        [SerializeField] public int ButtonSelectionIndex = 0;
        private int previousButtonSelectionIndex = 0;

        private Types CurrentBuildingType;
        private Types PreviousBuildingType;

        [Space(6)]
        [SerializeField] private Transform TowerSpecialUIParent;
        [SerializeField] private GameObject ArcherSpecialAttackUIPrefab;
        [SerializeField] private GameObject WizardSpecialAttackUIPrefab;
        [SerializeField] private GameObject CannonSpecialAttackUIPrefab;

        [Header("Script References")]
        [SerializeField] private UI.UpgradeUI upgradeUI;
        [SerializeField] private ResourceUIManager resourceManager;
        [SerializeField] private ItemCost itemCosts;

        // private variables
        private Vector3 hitPoint = Vector3.zero;
        private RaycastHit TowerHit;

        // Set Current tower interaction mode
        [HideInInspector] public InteractionMode CurrentInteractionMode;
        [Space(6)]
        [SerializeField] private List<TowerCore> SpecialAbilityUnlockedTowerList;

        [SerializeField] private AudioClip constructionAudio;
        [SerializeField] private Image towerRangeIndicator;
        private string sceneName;

        private void Start()
        {
            CurrentInteractionMode = InteractionMode.UpgradeMode;

            // Create Template Towers
            TowerPrefablist = new GameObject[TowerList.Length];
            for(int i = 0; i < TowerList.Length; i++)
            {
                TowerPrefablist[i] = Instantiate(TowerList[i], Vector3.zero, Quaternion.identity, transform);
                TowerPrefablist[i].SetActive(false);
                TowerPrefablist[i].name = TowerPrefablist[i].name.Replace("Clone", "Template");
                TowerPrefablist[i].GetComponentInChildren<BoxCollider>().enabled = false;
                TowerPrefablist[i].GetComponent<TowerCore>().enabled = false;
            }

            // Create Template Buildings
            BuildingPrefablist = new GameObject[BuildingList.Length];
            for(int i = 0; i < BuildingList.Length; i++)
            {
                BuildingPrefablist[i] = Instantiate(BuildingList[i], Vector3.zero, Quaternion.identity, transform);
                BuildingPrefablist[i].SetActive(false);
                BuildingPrefablist[i].name = BuildingPrefablist[i].name.Replace("Clone", "Template");
                BuildingPrefablist[i].GetComponentInChildren<BoxCollider>().enabled = false;
                BuildingPrefablist[i].GetComponent<ResourceBuilding.ResourceBuildingCore>().enabled = false;
            }

            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        private void Update()
        {
            if(CurrentInteractionMode == InteractionMode.PlacementMode)
            {
                if(Input.GetMouseButton(0))
                {
                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        var ray = camera.ScreenPointToRay(Input.mousePosition);
                        TowerHit = new RaycastHit();

                        if (Physics.Raycast(ray, out TowerHit, 100f))
                        {
                            hitPoint.x = Mathf.Ceil(TowerHit.point.x) - 0.5f;
                            hitPoint.z = Mathf.Ceil(TowerHit.point.z) - 0.5f;

                            if (TowerHit.collider.tag == "PlaceableGround")
                            {
                                if (CurrentBuildingType == Types.Tower)
                                {
                                    TowerPrefablist[TowerSelectedIndex].SetActive(true);
                                    TowerPrefablist[TowerSelectedIndex].transform.position = hitPoint;

                                    EnableRangeIndicator(hitPoint);
                                }
                                else
                                {
                                    BuildingPrefablist[BuildingSelectedIndex].SetActive(true);
                                    BuildingPrefablist[BuildingSelectedIndex].transform.position = hitPoint;
                                }
                            }
                        }
                    }
                    
                }

                if(Input.GetMouseButtonUp(0))
                {
                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        if (TowerHit.collider != null)
                        {
                            if (TowerHit.collider.tag == "PlaceableGround")
                            {
                                if (upgradeUI.PayGold(GetCostAmount()))
                                {
                                    if (CurrentBuildingType == Types.Tower)
                                    {
                                        DataManager.TowerPlaced();

                                        GameObject go = Instantiate(TowerList[TowerSelectedIndex], hitPoint, Quaternion.identity, TowerParent);
                                        go.GetComponent<TowerCore>().SetNewSprite();

                                        FindObjectOfType<AudioManagement>().PlayAudioClip(constructionAudio, AudioMixerGroups.SFX);

                                        DisableRangeIndicator();

                                        // Create UI
                                        switch (go.GetComponent<TowerCore>().towerType)
                                        {
                                            case TowerType.ArcherTower:
                                                go.GetComponent<TowerCore>().specialDirectionUI =
                                                    Instantiate(ArcherSpecialAttackUIPrefab, new Vector3(hitPoint.x, TowerSpecialUIParent.position.y, hitPoint.z), new Quaternion(0, 0, 0, 0), TowerSpecialUIParent);
                                                break;
                                            case TowerType.WizardTower:
                                                go.GetComponent<TowerCore>().specialDirectionUI =
                                                    Instantiate(WizardSpecialAttackUIPrefab, new Vector3(hitPoint.x, TowerSpecialUIParent.position.y, hitPoint.z), new Quaternion(0, 0, 0, 0), TowerSpecialUIParent);
                                                break;
                                            case TowerType.CannonTower:
                                                go.GetComponent<TowerCore>().specialDirectionUI =
                                                    Instantiate(CannonSpecialAttackUIPrefab, new Vector3(hitPoint.x, TowerSpecialUIParent.position.y, hitPoint.z), new Quaternion(0, 0, 0, 0), TowerSpecialUIParent);
                                                break;
                                        }

                                        go.GetComponent<TowerCore>().Init();
                                        go.GetComponent<TowerCore>().specialDirectionUI.SetActive(false);
                                    }
                                    else if (CurrentBuildingType == Types.ResourceBuilding)
                                    {
                                        Transform t;
                                        switch (BuildingSelectedIndex)
                                        {
                                            default:
                                                t = GoldButtonParent;
                                                break;
                                            case 1:
                                                t = ManaButtonParent;
                                                break;

                                        }
                                        GameObject go = Instantiate(BuildingList[BuildingSelectedIndex], hitPoint, Quaternion.identity, BuildingParent);

                                        GameObject buGold = Instantiate(ResourceCollectButtonPrefabGold, t);
                                        buGold.transform.localPosition = new Vector2(hitPoint.x, hitPoint.z);
                                        buGold.SetActive(false);

                                        GameObject buMana = Instantiate(ResourceCollectButtonPrefabMana, t);
                                        buMana.transform.localPosition = new Vector2(hitPoint.x, hitPoint.z);
                                        buMana.SetActive(false);

                                        // Settings for the Collect Resource Building
                                        go.GetComponent<ResourceBuilding.ResourceBuildingCore>().goldButton = buGold.GetComponent<Button>();
                                        go.GetComponent<ResourceBuilding.ResourceBuildingCore>().manaButton = buMana.GetComponent<Button>();
                                        go.GetComponent<ResourceBuilding.ResourceBuildingCore>().resourceManager = resourceManager;
                                        go.GetComponent<ResourceBuilding.ResourceBuildingCore>().AddButtonListener();
                                        go.GetComponent<ResourceBuilding.ResourceBuildingCore>().Init();
                                    }
                                }
                                else
                                {
                                    notificationManager.OpenGoldNotification();
                                }
                            }
                            if (CurrentBuildingType == Types.Destroy)
                            {
                                if (TowerHit.collider.tag == "Tower")
                                {
                                    DataManager.TowerRemoved();

                                    Destroy(TowerHit.collider.GetComponent<TowerCore>().specialDirectionUI);
                                    Destroy(TowerHit.collider.gameObject);
                                    CurrentBuildingType = PreviousBuildingType;
                                    SetSelectedButtonAttributes(previousButtonSelectionIndex);
                                    upgradeUI.PayGold(-5);
                                }
                                else if (TowerHit.collider.tag == "Building")
                                {
                                    Destroy(TowerHit.collider.GetComponent<ResourceBuilding.ResourceBuildingCore>().goldButton);
                                    Destroy(TowerHit.collider.gameObject);
                                    CurrentBuildingType = PreviousBuildingType;
                                    SetSelectedButtonAttributes(previousButtonSelectionIndex);
                                    upgradeUI.PayGold(-10);
                                }
                            }
                        }
                    }
                    

                    // Reset Show off Prefabs
                    TowerPrefablist[TowerSelectedIndex].SetActive(false);
                    TowerPrefablist[TowerSelectedIndex].transform.position = Vector3.zero;
                    BuildingPrefablist[BuildingSelectedIndex].SetActive(false);
                    BuildingPrefablist[BuildingSelectedIndex].transform.position = Vector3.zero;
                }
            }
            else if(CurrentInteractionMode == InteractionMode.UpgradeMode)
            {
                if(Input.GetMouseButtonDown(0) && sceneName != "Level1" )
                {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit _hit = new RaycastHit();

                    if(Physics.Raycast(ray, out _hit, 100f))
                    {
                        if(_hit.collider.tag == "Tower")
                        {
                            upgradeUI.currentTower = _hit.collider.GetComponent<TowerCore>();
                            upgradeUI.towerUpgradeCostText.text = upgradeUI.currentTower.TowerUpgradeCosts.UpgradeCosts[upgradeUI.currentTower.TowerLevel - 1] + "";
                            upgradeUI.UpdateUIPosition(_hit.collider.transform.position.x, _hit.collider.transform.position.z);
                            upgradeUI.SpecialButton();
                            EnableRangeIndicator(_hit.collider.transform.position);
                            CurrentInteractionMode = InteractionMode.None;
                        }
                    }
                }
            }
            else if(CurrentInteractionMode == InteractionMode.SpecialAbilitySelectMode)
            {
                for(int i = 0; i < SpecialAbilityUnlockedTowerList.Count; i++)
                {
                    SpecialAbilityUnlockedTowerList[i].SpecialAttackDirectionLookAt();
                }
                if(SpecialAbilityUnlockedTowerList.Count > 0)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                        RaycastHit _hit = new RaycastHit();

                        if(Physics.Raycast(ray, out _hit, 100f))
                        {
                            if(_hit.collider.tag == "Tower")
                            {
                                if(upgradeUI.PayMana(2))
                                {
                                    _hit.collider.GetComponent<Tower.TowerCore>().StartSecondairyAttack();
                                    CurrentInteractionMode = InteractionMode.UpgradeMode;
                                    SetSelectedButtonAttributes(0);
                                }
                            }
                        }
                    }
                }
            }
        }

        private int GetCostAmount()
        {
            if(CurrentBuildingType == Types.Tower)
            {
                switch(TowerSelectedIndex)
                {
                    default:
                        return itemCosts.ArcherTowerCost;
                    case 1:
                        return itemCosts.WizardTowerCost;
                    case 2:
                        return itemCosts.CannonTowerCost;
                }
            }
            else if(CurrentBuildingType == Types.ResourceBuilding)
            {
                switch(BuildingSelectedIndex)
                {
                    default:
                        return itemCosts.GoldMineCost;
                    case 1:
                        return itemCosts.ManaWellCost;
                }
            }

            return 0;
        }


        #region Public Functions

        // Enable the UI Arrows for the Special Attacks
        public void OnSpecialMode(bool OnOrOff)
        {
            for(int i = 0; i < SpecialAbilityUnlockedTowerList.Count; i++)
            {
                SpecialAbilityUnlockedTowerList[i].specialDirectionUI.SetActive(OnOrOff);
            }
        }

        // Set the scale of the button that was just pressed to indicate what tower has been selected.
        public void SetSelectedButtonAttributes(int _index)
        {
            // Reset the previous button
            SelectionButtons[ButtonSelectionIndex].transform.localScale = Vector2.one;

            // Set the new Button
            SelectionButtons[_index].transform.localScale = new Vector2(1.2f, 1.2f);

            // set variable to remember this button index for next time.
            ButtonSelectionIndex = _index;
        }

        // Set the Tower that the user selects to build
        public void SelectTower(int _i)
        {
            TowerSelectedIndex = _i;
            CurrentBuildingType = Types.Tower;
            CurrentInteractionMode = InteractionMode.PlacementMode;
        }

        // Set the Building that the user selects to build
        public void SelectBuilding(int _i)
        {
            BuildingSelectedIndex = _i;
            CurrentBuildingType = Types.ResourceBuilding;
            CurrentInteractionMode = InteractionMode.PlacementMode;
        }

        // Set Destroy towers/buildings mode
        public void SetDeleteBuilding()
        {
            previousButtonSelectionIndex = ButtonSelectionIndex;
            PreviousBuildingType = CurrentBuildingType;
            CurrentBuildingType = Types.Destroy;
        }

        // Set the interaction mode like upgrade mode, special attack mode.
        public void SetInteractionMode(int _i)
        {
            CurrentInteractionMode = (InteractionMode)_i;
        }

        // Add a tower that has reached the Special Attack level to a list, the list will contain all towers that can use their special attack.
        public void AddTowerToSpecialAbilityUnlockedList(TowerCore core)
        {
            if(SpecialAbilityUnlockedTowerList == null)
            {
                SpecialAbilityUnlockedTowerList = new List<TowerCore>();
            }

            SpecialAbilityUnlockedTowerList.Add(core);
        }

        public void EnableRangeIndicator(Vector3 indicatorPos) {
                float shootRange = TowerPrefablist[TowerSelectedIndex].GetComponent<TowerCore>().ShootingRange * .1f;

                towerRangeIndicator.gameObject.SetActive(true);
                towerRangeIndicator.transform.position = indicatorPos;
                towerRangeIndicator.transform.localScale = new Vector3(shootRange, shootRange);
        }

        public void DisableRangeIndicator() {
                towerRangeIndicator.gameObject.SetActive(false);
        }

        #endregion
    }
}