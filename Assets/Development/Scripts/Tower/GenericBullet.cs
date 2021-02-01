using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBullet : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float ShootingRange = 0f;
    private Rigidbody rb;
    private void OnEnable()
    {
        Invoke("Destroy", 3f);   
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10f;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position += moveDirection * moveSpeed * Time.fixedDeltaTime;        
    }

    public void SetMoveDirection(Vector3 dir)
    {
        moveDirection = dir;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("take damage");
            Debug.Log(damage);
            other.GetComponent<EnemyUnit>().TakeDamage(damage, Tower.TowerType.ArcherTower);
        } else
        {
            Debug.Log(other.gameObject);
        }
    }
}
