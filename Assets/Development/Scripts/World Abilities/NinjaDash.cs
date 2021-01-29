using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NinjaDash : MonoBehaviour
{
    [SerializeField] private ResourceUIManager resourceUIManager;
    private Vector3 startPos;
    private Vector3 dragPos;
    private Vector3 endPos;
    private Camera mainCam;
    private GameObject ninja;
    public event Action Reset;
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject ninjaSprite;
    [SerializeField] private float dragRange = 7;
    [SerializeField] private int damage = 20;

    [SerializeField] private AudioManagement audioManagement;
    [SerializeField] private AudioClip summonSound;
    [SerializeField] private AudioClip slashSound;
    private float camZ;

    private WorldAbilities worldAbilities;

    private void Start() {
        if ( worldAbilities == null )
            worldAbilities = FindObjectOfType<WorldAbilities>();

        line.sortingOrder = 1;
        line.SetVertexCount(2);

        mainCam = Camera.main;
        camZ = mainCam.transform.position.y;

        ninja = Instantiate(ninjaSprite, startPos, Quaternion.Euler(90, 0, 0));
        ninja.transform.SetParent(transform);
        ninja.SetActive(false);

        if ( gameObject.active )
            gameObject.SetActive(false);
    }

    bool stopTest = false;
    bool moveNinja = false;
    private void Update() {
        if ( Input.GetMouseButtonDown(0) && !moveNinja && !IsMouseOnUI() ) {
            //print("Down");
            line.enabled = true;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = camZ;
            startPos = mainCam.ScreenToWorldPoint(mousePos);
            line.SetPosition(0, startPos);

            ninja.transform.position = startPos;
            ninja.SetActive(true);
            audioManagement.PlayAudioClip(summonSound, AudioMixerGroups.SFX);
            GameController.Mana -= worldAbilities.ninjaDashManaCost;
            resourceUIManager.UpdateResourceUI();
            worldAbilities.ninjaDashButton.interactable = false;
            worldAbilities.ninjaDashTimer = 0f;

        }

        if ( Input.GetMouseButton(0) && !moveNinja && !IsMouseOnUI() ) {

            //Vector3 dist = startPos - endPos;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = camZ;
            dragPos = mainCam.ScreenToWorldPoint(mousePos);
            print(dragPos);
            if ( !stopTest ) {
                endPos = dragPos;
                line.SetPosition(1, endPos);
            }

            Debug.DrawLine(startPos, endPos, Color.cyan);

            if ( Vector3.Distance(startPos, dragPos) > dragRange ) {
                stopTest = true;
            } else {
                stopTest = false;
            }
        }

        if ( Input.GetMouseButtonUp(0) && !moveNinja ) {

            moveNinja = true;
            audioManagement.PlayAudioClip(slashSound, AudioMixerGroups.SFX);
        }

        if ( moveNinja ) {
            ninja.transform.position = Vector3.MoveTowards(ninja.transform.position, endPos, 20 * Time.deltaTime);

            if ( Vector3.Distance(ninja.transform.position, endPos) < .1f ) {
                DamageEnemies();

                line.enabled = false;
                stopTest = false;
                moveNinja = false;
                StartCoroutine(DisableObject());
                worldAbilities.ResetNinjaDash();
            }
        }
    }

    private void DamageEnemies() {
        float thickness = .1f;

        RaycastHit[] hits;

        hits = Physics.SphereCastAll(startPos, thickness, endPos - startPos);

        for ( int i = 0; i < hits.Length; i++ ) {
            if ( hits[i].transform.GetComponent<EnemyUnit>() ) {
                hits[i].transform.GetComponent<EnemyUnit>().TakeDamage(damage, Tower.TowerType.NullValue);
            }
        }
    }

    private IEnumerator DisableObject() {
        yield return new WaitForSeconds(0.5f);
        ninja.SetActive(false);
        gameObject.SetActive(false);
    }

    private bool IsMouseOnUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}