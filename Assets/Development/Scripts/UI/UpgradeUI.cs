using System.Collections;
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

        [Header("Resources")]
        [SerializeField] private float DamageGoldMultiplier;
        [SerializeField] private float FirerateGoldMultiplier;
        [SerializeField] private ResourceUIManager ResourceManager;

        [Header("a")]
        [SerializeField] private TowerSelectionButtonManager TowerSelectionManager;

        [Space(4)]
        [HideInInspector] public TowerCore currentTower;

        private void Start()
        {
            UpgradePanel.gameObject.SetActive(false);
        }

        public void UpdateUIPosition(float _x, float _y)
        {
            UpgradePanel.gameObject.SetActive(true);

            UpgradePanel.anchoredPosition = new Vector2(_x, _y);
            UpgradePanel.localScale = Vector3.zero;

            UpdateButtonUI();

            StartCoroutine(LerpUI());
        }

        private IEnumerator LerpUI()
        {
            while(Vector3.Distance(UpgradePanel.anchoredPosition, Vector2.zero) > 0.05f)
            {
                UpgradePanel.anchoredPosition = Vector3.Lerp(UpgradePanel.anchoredPosition, Vector2.zero, UILerpSpeed * Time.deltaTime);
                UpgradePanel.localScale = Vector3.Lerp(UpgradePanel.localScale, Vector3.one, UILerpSpeed * Time.deltaTime);

                yield return null;
            }
        }

        public bool PayGold(int Amount)
        {
            if(GameController.Gold >= Amount)
            {
                GameController.Gold -= Amount;
            }
            else
            {
                return false;
            }

            TowerSelectionManager.UpdateTowerButtonUI();

            ResourceManager.UpdateResourceUI();

            return true;
        }

        public void DamageButton(GameObject button)
        {
            if(PayGold(1))
            {
                currentTower.DamageLevel += 1;
            }

            UpdateButtonUI();
            currentTower.UpdateDamageValues();
        }

        public void FireRateButton(GameObject button)
        {
            if(PayGold(1))
            {
                currentTower.FireRateLevel += 1;
            }

            UpdateButtonUI();
            currentTower.UpdateDamageValues();
        }

        private void UpdateButtonUI()
        {
            DamageButtonPanel.UpgradeCost_Text.text = string.Format("{0}", currentTower.DamageLevel + 1);
            DamageButtonPanel.UpgradeMultiplier_Text.text = string.Format("{0}", currentTower.DamageLevel);

            FirerateButtonPanel.UpgradeCost_Text.text = string.Format("{0}", currentTower.FireRateLevel + 1);
            FirerateButtonPanel.UpgradeMultiplier_Text.text = string.Format("{0}", currentTower.FireRateLevel);
        }

        public void CleanUpAfterClosing()
        {
            currentTower = null;
        }
    }
}