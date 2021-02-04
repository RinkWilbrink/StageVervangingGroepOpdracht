using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAbilityUI : MonoBehaviour
{
    private Tower.TowerInteraction towerInteraction;
    private CanvasGroup canvasGroup;

    private void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        towerInteraction = FindObjectOfType<Tower.TowerInteraction>();
    }

    private void Update() {
        if ( towerInteraction.CurrentInteractionMode == Tower.InteractionMode.None ) {
            canvasGroup.alpha = .15f;
            canvasGroup.blocksRaycasts = false;
        } else {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
