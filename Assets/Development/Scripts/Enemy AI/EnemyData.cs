using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Values")]
public class EnemyData : ScriptableObject
{
    public int health;
    public float speed;
    public int goldReward;
    public int attackDamage;
}
