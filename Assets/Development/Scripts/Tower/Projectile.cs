using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class Projectile : MonoBehaviour
    {
        private GameObject Target;

        public float speed = 70f;

        public void seek(GameObject _target)
        {
            Target = _target;
            Debug.Log(_target);
        }

        // Update is called once per frame
        void Update()
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 dir = Target.transform.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }

        void HitTarget()
        {
            Debug.Log("hit");
        }
    }

}
