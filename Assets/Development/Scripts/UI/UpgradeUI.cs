using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace UI
{
    public class UpgradeUI : MonoBehaviour
    {
        // Variables
        [SerializeField] private RectTransform UpgradePanel;
        [Header("UI Stuff")]
        [SerializeField] private float UILerpSpeed;

        [Header("Upgrade Stats")]
        [SerializeField] private float DamageIncrease;
        [SerializeField] private float FireRateIncrease;

        [Header("bruh")]
        [SerializeField] private UpgradePanel DamageButtonPanel;
        [SerializeField] private UpgradePanel FirerateButtonPanel;

        [Space(6)]
        [SerializeField] private float DamageGoldMultiplier;
        [SerializeField] private float FirerateGoldMultiplier;

        [Space(4)]
        [HideInInspector] public Tower.TowerCore currentTower;

        void Start()
        {
            UpgradePanel.gameObject.SetActive(false);
        }

        public void UpdateUIPosition(float _x, float _y)
        {
            UpgradePanel.gameObject.SetActive(true);

            UpgradePanel.anchoredPosition = new Vector2(_x, _y);
            UpgradePanel.localScale = Vector3.zero;

            DamageButtonPanel.UpgradeMultiplier_Text.text = string.Format("{0}", currentTower.DamageMultiplier);
            DamageButtonPanel.UpgradeCost_Text.text = string.Format("{0}", currentTower.DamageMultiplier);

            FirerateButtonPanel.UpgradeMultiplier_Text.text = string.Format("{0}", currentTower.FireRateMutliplier);
            FirerateButtonPanel.UpgradeCost_Text.text = string.Format("{0}", currentTower.FireRateMutliplier);

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

        private bool PayGold(int Amount)
        {
            if(GameController.Gold >= Amount)
            {
                GameController.Gold -= Amount;
            }
            else
            {
                //Update UI That there is not enough gold.
                return false;
            }
            return true;
        }

        public void DamageButton(GameObject button)
        {
            if(PayGold(0))
            {
                currentTower.DamageMultiplier += DamageIncrease;

                Debug.Log(button.GetComponent<UpgradePanel>().UpgradeMultiplier_Text);
            }

            UpdateButtonUI();
        }

        public void FireRateButton(GameObject button)
        {
            if(PayGold(0))
            {
                currentTower.FireRateMutliplier += FireRateIncrease;

                Debug.Log(button.GetComponent<UpgradePanel>().UpgradeMultiplier_Text);
            }

            UpdateButtonUI();
        }

        private void UpdateButtonUI()
        {
            DamageButtonPanel.UpgradeMultiplier_Text.text = string.Format("{0}", currentTower.DamageMultiplier);
            DamageButtonPanel.UpgradeCost_Text.text = string.Format("{0}", currentTower.DamageMultiplier);

            FirerateButtonPanel.UpgradeMultiplier_Text.text = string.Format("{0}", currentTower.FireRateMutliplier);
            FirerateButtonPanel.UpgradeCost_Text.text = string.Format("{0}", currentTower.FireRateMutliplier);
        }

        public void CleanUpAfterClosing()
        {
            currentTower = null;
        }
    }
}