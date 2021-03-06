using MainMenuUI;
using System;
using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UpgradeUI : MonoBehaviour
    {
        public static bool UpgradeUIReady;

        private Scene currScene;
        // Variables
        [SerializeField] private RectTransform UpgradePanel;
        [Header("UI Stuff")]
        [SerializeField] private float UILerpSpeed;

        [Header("Upgrade Stats")]
        [SerializeField] private float DamageIncrease;
        [SerializeField] private float FireRateIncrease;

        [Header("Special Button")]
        [SerializeField] private Button buttonUpgrade;
        [SerializeField] private Button buttonSpecial1;
        [SerializeField] private Button buttonSpecial2;
        [SerializeField] private Button closeUpgradeButton;

        [Space(4)]

        [SerializeField] private Dictionary<Tower.TowerType, images> TowerTypeImageDictionairy = new Dictionary<TowerType, images>();

        [Header("B")]
        [SerializeField] private images[] Towers;

        [Space(6)]

        [Header("Resources")]
        [SerializeField] private float DamageGoldMultiplier;
        [SerializeField] private float FirerateGoldMultiplier;
        [SerializeField] private ResourceUIManager ResourceManager;

        [Space(4)]
        [SerializeField] private TMPro.TextMeshProUGUI HealthText;
        [SerializeField] private GameObject DeathScreen;

        [Header("Script References")]
        [SerializeField] private SelectionButtonManager TowerSelectionManager;
        [SerializeField] private TowerInteraction TowerInteraction;

        [Space(6)]

        [SerializeField] private Button SpecialAbilityModeButton;

        [Space(4)]
        [HideInInspector] public TowerCore currentTower;

        
        [SerializeField] private string Level1;
        [SerializeField] private string Level2;
        [SerializeField] private string Level3;

        [SerializeField] private GameObject Errormssg;

        [SerializeField] private Sprite lockedImage;

        public TMPro.TextMeshProUGUI towerUpgradeCostText;

        private void Awake()
        {
            for(int i = 0; i < Enum.GetNames(typeof(Tower.TowerType)).Length; i++)
            {
                TowerTypeImageDictionairy.Add((TowerType)i, Towers[i]);
            }
        }

        private void Start()
        {
            UpgradePanel.gameObject.SetActive(false);
            SpecialAbilityModeButton.interactable = false;
            UpgradeUIReady = false;
            currScene = SceneManager.GetActiveScene();

            HealthText.text = string.Format("{0}", GameController.MainTowerHP);
        }

        public void UpdateUIPosition(float _x, float _y)
        {

            if (currScene.name != Level1)
            {
                UpgradePanel.gameObject.SetActive(true);

                UpgradePanel.anchoredPosition = new Vector2(_x, _y);
                UpgradePanel.localScale = Vector3.zero;

                StartCoroutine(LerpUI());
            }
            else 
            {
                Debug.Log("Can't upgrade towers on level 1");
            }
            
        }

        #region Private Functions

        private IEnumerator LerpUI()
        {
            while (Vector3.Distance(UpgradePanel.localScale, Vector3.one) > 0.005f)
            {
                UpgradePanel.localScale = Vector3.Lerp(UpgradePanel.localScale, Vector3.one, UILerpSpeed * Time.deltaTime);

                yield return null;
            }

            yield return 0;
        }

        // Set the size of the Special Attack Button when clicked
        private void SetSpecialButtonTransform(int Index)
        {
            Vector2 Special1Scale = Vector2.one; Vector2 Special2Scale = Vector2.one;
            Vector2 Special1Position = new Vector2(-1, 0); Vector2 Special2Position = new Vector2(1, 0);

            if(Index == 1)
            {
                Special1Scale = new Vector2(1.2f, 1.2f);
                Special1Position = new Vector2(
                        Special1Position.x - (buttonSpecial1.GetComponent<RectTransform>().rect.width * 0.1f),
                        Special1Position.y + (buttonSpecial1.GetComponent<RectTransform>().rect.height * 0.1f));
            }
            else if(Index == 2)
            {
                Special2Scale = new Vector2(1.2f, 1.2f);
                Special2Position = new Vector2(
                        Special2Position.x + (buttonSpecial2.GetComponent<RectTransform>().rect.width * 0.1f),
                        Special2Position.y + (buttonSpecial2.GetComponent<RectTransform>().rect.height * 0.1f));
            }

            buttonSpecial1.transform.localScale = Special1Scale; 
            buttonSpecial2.transform.localScale = Special2Scale;
            buttonSpecial1.GetComponent<RectTransform>().anchoredPosition = Special1Position; 
            buttonSpecial2.GetComponent<RectTransform>().anchoredPosition = Special2Position;
        }

        // Enable or Disable the Special Attack button for the towers
        private void SetSpecialButtons()
        {
            buttonSpecial1.interactable = false;
            buttonSpecial2.interactable = false;

            if(currentTower.TowerLevel >= currentTower.TowerLevelToUnlockSpecial)
            {
                if(currentTower.SpecialUnlocked == SpecialAttack.None)
                {
                    if(currScene.name == Level3)
                    {
                        buttonSpecial1.interactable = true;
                        buttonSpecial2.interactable = true;
                    }
                    else
                    {
                        buttonSpecial1.interactable = false;
                        buttonSpecial2.interactable = false;                      
                    }
                }
            }

            SetSpecialButtonTransform((int)currentTower.SpecialUnlocked);
        }

        #endregion

        #region PayResources

        public void DoMainTowerDamage(int _dmg)
        {
            GameController.MainTowerHP -= _dmg;

            if(GameController.MainTowerHP < 1)
            {
                Debug.Log("GAME OVER");
                DeathScreen.SetActive(true);

                GameTime.SetTimeScale(0);
            }

            HealthText.text = string.Format("{0}", GameController.MainTowerHP);
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

        #endregion

        [SerializeField] private AudioClip constructionAudio;
        public void UpgradeTower()
        {
            if (/*PayGold(4)*/ PayGold(currentTower.TowerUpgradeCosts.UpgradeCosts[currentTower.TowerLevel - 1]))
            {
                currentTower.TowerLevel += 1;
                FindObjectOfType<AudioManagement>().PlayAudioClip(constructionAudio, AudioMixerGroups.SFX);
                towerUpgradeCostText.text = currentTower.TowerUpgradeCosts.UpgradeCosts[currentTower.TowerLevel - 1] + "";

                if(currentTower.TowerLevel == currentTower.TowerLevelToUnlockSpecial && currScene.name == Level3)
                {
                    TowerInteraction.AddTowerToSpecialAbilityUnlockedList(currentTower);
                    SpecialAbilityModeButton.interactable = true;
                    buttonUpgrade.interactable = false;
                    closeUpgradeButton.interactable = false;
                    UpgradeUIReady = true;
                    SetSpecialButtons();
                } else
                {
                    SpecialAbilityModeButton.interactable = false;
                    UpgradeUIReady = false;
                }
                if (currentTower.SpecialUnlocked != SpecialAttack.None)
                {
                    currentTower.TowerSpecialLevel += 1;
                }
                currentTower.SetNewSprite();
                currentTower.UpdateDamageValues();
            }
        }

        // Cleanup function for when closing the Upgrade UI Panel
        public void CloseUpgradePanel()
        {
            currentTower = null;
        }

        public void SpecialButton()
        {
            images b;

            if (currScene.name == Level3)
            {
                TowerTypeImageDictionairy.TryGetValue(currentTower.towerType, out b);
                buttonSpecial1.image.sprite = b.image1;
                buttonSpecial2.image.sprite = b.image2;

                SetSpecialButtonTransform((int)currentTower.SpecialUnlocked);
                SetSpecialButtons();
            }
            else if (currScene.name == Level2)
            {
                buttonSpecial1.image.sprite = lockedImage;
                buttonSpecial2.image.sprite = lockedImage;

                SetSpecialButtonTransform((int)currentTower.SpecialUnlocked);
                SetSpecialButtons();
            }
            
        }

        // Unlock special ability for the tower that has just been upgraded.
        public void SetSpecialAbility(int _i)
        {
            if(_i > 2)
            {
                _i = 0;
            }
            currentTower.SpecialUnlocked = (SpecialAttack)_i;

            buttonUpgrade.interactable = true;
            closeUpgradeButton.interactable = true;
            UpgradeUIReady = false;
            buttonSpecial1.interactable = false;
            buttonSpecial2.interactable = false;

            SetSpecialButtons();
            currentTower.SetNewSprite();
        }

        [Serializable]
        public struct images
        {
            [SerializeField] public Sprite image1;
            [SerializeField] public Sprite image2;
        }
    }
}