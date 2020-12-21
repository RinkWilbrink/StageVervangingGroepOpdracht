using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Achievements;

public class AchievementListener : MonoBehaviour
{
    AchievementManager achievementManager;

    void Start()
    {
        achievementManager = AchievementManager.current;
        achievementManager.AchivementUnlocked += Unlock;
    }
    
    private void Unlock(int AchievementID)
    {
        achievementManager.UnlockAchievement(AchievementID, "Unlocked Achievement", "You did the thing!");
    }
}
