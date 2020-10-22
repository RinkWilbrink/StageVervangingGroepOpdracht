using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThousandCranes : MonoBehaviour
{
    [SerializeField] private float speedDebuff = 2f;
    public float speedDebuffTime = 10f;

    private void Update() {
        if ( Input.GetKeyDown(KeyCode.J) ) {
            ThousandCranesAbility();
        }
    }

    public void ThousandCranesAbility() {
        //Collider[] enemies = Physics.OverlapSphere(Vector3.one, 100);
        EnemyUnit[] enemies = FindObjectsOfType<EnemyUnit>();
        for ( int i = 0; i < enemies.Length; i++ ) {
            //if ( enemies[i].GetComponent<EnemyUnit>() ) {
            //EnemyUnit EU = enemies[i].GetComponent<EnemyUnit>();
            enemies[i].SlowDown(speedDebuff, speedDebuffTime);
        }
    }
}
