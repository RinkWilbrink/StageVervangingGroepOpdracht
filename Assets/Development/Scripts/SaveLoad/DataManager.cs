using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static SaveLoad _SaveLoad;

    public static int _LastLevelBeaten;

    public static int _EnemiesKilled;
    public static int _ResourcesGathered;
    public static int _TowersPlaced;

    public static void ResourcesGained(int amount)
    {
        _ResourcesGathered += amount;
    }

    public static void EnemySlayed()
    {
        _EnemiesKilled += 1;
    }

    public static void TowerPlaced()
    {
        _TowersPlaced += 1;
    }

    public static void TowerRemoved()
    {
        _TowersPlaced -= 1;
    }

    public static void LevelComplete(int levelNumber)
    {
        if (_LastLevelBeaten < levelNumber)
        {
            _LastLevelBeaten = levelNumber;
        }

        _SaveLoad.SaveData();
    }

    public static void AchievementCheck(int achievementID, int amount)
    {

    }
}
