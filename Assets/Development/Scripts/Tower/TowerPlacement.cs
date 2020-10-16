using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum BuildingTypes
{
    Tower = 0, ResourceBuilding = 1
}

/*
 * TODO:
 * 
 * 15 OCT: Pay to place towers
 * 
*/

public class TowerPlacement : MonoBehaviour
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

    private bool DestroyTowerMode = false;

    [SerializeField] private UINotificationManager notificationManager;

    [Space(4)]
    [SerializeField] private GameObject ResourceCollectButtonPrefab;
    [SerializeField] private Transform GoldButtonParent;
    [SerializeField] private Transform ManaButtonParent;

    private BuildingTypes CurrentBuildingType;

    [Header("Script References")]
    [SerializeField] private UI.UpgradeUI upgradeUI;
    [SerializeField] private ResourceUIManager resourceManager;
    [SerializeField] private ItemCost itemCosts;

    [HideInInspector] public bool CanRaycast = true;
    [HideInInspector] public bool CanPlaceTowers = true;

    // private variables
    private Vector3 hitPoint = Vector3.zero;
    private RaycastHit hit;

    private void Start()
    {
        // Create Template Towers
        TowerPrefablist = new GameObject[TowerList.Length];
        for(int i = 0; i < TowerList.Length; i++)
        {
            TowerPrefablist[i] = Instantiate(TowerList[i], Vector3.zero, Quaternion.identity, transform);
            TowerPrefablist[i].SetActive(false);
            TowerPrefablist[i].name = TowerPrefablist[i].name.Replace("Clone", "Template");
            TowerPrefablist[i].GetComponentInChildren<BoxCollider>().enabled = false;
            TowerPrefablist[i].GetComponentInChildren<Tower.TowerCore>().enabled = false;
        }

        // Create Template Buildings
        BuildingPrefablist = new GameObject[BuildingList.Length];
        for(int i = 0; i < TowerList.Length; i++)
        {
            BuildingPrefablist[i] = Instantiate(BuildingList[i], Vector3.zero, Quaternion.identity, transform);
            BuildingPrefablist[i].SetActive(false);
            BuildingPrefablist[i].name = BuildingPrefablist[i].name.Replace("Clone", "Template");
            BuildingPrefablist[i].GetComponentInChildren<BoxCollider>().enabled = false;
            BuildingPrefablist[i].GetComponent<ResourceBuilding.ResourceBuildingCore>().enabled = false;
        }

        // Set Booleans for placing towers and buildings
        CanRaycast = true;
        CanPlaceTowers = false;
        DestroyTowerMode = false;
    }

    private void Update()
    {
        if(CanRaycast)
        {
            if(CanPlaceTowers)
            {
                if(Input.GetMouseButton(0))
                {
                    var ray = camera.ScreenPointToRay(Input.mousePosition);

                    hit = new RaycastHit();

                    if(Physics.Raycast(ray, out hit, 100f))
                    {
                        hitPoint.x = Mathf.Ceil(hit.point.x) - 0.5f;
                        hitPoint.z = Mathf.Ceil(hit.point.z) - 0.5f;

                        if(hit.collider.tag == "PlaceableGround")
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
                    if(hit.collider != null)
                    {
                        if(hit.collider.tag == "PlaceableGround")
                        {
                            if(upgradeUI.PayGold(GetCostAmount()))
                            {
                                if(CurrentBuildingType == BuildingTypes.Tower)
                                {
                                    GameObject go = Instantiate(TowerList[TowerSelectedIndex], hitPoint, Quaternion.identity, TowerParent);
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
                        else if(DestroyTowerMode == true)
                        {
                            try
                            {
                                Destroy(hit.collider.gameObject);
                                DestroyTowerMode = false;
                            }
                            catch
                            {
                                Debug.Log("Couldnt destroy tower!");
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
            else
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                    RaycastHit _hit = new RaycastHit();

                    if(Physics.Raycast(ray, out _hit, 100f))
                    {
                        if(_hit.collider.tag == "Tower")
                        {
                            upgradeUI.currentTower = _hit.collider.GetComponent<Tower.TowerCore>();
                            upgradeUI.UpdateUIPosition(_hit.point.x, _hit.point.z);
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

    public void SelectTower(int _i)
    {
        TowerSelectedIndex = _i;
        CurrentBuildingType = BuildingTypes.Tower;
        CanPlaceTowers = true;
    }

    public void SelectBuilding(int _i)
    {
        BuildingSelectedIndex = _i;
        CurrentBuildingType = BuildingTypes.ResourceBuilding;
        CanPlaceTowers = true;
    }

    public void SetDeleteBuilding()
    {
        DestroyTowerMode = true;
    }

    public void SetCanPlaceTowers(bool _x)
    {
        CanRaycast = _x;
    }

    public void ResetUpgradeUI(bool _x)
    {
        SetCanPlaceTowers(_x);
    }

    #endregion
}