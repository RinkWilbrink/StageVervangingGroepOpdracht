using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBullet : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;
    [SerializeField] private float damage;
    private void OnEnable()
    {
        Invoke("Destroy", 3f);   
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
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
        if (other.tag == "Enemy")
        {
            Debug.Log(damage);
            other.GetComponent<EnemyUnit>().TakeDamage(damage, Tower.TowerType.ArcherTower);
        }
    }
}
