using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject[] TowerList;

    // private variables
    private GridBlockType[,] grid = new GridBlockType[20, 20];
    private Vector3 hitPoint = Vector3.zero;

    private int cool;

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if(Physics.Raycast(ray, out hit, 100f))
            {
                hitPoint.x = Mathf.Ceil(hit.point.x) - 0.5f;
                hitPoint.z = Mathf.Ceil(hit.point.z) - 0.5f;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log(hitPoint);
            GameObject go = Instantiate(TowerList[cool], hitPoint, Quaternion.identity, gameObject.transform);
        }
    }

    public void OnButtonClick(int _i)
    {
        cool = _i;
    }
}
