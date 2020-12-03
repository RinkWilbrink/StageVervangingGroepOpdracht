using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionButtonsUI : MonoBehaviour
{
    // Variables
    [SerializeField] private RectTransform ButtonsPanel;
    [SerializeField] private float UILerpSpeed;

    [HideInInspector] public int ButtonIndex = 0;

    [SerializeField] private Tower.TowerInteraction towerPlacement;

    private bool IsOpened = false;
    private bool IsOpening = false;

    private void Start()
    {
        ButtonsPanel.gameObject.SetActive(true);
        ButtonsPanel.anchoredPosition = new Vector2(gameObject.GetComponent<RectTransform>().rect.width + 10f, 0f);
    }

    // Set the Slide In or Out state.
    public void UISlideIn()
    {
        if (!IsOpening)
        {
            towerPlacement.SetSelectedButtonAttributes(towerPlacement.ButtonSelectionIndex);

            if (IsOpened)
            {
                ButtonsPanel.anchoredPosition = new Vector2(-5f, 0);
                StartCoroutine(buttonSlide(new Vector2(gameObject.GetComponent<RectTransform>().rect.width + 10f, 0f)));
                IsOpened = false;
                towerPlacement.CurrentInteractionMode = Tower.InteractionMode.UpgradeMode;
            }
            else
            {
                ButtonsPanel.anchoredPosition = new Vector2(gameObject.GetComponent<RectTransform>().rect.width + 10f, 0f);
                StartCoroutine(buttonSlide(new Vector2(-5f, 0)));
                IsOpened = true;
                //towerPlacement.SelectTower(0);
                towerPlacement.CurrentInteractionMode = Tower.InteractionMode.PlacementMode;
            }
        }
    }

    // Slide in or out the Tower and Building buttons
    IEnumerator buttonSlide(Vector2 NewPosition)
    {
        IsOpening = true;

        while (Vector3.Distance(ButtonsPanel.anchoredPosition, NewPosition) > 0.05f)
        {
            ButtonsPanel.anchoredPosition = Vector3.Lerp(ButtonsPanel.anchoredPosition, NewPosition, UILerpSpeed * Time.deltaTime);

            yield return null;
        }

        IsOpening = false;
    }

    // Set the time scale for the GameTime.delta time
    public void PauseGame(float TimeScale)
    {
        GameTime.SetTimeScale(TimeScale);
    }
}
