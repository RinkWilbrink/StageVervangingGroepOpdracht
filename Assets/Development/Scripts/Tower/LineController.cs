using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class LineController : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private LightningTargetCache target1;
        private LightningTargetCache target2;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            lineRenderer.SetPosition(0, target1.GetVector());
            lineRenderer.SetPosition(1, target2.GetVector());
        }

        public void AssignTarget(LightningTargetCache startPosition, LightningTargetCache endPosition)
        {
            lineRenderer.positionCount = 2;

            target1 = startPosition;
            target2 = endPosition;
        }
    }
}


