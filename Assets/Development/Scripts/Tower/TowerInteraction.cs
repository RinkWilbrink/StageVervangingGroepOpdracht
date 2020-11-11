using ResourceBuilding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tower
{
    enum BuildingTypes
    {
        Tower = 0, ResourceBuilding = 1, Destroy = 2
    }
    public enum TowerInteractionMode
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
        [SerializeField] private GameObject ResourceCollectButtonPrefab;

        [Header("Building Collection Button Parents")]
        [SerializeField] private Transform GoldButtonParent;
        [SerializeField] private Transform ManaButtonParent;

        [Space(6)]
        [SerializeField] private Button[] SelectionButtons;
        [SerializeField] public int ButtonSelectionIndex = 0;
        private int previousButtonSelectionIndex = 0;

        private BuildingTypes CurrentBuildingType;
        private BuildingTypes PreviousBuildingType;

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
        [HideInInspector] public TowerInteractionMode CurrentInteractionMode;

        [SerializeField] private List<TowerCore> SpecialAbilityUnlockedTowerList;

        private void Start()
        {
            CurrentInteractionMode = TowerInteractionMode.UpgradeMode;

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
                BuildingPrefablist[i].GetComponent<ResourceBuildingCore>().enabled = false;
            }
        }

        private void Update()
        {
            if(CurrentInteractionMode == TowerInteractionMode.PlacementMode)
            {
                if(Input.GetMouseButton(0))
                {
                    var ray = camera.ScreenPointToRay(Input.mousePosition);
                    TowerHit = new RaycastHit();

                    if(Physics.Raycast(ray, out TowerHit, 100f))
                    {
                        hitPoint.x = Mathf.Ceil(TowerHit.point.x) - 0.5f;
                        hitPoint.z = Mathf.Ceil(TowerHit.point.z) - 0.5f;

                        if(TowerHit.collider.tag == "PlaceableGround")
                        {
                            if(CurrentBuildingType == BuildingTypes.Tower)
                            {
                                TowerPrefablist[TowerSelectedIndex].SetActive(true);
                                TowerPrefablist[TowerSelectedIndex].transform.position = hitPoint;
                            }
                            else
                            {
                                BuildingPrefablist[BuildingSelectedIndex].SetActive(true);
                                BuildingPrefablist[BuildingSelectedIndex].transform.position = hitPoint;
                            }
                        }
                    }
                }

                if(Input.GetMouseButtonUp(0))
                {
                    if(TowerHit.collider != null)
                    {
                        if(TowerHit.collider.tag == "PlaceableGround")
                        {
                            if(upgradeUI.PayGold(GetCostAmount()))
                            {
                                if(CurrentBuildingType == BuildingTypes.Tower)
                                {
                                    GameObject go = Instantiate(TowerList[TowerSelectedIndex], hitPoint, Quaternion.identity, TowerParent);
                                    go.GetComponent<TowerCore>().SetNewSprite();

                                    // Create UI
                                    switch(go.GetComponent<TowerCore>().towerType)
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

                                    go.GetComponent<TowerCore>().specialDirectionUI.SetActive(false);
                                }
                                else if(CurrentBuildingType == BuildingTypes.ResourceBuilding)
                                {
                                    Transform t;
                                    switch(BuildingSelectedIndex)
                                    {
                                        default:
                                            t = GoldButtonParent;
                                            break;
                                        case 1:
                                            t = ManaButtonParent;
                                            break;

                                    }
                                    GameObject go = Instantiate(BuildingList[BuildingSelectedIndex], hitPoint, Quaternion.identity, BuildingParent);

                                    GameObject bu = Instantiate(ResourceCollectButtonPrefab, t);
                                    bu.transform.localPosition = new Vector2(hitPoint.x, hitPoint.z);
                                    bu.SetActive(false);

                                    // Settings for the Button
                                    go.GetComponent<ResourceBuilding.ResourceBuildingCore>().button = bu.GetComponent<Button>();
                                    go.GetComponent<ResourceBuilding.ResourceBuildingCore>().resourceManager = resourceManager;
                                    go.GetComponent<ResourceBuilding.ResourceBuildingCore>().AddButtonListener();
                                }
                            }
                            else
                            {
                                notificationManager.OpenGoldNotification();
                            }
                        }
                        if(CurrentBuildingType == BuildingTypes.Destroy)
                        {
                            if(TowerHit.collider.tag == "Tower")
                            {
                                Destroy(TowerHit.collider.GetComponent<TowerCore>().specialDirectionUI);
                                Destroy(TowerHit.collider.gameObject);
                                CurrentBuildingType = PreviousBuildingType;
                                SetSelectedButtonAttributes(previousButtonSelectionIndex);
                                upgradeUI.PayGold(-5);
                            }
                            else if(TowerHit.collider.tag == "Building")
                            {
                                Destroy(TowerHit.collider.GetComponent<ResourceBuildingCore>().button);
                                Destroy(TowerHit.collider.gameObject);
                                CurrentBuildingType = PreviousBuildingType;
                                SetSelectedButtonAttributes(previousButtonSelectionIndex);
                                upgradeUI.PayGold(-10);
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
            else if(CurrentInteractionMode == TowerInteractionMode.UpgradeMode)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit _hit = new RaycastHit();

                    if(Physics.Raycast(ray, out _hit, 100f))
                    {
                        if(_hit.collider.tag == "Tower")
                        {
                            upgradeUI.currentTower = _hit.collider.GetComponent<TowerCore>();
                            upgradeUI.UpdateUIPosition(_hit.collider.transform.position.x, _hit.collider.transform.position.z);
                            upgradeUI.SpecialButton();
                            CurrentInteractionMode = TowerInteractionMode.None;
                        }
                    }
                }
            }
            else if(CurrentInteractionMode == TowerInteractionMode.SpecialAbilitySelectMode)
            {
                for(int i = 0; i < SpecialAbilityUnlockedTowerList.Count; i++)
                {
                    SpecialAbilityUnlockedTowerList[i].LookAt();
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
                                    CurrentInteractionMode = TowerInteractionMode.UpgradeMode;
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
            if(CurrentBuildingType == BuildingTypes.Tower)
            {
                switch(TowerSelectedIndex)
                {
                    default:
                        return itemCosts.ArcherTowerCost;
                    case 1:
                        return itemCosts.WizardTowerCost;
                }
            }
            else if(CurrentBuildingType == BuildingTypes.ResourceBuilding)
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

        public void OnSpecialMode(bool OnOrOff)
        {
            for(int i = 0; i < SpecialAbilityUnlockedTowerList.Count; i++)
            {
                SpecialAbilityUnlockedTowerList[i].specialDirectionUI.SetActive(OnOrOff);
            }
        }

        public void SetSelectedButtonAttributes(int _index)
        {
            // Reset the previous button
            SelectionButtons[ButtonSelectionIndex].transform.localScale = Vector2.one;

            // Set the new Button
            SelectionButtons[_index].transform.localScale = new Vector2(1.2f, 1.2f);

            // set variable to remember this button index for next time.
            ButtonSelectionIndex = _index;
        }

        public void SelectTower(int _i)
        {
            TowerSelectedIndex = _i;
            CurrentBuildingType = BuildingTypes.Tower;
            CurrentInteractionMode = TowerInteractionMode.PlacementMode;
        }

        public void SelectBuilding(int _i)
        {
            BuildingSelectedIndex = _i;
            CurrentBuildingType = BuildingTypes.ResourceBuilding;
            CurrentInteractionMode = TowerInteractionMode.PlacementMode;
        }

        public void SetDeleteBuilding()
        {
            previousButtonSelectionIndex = ButtonSelectionIndex;
            PreviousBuildingType = CurrentBuildingType;
            CurrentBuildingType = BuildingTypes.Destroy;
        }

        public void SetInteractionMode(int _i)
        {
            CurrentInteractionMode = (TowerInteractionMode)_i;
        }

        public void AddTowerToSpecialAbilityUnlockedList(TowerCore core)
        {
            if(SpecialAbilityUnlockedTowerList == null)
            {
                SpecialAbilityUnlockedTowerList = new List<TowerCore>();
            }

            SpecialAbilityUnlockedTowerList.Add(core);
        }

        #endregion
    }
}