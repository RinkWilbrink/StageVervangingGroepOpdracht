using UnityEngine;

public class ThousandCranes : MonoBehaviour
{
    [SerializeField] private AudioManagement audioManagement;
    [SerializeField] private AudioClip flockOfCranesAudio;
    [SerializeField] private float speedDebuff = 2f;
    public float speedDebuffTime = 10f;

    public void ThousandCranesAbility()
    {
        audioManagement.PlayAudioClip(flockOfCranesAudio, AudioMixerGroups.SFX);
        EnemyUnit[] enemies = FindObjectsOfType<EnemyUnit>();
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SlowDown(speedDebuff, speedDebuffTime);
        }
    }
}
