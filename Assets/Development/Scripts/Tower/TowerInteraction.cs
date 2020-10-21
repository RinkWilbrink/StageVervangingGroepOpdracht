using Boo.Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum BuildingTypes
{
    Tower = 0, ResourceBuilding = 1, Destroy
}

namespace Tower
{
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

        [SerializeField] private UINotificationManager notificationManager;

        [Space(4)]
        [SerializeField] private GameObject ResourceCollectButtonPrefab;
        [SerializeField] private Transform GoldButtonParent;
        [SerializeField] private Transform ManaButtonParent;

        [Space(6)]
        [SerializeField] private Button[] buildingButtons;
        [SerializeField] private int buildingButtonsIndex = 0;

        private BuildingTypes CurrentBuildingType;
        private BuildingTypes PreviousBuildingType;

        [Header("Script References")]
        [SerializeField] private UI.UpgradeUI upgradeUI;
        [SerializeField] private ResourceUIManager resourceManager;
        [SerializeField] private ItemCost itemCosts;

        // private variables
        private Vector3 hitPoint = Vector3.zero;
        private RaycastHit hit;

        // Set Current tower interaction mode
        [HideInInspector] public TowerInteractionMode CurrentInteractionMode;

        [HideInInspector] private List<TowerCore> SpecialAbilityUnlockedTowerList;

        private void Start()
        {
            // Create Template Towers
            TowerPrefablist = new GameObject[TowerList.Length];
            for (int i = 0; i < TowerList.Length; i++)
            {
                TowerPrefablist[i] = Instantiate(TowerList[i], Vector3.zero, Quaternion.identity, transform);
                TowerPrefablist[i].SetActive(false);
                TowerPrefablist[i].name = TowerPrefablist[i].name.Replace("Clone", "Template");
                TowerPrefablist[i].GetComponentInChildren<BoxCollider>().enabled = false;
                TowerPrefablist[i].GetComponentInChildren<Tower.TowerCore>().enabled = false;
            }

            // Create Template Buildings
            BuildingPrefablist = new GameObject[BuildingList.Length];
            for (int i = 0; i < TowerList.Length; i++)
            {
                BuildingPrefablist[i] = Instantiate(BuildingList[i], Vector3.zero, Quaternion.identity, transform);
                BuildingPrefablist[i].SetActive(false);
                BuildingPrefablist[i].name = BuildingPrefablist[i].name.Replace("Clone", "Template");
                BuildingPrefablist[i].GetComponentInChildren<BoxCollider>().enabled = false;
                BuildingPrefablist[i].GetComponent<ResourceBuilding.ResourceBuildingCore>().enabled = false;
            }
        }

        private void Update()
        {
            if(CurrentInteractionMode == TowerInteractionMode.PlacementMode)
            {
                if (Input.GetMouseButton(0))
                {
                    var ray = camera.ScreenPointToRay(Input.mousePosition);

                    hit = new RaycastHit();

                    if (Physics.Raycast(ray, out hit, 100f))
                    {
                        hitPoint.x = Mathf.Ceil(hit.point.x) - 0.5f;
                        hitPoint.z = Mathf.Ceil(hit.point.z) - 0.5f;

                        if (hit.collider.tag == "PlaceableGround")
                        {
                            if (CurrentBuildingType == BuildingTypes.Tower)
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

                if (Input.GetMouseButtonUp(0))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.tag == "PlaceableGround")
                        {
                            if (upgradeUI.PayGold(GetCostAmount()))
                            {
                                if (CurrentBuildingType == BuildingTypes.Tower)
                                {
                                    GameObject go = Instantiate(TowerList[TowerSelectedIndex], hitPoint, Quaternion.identity, TowerParent);
                                }
                                else if (CurrentBuildingType == BuildingTypes.ResourceBuilding)
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
                        else if (CurrentBuildingType == BuildingTypes.Destroy)
                        {
                            if (hit.collider.tag == "Tower" || hit.collider.tag == "Building")
                            {
                                Destroy(hit.collider.gameObject);
                                CurrentBuildingType = PreviousBuildingType;
                                upgradeUI.PayGold(-5);
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
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                    RaycastHit _hit = new RaycastHit();

                    if (Physics.Raycast(ray, out _hit, 100f))
                    {
                        if (_hit.collider.tag == "Tower")
                        {
                            upgradeUI.currentTower = _hit.collider.GetComponent<TowerCore>();
                            upgradeUI.UpdateUIPosition(_hit.collider.transform.position.x, _hit.collider.transform.position.z);
                            CurrentInteractionMode = TowerInteractionMode.None;
                        }
                    }
                }
            }
            else if(CurrentInteractionMode == TowerInteractionMode.SpecialAbilitySelectMode)
            {
                for (int i = 0; i < SpecialAbilityUnlockedTowerList.Count; i++)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                        RaycastHit _hit = new RaycastHit();

                        if (Physics.Raycast(ray, out _hit, 100f))
                        {
                            if (_hit.collider.tag == "Tower")
                            {
                                hit.collider.GetComponent<Tower.TowerCore>().StartSecondairyAttack();

                                CurrentInteractionMode = TowerInteractionMode.UpgradeMode;
                            }
                        }
                    }
                }
            }
        }

        private int GetCostAmount()
        {
            if (CurrentBuildingType == BuildingTypes.Tower)
            {
                switch (TowerSelectedIndex)
                {
                    default:
                        return itemCosts.ArcherTowerCost;
                    case 1:
                        return itemCosts.WizardTowerCost;
                }
            }
            else if (CurrentBuildingType == BuildingTypes.ResourceBuilding)
            {
                switch (BuildingSelectedIndex)
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

        public void SetSelectedButtonAttributes(int _index)
        {
            // Reset the previous button
            buildingButtons[buildingButtonsIndex].transform.localScale = Vector2.one;

            // Set the new Button
            buildingButtons[_index].transform.localScale = new Vector2(1.1f, 1.1f);

            // set variable to remember this button index for next time.
            buildingButtonsIndex = _index;
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