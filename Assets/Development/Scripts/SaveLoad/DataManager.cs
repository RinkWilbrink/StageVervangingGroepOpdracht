using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Achievements;

public static class DataManager
{
    public static SaveLoad _SaveLoad;

    public static int _LastLevelBeaten;

    public static int _EnemiesKilled;
    public static int _GoldCollected;
    public static int _ManaCollected;
    public static int _TowersPlaced;

    public static bool[] _AchivementCheck = new bool[5];

    public static void EnemySlayed()
    {
        _EnemiesKilled += 1;

        if (_EnemiesKilled >= 1 && _AchivementCheck[0] == false)
        {
            AchievementManager.current.UnlockAchievement("Enemy's Slayed", "1 Enemy Slayed");
            _AchivementCheck[0] = true;
        }
    }

    public static void ResourcesGained(int amount, bool gold)
    {
        if (gold != true)
        {
            _ManaCollected += amount;
        }
        else
        {
            _GoldCollected += amount;
        }

        if (_ManaCollected >= 1 && _AchivementCheck[1] == false)
        {
            AchievementManager.current.UnlockAchievement("Gold Gathered", "Collect 1 gold");
            _AchivementCheck[1] = true;
        }

        if (_GoldCollected >= 1 && _AchivementCheck[4] == false)
        {
            AchievementManager.current.UnlockAchievement("Mana Gathered", "Collect 1 mana");
            _AchivementCheck[4] = true;
        }
    }

    public static void TowerPlaced()
    {
        _TowersPlaced += 1;

        if (_TowersPlaced >= 1 && _AchivementCheck[2] == false)
        {
            AchievementManager.current.UnlockAchievement("Tower Place", "Place 1 Tower");
            _AchivementCheck[2] = true;
        }
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

        if (_LastLevelBeaten == 3 && _AchivementCheck[3] == false)
        {
            AchievementManager.current.UnlockAchievement("Level 3 Beaten", "You have completed the all 3 levels!");
            _AchivementCheck[3] = true;
        }

        _SaveLoad.SaveData();
    }
}
