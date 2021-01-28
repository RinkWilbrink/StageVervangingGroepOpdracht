using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAbilitie : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private float damage;
    // Update is called once per frame
    void Update()
    {               
         timer -= Time.deltaTime;  
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Enemy")
        {
            if (timer <= 0)
            {
                other.gameObject.GetComponent<EnemyUnit>().TakeDamage(damage, Tower.TowerType.CannonTower);
                timer = 0.5f;
            }
        }
    }

}
