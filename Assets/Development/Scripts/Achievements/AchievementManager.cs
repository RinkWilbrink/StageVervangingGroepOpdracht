using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    // Variables
    [Header("Text")]
    [SerializeField] private RectTransform AchievementPanel;
    [SerializeField] private RectTransform MaskPanel;
    [Space(4)]
    [SerializeField] private TMPro.TextMeshProUGUI AchievementName;
    [SerializeField] private TMPro.TextMeshProUGUI AchievementRequirements;

    [Header("Audio")]
    [SerializeField] private AudioSource dingSoundEffect;

    [Header("Slide In/Out Timing")]
    [SerializeField] private float SlideSpeed;
    [SerializeField] private float SlideInBetweenTime;

    private bool IsSliding = false;
    private Vector2 SlideOutPosition;

    private void Awake()
    {
        IsSliding = false;

        SlideOutPosition = new Vector2(MaskPanel.rect.width + 10, 0);
        AchievementPanel.anchoredPosition = SlideOutPosition;
    }

    private void Update()
    {
        if(IsSliding == false)
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                dingSoundEffect.pitch = (1f + Random.Range(-0.02f, 0.02f));
                dingSoundEffect.Play();
                AchievementUnlocked();
            }
        }
    }

    public void AchievementUnlocked()
    {
        SetAchievementText("Name", "Requirements");
        StartCoroutine(AchievementSlide());
    }

    private IEnumerator AchievementSlide()
    {
        IsSliding = true;

        while(Vector2.Distance(AchievementPanel.anchoredPosition, Vector2.zero) > 0.1f)
        {
            AchievementPanel.anchoredPosition = Vector2.Lerp(AchievementPanel.anchoredPosition, Vector2.zero, SlideSpeed * Time.deltaTime);

            yield return null;
        }

        AchievementPanel.anchoredPosition = Vector2.zero;

        yield return new WaitForSeconds(SlideInBetweenTime);

        while(Vector2.Distance(AchievementPanel.anchoredPosition, SlideOutPosition) > 0.1f)
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