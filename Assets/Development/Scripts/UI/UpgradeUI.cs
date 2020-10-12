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

            StartCoroutine(LerpUI(new Vector2(_x, _y)));
        }

        IEnumerator LerpUI(Vector2 position)
        {
            UpgradePanel.anchoredPosition = position;
            UpgradePanel.localScale = Vector3.zero;

            while (Vector3.Distance(position, Vector2.zero) > 0.05f)
            {
                UpgradePanel.anchoredPosition = Vector3.Lerp(UpgradePanel.anchoredPosition, Vector2.zero, UILerpSpeed * Time.deltaTime);
                UpgradePanel.localScale = Vector3.Lerp(UpgradePanel.localScale, Vector3.one, UILerpSpeed * Time.deltaTime);

                yield return null;
            }

            Debug.Log("Banaan!");

            yield return new WaitForSeconds(0.1f);
        }
    }
}