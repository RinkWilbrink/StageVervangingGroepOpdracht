using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThousandCranes : MonoBehaviour
{
    public void ThousandCranesAbility() {
        Collider[] enemies = Physics.OverlapSphere(Vector3.one, 100);

        for ( int i = 0; i < enemies.Length; i++ ) {
            if ( enemies[i].GetComponent<EnemyUnit>() ) {
                EnemyUnit EU = enemies[i].GetComponent<EnemyUnit>();

            }

        }
    }
}
