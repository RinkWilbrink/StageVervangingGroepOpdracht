using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFirePattern : MonoBehaviour
{
    private float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("Fire", 0f, 0.01f);
    }

    private void Fire()
    {
        float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
        float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

        Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
        Vector2 bulDir = (bulMoveVector - transform.position).normalized;

        GameObject bul = GenericPool.bulletPoolInstanse.GetBullet();
        bul.transform.position = transform.position;
        bul.transform.rotation = transform.rotation;
        bul.SetActive(true);
        bul.GetComponent<GenericBullet>().SetMoveDirection(bulDir);

        angle += 10f;
    }
}
