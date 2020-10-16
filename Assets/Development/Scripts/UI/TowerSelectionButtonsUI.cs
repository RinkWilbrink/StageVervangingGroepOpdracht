using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerSelectionButtonsUI : MonoBehaviour
{
    // Variables
    [SerializeField] private RectTransform ButtonsPanel;
    [SerializeField] private float UILerpSpeed;

    [SerializeField] private TowerPlacement towerPlacement;

    private bool IsOpened = false;
    private bool IsOpening = false;

    private void Start()
    {
        ButtonsPanel.gameObject.SetActive(true);
        ButtonsPanel.anchoredPosition = new Vector2(gameObject.GetComponent<RectTransform>().rect.width, 0f);
    }

    public void UISlideIn()
    {
        if(!IsOpening)
        {
            if(IsOpened)
            {
                ButtonsPanel.anchoredPosition = Vector2.zero;
                StartCoroutine(buttonSlide(new Vector2(gameObject.GetComponent<RectTransform>().rect.width, 0f)));

                IsOpened = false;
                towerPlacement.CanPlaceTowers = false;
            }
            else
            {
                ButtonsPanel.anchoredPosition = new Vector2(gameObject.GetComponent<RectTransform>().rect.width, 0f);
                StartCoroutine(buttonSlide(Vector2.zero));

                IsOpened = true;
                towerPlacement.SelectTower(0);
            }
        }
    }

    IEnumerator buttonSlide(Vector2 NewPosition)
    {
        IsOpening = true;

        while(Vector3.Distance(ButtonsPanel.anchoredPosition, NewPosition) > 0.05f)
        {
            ButtonsPanel.anchoredPosition = Vector3.Lerp(ButtonsPanel.anchoredPosition, NewPosition, UILerpSpeed * Time.deltaTime);

            yield return null;
        }

        IsOpening = false;
    }
}
