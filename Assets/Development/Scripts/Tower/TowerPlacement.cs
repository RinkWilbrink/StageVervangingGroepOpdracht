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
    [SerializeField] private UpgradeUI upgradeUI;


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
    }

    private void Update()
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

                Debug.Log(hitPoint);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(hit.collider.tag == "PlaceableGround")
            {
                //Debug.Log(hitPoint);
                GameObject go = Instantiate(TowerList[TowerSelectedIndex], hitPoint, Quaternion.identity, TowerParent);
            }
            if(hit.collider.tag == "Tower")
            {
                //Debug.Log("Cool");
                //upgradeUI.UpdateUIPosition(hitPoint.x, hitPoint.z);
            }

            Prefablist[TowerSelectedIndex].SetActive(false);
            Prefablist[TowerSelectedIndex].transform.position = Vector3.zero;
        }
    }

    public void OnButtonClick(int _i)
    {
        TowerSelectedIndex = _i;
    }
}