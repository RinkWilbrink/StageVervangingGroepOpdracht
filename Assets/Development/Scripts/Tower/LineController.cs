using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private Transform target1;
    private Transform target2;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, target1.transform.position);
        lineRenderer.SetPosition(1, target2.transform.position);
    }

    public void AssignTarget(Transform startPosition, Transform endPosition)
    {
        lineRenderer.positionCount = 2;

        target1 = startPosition;
        target2 = endPosition;
    }
}
