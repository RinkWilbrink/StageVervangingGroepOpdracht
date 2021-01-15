using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] Text m_EnemiesSlayedAmount, m_TowersPlacedAmount, m_GoldCollectedAmount, m_ManaGainedAmount;

    void Start()
    {
        m_EnemiesSlayedAmount.text = DataManager._EnemiesKilled.ToString();
        m_TowersPlacedAmount.text = DataManager._TowersPlaced.ToString();
        m_ManaGainedAmount.text = DataManager._ManaCollected.ToString();
        m_GoldCollectedAmount.text = DataManager._GoldCollected.ToString();
    }
}
