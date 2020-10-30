using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireworkRocket : MonoBehaviour
{
    [SerializeField] private float explosionSize;
    [SerializeField] private int damage;
    [SerializeField] private GameObject fireworkSprite;
    [SerializeField] private GameObject explosionSprite;
    private GameObject fireworkRocket;
    private GameObject fireworkExplosion;
    private Camera mainCam;
    private float camZ;
    private WorldAbilities worldAbilities;

    private void Start() {
        mainCam = Camera.main;
        camZ = mainCam.transform.position.y;

        worldAbilities = FindObjectOfType<WorldAbilities>();

        fireworkRocket = Instantiate(fireworkSprite, Vector3.zero, Quaternion.Euler(90, 0, 0));
        fireworkRocket.transform.localScale = Vector3.one * 2f;
        fireworkRocket.transform.SetParent(transform);
        fireworkRocket.SetActive(false);

        fireworkExplosion = Instantiate(explosionSprite, Vector3.zero, Quaternion.Euler(90, 0, 0));
        fireworkExplosion.transform.SetParent(transform);
        fireworkExplosion.SetActive(false);

        if ( gameObject.active )
            gameObject.SetActive(false);
    }

    Vector3 explosionPos;
    private bool rocketLaunched = false;
    private void Update() {
        if ( Input.GetMouseButtonUp(0) && !rocketLaunched ) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = camZ;

            explosionPos = mainCam.ScreenToWorldPoint(mousePos);

            fireworkRocket.transform.position = new Vector3(explosionPos.x, .5f, explosionPos.z);
            fireworkExplosion.transform.position = explosionPos;
            fireworkRocket.SetActive(true);
            rocketLaunched = true;
        }

        if ( rocketLaunched ) {
            if ( fireworkRocket.transform.localScale.x >= .1f )
                fireworkRocket.transform.localScale -= new Vector3(1.6f * Time.deltaTime, 1.6f * Time.deltaTime);

            if ( fireworkRocket.transform.localScale.x < .6f ) {
                fireworkRocket.SetActive(false);

                fireworkExplosion.SetActive(true);
            }

            if ( fireworkRocket.transform.localScale.x < .4f ) {
                Collider[] enemies = Physics.OverlapSphere(explosionPos, explosionSize);

                for ( int i = 0; i < enemies.Length; i++ ) {
                    if ( enemies[i].GetComponent<EnemyUnit>() ) {
                        enemies[i].GetComponent<EnemyUnit>().TakeDamage(damage);
                    }
                }
            }
            if ( fireworkRocket.transform.localScale.x < .2f ) {
                fireworkRocket.transform.localScale = Vector3.one * 2f;
                fireworkExplosion.SetActive(false);
                rocketLaunched = false;
                worldAbilities.ResetFireworkRocket();
            }
        }
    }
}
