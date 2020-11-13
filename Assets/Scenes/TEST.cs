using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    [SerializeField] private GameObject go1, go2;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            float Distance1 = Mathf.Sqrt((go1.transform.position - go2.transform.position).sqrMagnitude);

            float Distance2 = (go1.transform.position - go2.transform.position);

            Debug.LogFormat("D1: {0} | D2: {1}", Distance1, Distance2);
        }
    }
}
