using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class Projectile : MonoBehaviour
    {
        private GameObject Target;

        public float speed = 20f;

        public float smoothTime = 0.3f;

        private Vector3 velocity = Vector3.zero;
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

            //transform.position = Vector3.SmoothDamp(transform. position, dir, ref velocity, smoothTime);

            
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            
        }

        void HitTarget()
        {

            Debug.Log("hit");
            Destroy(gameObject);
        }
    }

}
