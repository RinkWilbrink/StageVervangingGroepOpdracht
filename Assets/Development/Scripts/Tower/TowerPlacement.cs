using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GridBlockType
{
    Ground = 0, Path, Tower, MainTower = 3
}

public class TowerPlacement : MonoBehaviour
{
    // Variables
    [SerializeField] private Camera camera;
    [Space(6)]
    [SerializeField] private Transform TowerParent;
    [SerializeField] private GameObject[] TowerList;
    private int TowerSelectedIndex;
    private GameObject[] Prefablist;

    [Space(6)]
    [SerializeField] private UI.UpgradeUI upgradeUI;

    [HideInInspector] public bool CanRaycast = true;
    private bool CanPlaceTowers = true;
    private bool PlacingTowers = true;

    // private variables
    private GridBlockType[,] grid = new GridBlockType[20, 20];
    private Vector3 hitPoint = Vector3.zero;
    private RaycastHit hit;

    private void Start()
    {
        Prefablist = new GameObject[TowerList.Length];
        for(int i = 0; i < TowerList.Length; i++)
        {
            Prefablist[i] = Instantiate(TowerList[i], Vector3.zero, Quaternion.identity, transform);
            Prefablist[i].SetActive(false);
            Prefablist[i].name = Prefablist[i].name.Replace("Clone", "Template");
            Prefablist[i].GetComponentInChildren<BoxCollider>().enabled = false;
        }

        CanRaycast = true;
        CanPlaceTowers = false;
        PlacingTowers = true;
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
                            Prefablist[TowerSelectedIndex].SetActive(true);
                            Prefablist[TowerSelectedIndex].transform.position = hitPoint;
                        }
                    }
                }

                if(Input.GetMouseButtonUp(0))
                {
                    if(hit.collider != null)
                    {
                        if(PlacingTowers)
                        {
                            if(hit.collider.tag == "PlaceableGround")
                            {
                                //Debug.Log(hitPoint);
                                GameObject go = Instantiate(TowerList[TowerSelectedIndex], hitPoint, Quaternion.identity, TowerParent);

                                CanPlaceTowers = false;
                                PlacingTowers = false;
                            }
                        }
                        else
                        {
                            PlacingTowers = true;
                        }
                    }

                    Prefablist[TowerSelectedIndex].SetActive(false);
                    Prefablist[TowerSelectedIndex].transform.position = Vector3.zero;
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

                            Debug.LogFormat("BRUH!", _hit.collider.GetComponent<Tower.TowerCore>().name);

                            upgradeUI.UpdateUIPosition(_hit.point.x, _hit.point.z);
                        }
                    }
                }
            }
        }
    }

    public void OnButtonClick(int _i)
    {
        TowerSelectedIndex = _i;
        CanPlaceTowers = true;
    }

    public void SetCanPlaceTowers(bool _x)
    {
        //Debug.Log("BANAAAN!");
        CanRaycast = _x;
    }

    public void ResetUpgradeUI(bool _x)
    {
        SetCanPlaceTowers(_x);
    }
}