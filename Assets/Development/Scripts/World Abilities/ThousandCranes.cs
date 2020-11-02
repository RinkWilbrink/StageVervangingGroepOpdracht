using UnityEngine;

public class ThousandCranes : MonoBehaviour
{
    [SerializeField] private float speedDebuff = 2f;
    public float speedDebuffTime = 10f;

    public void ThousandCranesAbility()
    {
        EnemyUnit[] enemies = FindObjectsOfType<EnemyUnit>();
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SlowDown(speedDebuff, speedDebuffTime);
        }
    }
}
