using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCore : MonoBehaviour
{
    // Variables
    [Header("Stats")]
    [SerializeField] private int PrimairyShootingTime;
    [SerializeField] private int PrimairyDamage;
    [Space(6)]
    [SerializeField] private int SecondairyShootingTime;
    [SerializeField] private int SecondairyDamage;

    [Header("a")]
    [SerializeField] private GameObject ShootOrigin;

    [Header("Targets")]
    [SerializeField] private GameObject CurrentTarget;

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PrimairyAttack();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            SecondairyAttack();
        }
    }

    private void HitManager()
    {
        RaycastHit hit;
        Vector3 direction = (CurrentTarget.transform.position - ShootOrigin.transform.position).normalized;

        Physics.Raycast(ShootOrigin.transform.position, direction, out hit, 100f);

        Debug.Log(hit.collider.name);

        Debug.DrawRay(ShootOrigin.transform.position, direction, Color.red, 1f);
    }

    private void TargetManager()
    {
        //GameObject newTarget;

        //return newTarget;
    }

    // Virtual functions for shooting and special abilities
    public virtual void PrimairyAttack() { Debug.Log("Core Primairy"); }
    public virtual void SecondairyAttack() { Debug.Log("Core Secondairy"); }
}