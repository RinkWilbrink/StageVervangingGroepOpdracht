using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFirePattern : MonoBehaviour
{
    private float angle = 0f;

    int length = 40;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fire());
    }

    public IEnumerator Fire()
    {        
        for (int i = 0; i < length; i++)
        {
            float bulDirX =  Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirZ =  Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, 0f, bulDirZ);
            Vector2 bulDir = bulMoveVector.normalized;

            GameObject bul = GenericPool.bulletPoolInstanse.GetBullet();
            bul.transform.position = transform.position;
            //bul.transform.rotation = transform.rotation;
            //bul.transform.up = bulMoveVector;
            bul.SetActive(true);
            bul.GetComponent<GenericBullet>().SetMoveDirection(bulDir);
            angle += 10f;
            yield return new WaitForSeconds(0.01f);

        }
    }
}
