using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    // Variables
    [SerializeField] private RectTransform UpgradePanel;

    void Start()
    {
        UpgradePanel.gameObject.SetActive(false);
    }

    public void UpdateUIPosition(float _x, float _y)
    {
        UpgradePanel.gameObject.SetActive(true);

        UpgradePanel.anchoredPosition = new Vector3(_x, _y);
        UpgradePanel.localScale = Vector3.one;
    }
}

