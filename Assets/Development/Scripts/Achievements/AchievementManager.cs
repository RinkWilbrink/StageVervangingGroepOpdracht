using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* References
 * 
 * Events:
 * https://www.youtube.com/watch?v=gx0Lt4tCDE0
 * https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html
*/

namespace Achievements
{
    public class AchievementManager : MonoBehaviour
    {
        // Variables
        [Header("Text")]
        [SerializeField] private RectTransform AchievementPanel;
        [Space(6)]
        [SerializeField] private TMPro.TextMeshProUGUI AchievementName;
        [SerializeField] private TMPro.TextMeshProUGUI AchievementRequirements;

        public static AchievementManager current;

        [Header("Audio")]
        [SerializeField] private AudioSource dingSoundEffect;

        [Header("Slide In/Out Timing")]
        [SerializeField] private float SlideSpeed;
        [SerializeField] private float SlideInBetweenTime;

        private bool IsSliding = false;
        private Vector2 SlideOutPosition;

        private void Awake()
        {
            current = this;

            IsSliding = false;

            SlideOutPosition = new Vector2(gameObject.GetComponent<RectTransform>().rect.width + 10, 0);
            AchievementPanel.anchoredPosition = SlideOutPosition;
        }

        private void Update()
        {
            if (IsSliding == false)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    UnlockAchievement("");
                }
            }
        }

        public void UnlockAchievement(string AchievementUnlocked)
        {
            // Audio
            dingSoundEffect.pitch = (1f + UnityEngine.Random.Range(-0.02f, 0.02f));
            dingSoundEffect.Play();

            // Visual stuff
            SetAchievementText("Name", "Requirements");
            StartCoroutine(AchievementSlide());
        }

        private IEnumerator AchievementSlide()
        {
            IsSliding = true;

            while (Vector2.Distance(AchievementPanel.anchoredPosition, Vector2.zero) > 1f)
            {
                AchievementPanel.anchoredPosition = Vector2.Lerp(AchievementPanel.anchoredPosition, Vector2.zero, SlideSpeed * Time.deltaTime);

                yield return null;
            }

            AchievementPanel.anchoredPosition = Vector2.zero;

            yield return new WaitForSeconds(SlideInBetweenTime);

            while (Vector2.Distance(AchievementPanel.anchoredPosition, SlideOutPosition) > 1f)
            {
                AchievementPanel.anchoredPosition = Vector2.Lerp(AchievementPanel.anchoredPosition, SlideOutPosition, SlideSpeed * Time.deltaTime);

                yield return null;
            }

            AchievementPanel.anchoredPosition = SlideOutPosition;
            IsSliding = false;
        }

        private void SetAchievementText(string _Name, string _Requirements)
        {
            AchievementName.text = string.Format("{0} Unlocked", _Name);
            AchievementRequirements.text = string.Format("{0}", _Requirements);
        }
    }
}