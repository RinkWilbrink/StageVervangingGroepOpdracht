using System.Collections;
using Tower;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("Special Button")]
        [SerializeField] private Button buttonSpecial1;
        [SerializeField] private Button buttonSpecial2;

        [Header("Resources")]
        [SerializeField] private float DamageGoldMultiplier;
        [SerializeField] private float FirerateGoldMultiplier;
        [SerializeField] private ResourceUIManager ResourceManager;

        [Header("Script References")]
        [SerializeField] private SelectionButtonManager TowerSelectionManager;
        [SerializeField] private TowerInteraction TowerInteraction;

        [Space(6)]

        [SerializeField] private Button SpecialAbilityModeButton;

        [Space(4)]
        [HideInInspector] public TowerCore currentTower;

        private void Start()
        {
            UpgradePanel.gameObject.SetActive(false);
            SpecialAbilityModeButton.interactable = false;
        }

        public void UpdateUIPosition(float _x, float _y)
        {
            UpgradePanel.gameObject.SetActive(true);

            UpgradePanel.anchoredPosition = new Vector2(_x, _y);
            UpgradePanel.localScale = Vector3.zero;

            //UpdateButtonUI();

            StartCoroutine(LerpUI());
        }

        private IEnumerator LerpUI()
        {
            while (Vector3.Distance(UpgradePanel.localScale, Vector3.one) > 0.005f)
            {
                UpgradePanel.localScale = Vector3.Lerp(UpgradePanel.localScale, Vector3.one, UILerpSpeed * Time.deltaTime);

                yield return null;
            }

            yield return 0;
        }

        public bool PayGold(int Amount)
        {
            if(Amount <= GameController.Gold)
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

        public bool PayMana(int Amount)
        {
            if (Amount <= GameController.Mana)
            {
                GameController.Mana -= Amount;
            }
            else
            {
                return false;
            }

            TowerSelectionManager.UpdateTowerButtonUI();

            ResourceManager.UpdateResourceUI();

            return true;
        }

        public void UpgradeTower()
        {
            if (PayGold(1))
            {
                currentTower.TowerLevel += 1;
                if(currentTower.TowerLevel == currentTower.TowerLevelToUnlockSpecial)
                {
                    TowerInteraction.AddTowerToSpecialAbilityUnlockedList(currentTower);
                    SpecialAbilityModeButton.interactable = true;
                }
            }

            currentTower.UpdateDamageValues();
        }

        public void CleanUpAfterClosing()
        {
            currentTower = null;
        }

        public void SpecialButton()
        {
            buttonSpecial1.image = image1;
            buttonSpecial2.image = image2;
        }
    }
}