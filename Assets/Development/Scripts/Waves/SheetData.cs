using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SheetData", menuName = "Create SheetData")]
public class SheetData : ScriptableObject
{
    public int[] GoldReward;
    public int[] EnemyCount;
}

[Serializable]
public class WaveData
{
    public int goldReward;
    public int enemyCount;
}

[Serializable]
public class WaveDataCollection
{
    public WaveData[] jsonData;
}
