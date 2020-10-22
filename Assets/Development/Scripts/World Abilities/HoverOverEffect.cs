using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOverEffect : MonoBehaviour
{
    [SerializeField, Range(1, 3)] private float maxScale = 1.2f;
    [SerializeField, Range(0, 10)] private float speedEffect = 1.77f;
    private bool isTouched = false;

    private void Update() {
        if ( transform.localScale.x < maxScale && isTouched )
            transform.localScale += new Vector3(speedEffect * Time.deltaTime, speedEffect * Time.deltaTime, speedEffect * Time.deltaTime);
        else if ( transform.localScale.x > 1f && !isTouched )
            transform.localScale -= new Vector3(speedEffect * Time.deltaTime, speedEffect * Time.deltaTime, speedEffect * Time.deltaTime);
    }

    public void IncreaseSize() {
        isTouched = true;
    }

    public void DecreaseSize() {
        isTouched = false;
    }
}