using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyChallenge : MonoBehaviour
{
    public ChallengeType challengeType;
    public string challengeName = "";
    public string challengeDescription = "Kill 20 enemies";
    public int progress = 0;
    public int maxProgress = 20;
}

public enum ChallengeType
{
    KillAnyEnemy = 0,
    KillSpecialEnemy,
    CompleteWaves
}
