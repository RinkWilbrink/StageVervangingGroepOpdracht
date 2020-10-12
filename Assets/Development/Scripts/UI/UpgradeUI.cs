using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UpgradeUI : MonoBehaviour
    {
        // Variables
        [SerializeField] private RectTransform UpgradePanel;
        [Header("UI Stuff")]
        [SerializeField] private float UILerpSpeed;

        [SerializeField] private Tower.TowerCore currentTower;

        void Start()
        {
            UpgradePanel.gameObject.SetActive(false);
        }

        public void UpdateUIPosition(float _x, float _y)
        {
            UpgradePanel.gameObject.SetActive(true);

            UpgradePanel.anchoredPosition = new Vector2(_x, _y);
            UpgradePanel.localScale = Vector3.zero;

            StartCoroutine(LerpUI());
        }

        IEnumerator LerpUI()
        {
            while (Vector3.Distance(UpgradePanel.anchoredPosition, Vector2.zero) > 0.05f)
            {
                UpgradePanel.anchoredPosition = Vector3.Lerp(UpgradePanel.anchoredPosition, Vector2.zero, UILerpSpeed * Time.deltaTime);
                UpgradePanel.localScale = Vector3.Lerp(UpgradePanel.localScale, Vector3.one, UILerpSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}