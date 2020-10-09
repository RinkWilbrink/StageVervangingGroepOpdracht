using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    // Variables
    [SerializeField] private RectTransform rect;

    void Start()
    {
        try
        {
            rect.localScale = Vector3.zero;
        }
        catch
        {
            Debug.LogErrorFormat("rect is NULL");
        }
    }

    public void UpdateUIPosition(float _x, float _y)
    {
        rect.anchoredPosition = new Vector3(_x, _y);
        rect.localScale = Vector3.one;
    }
}

