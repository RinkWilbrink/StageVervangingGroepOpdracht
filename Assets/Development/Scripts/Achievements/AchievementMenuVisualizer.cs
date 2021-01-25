using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenuVisualizer : MonoBehaviour
{
    [SerializeField] int m_AchievementID;
    [SerializeField] Sprite m_LockedSprite;

    private void Start()
    {
        if(DataManager._AchivementCheck[m_AchievementID] == false)
        {
            GetComponent<Image>().sprite = m_LockedSprite;
        }
    }
}
