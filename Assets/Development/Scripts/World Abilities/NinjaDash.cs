using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NinjaDash : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private Camera mainCam;
    private GameObject ninja;
    public event Action Reset;
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject ninjaSprite;
    [SerializeField] private float dragRange = 7;
    [SerializeField] private int damage = 20;
    private float camZ;

    private WorldAbilities worldAbilities;

    private void Start() {
        //line = GetComponent<LineRenderer>();
        if ( worldAbilities == null )
            worldAbilities = FindObjectOfType<WorldAbilities>();

        line.sortingOrder = 1;
        line.material = new Material(Shader.Find("Sprites/Default"));
        //line.material.color = Color.red;
        line.SetVertexCount(2);

        mainCam = Camera.main;
        camZ = mainCam.transform.position.y;

        ninja = Instantiate(ninjaSprite, startPos, Quaternion.Euler(90, 0, 0));
        ninja.transform.SetParent(transform);
        ninja.SetActive(false);

        if ( gameObject.active )
            gameObject.SetActive(false);

        //line.gameObject.SetActive(false);
        //line.enabled = false;
    }

    bool stopTest = false;
    bool moveNinja = false;
    private void Update() {
        if ( Input.GetMouseButtonDown(0) && !moveNinja && !IsMouseOnUI() ) {
            print("Down");
            line.enabled = true;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = camZ;
            startPos = mainCam.ScreenToWorldPoint(mousePos);
            //startPos.y = 0;
            line.SetPosition(0, startPos);

            ninja.transform.position = startPos;
            ninja.SetActive(true);
        }

        if ( Input.GetMouseButton(0) && !moveNinja && !IsMouseOnUI() ) {
            print("Drag");

            Vector3 dist = startPos - endPos;

            if ( !stopTest ) {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = camZ;
                endPos = mainCam.ScreenToWorldPoint(mousePos);
                //endPos.y = 0;
                line.SetPosition(1, endPos);
            }

            //Debug.Log("StartPos: " + startPos + " / EndPos: " + endPos);
            if ( Vector3.Distance(startPos, endPos) > dragRange ) {
                stopTest = true;
            }
        }

        if ( Input.GetMouseButtonUp(0) && !moveNinja ) {
            print("Up");

            moveNinja = true;
        }

        if ( moveNinja ) {
            ninja.transform.position = Vector3.MoveTowards(ninja.transform.position, endPos, 20 * Time.deltaTime);

            if ( Vector3.Distance(ninja.transform.position, endPos) < .1f ) {
                DamageEnemies();

                line.enabled = false;
                stopTest = false;
                moveNinja = false;
                ninja.SetActive(false);

                worldAbilities.ResetNinjaDash();
            }
        }
    }

    private void DamageEnemies() {
        float thickness = 1f;

        RaycastHit[] hits;

        hits = Physics.SphereCastAll(startPos, thickness, endPos - startPos);

        for ( int i = 0; i < hits.Length; i++ ) {
            if ( hits[i].transform.GetComponent<EnemyUnit>() ) {
                hits[i].transform.GetComponent<EnemyUnit>().TakeDamage(damage);
                Debug.Log("An Enemy is hit");
            }
        }
    }

    private bool IsMouseOnUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}