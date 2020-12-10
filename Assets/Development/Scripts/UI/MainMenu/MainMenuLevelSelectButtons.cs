using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuUI
{
    public class MainMenuLevelSelectButtons : MonoBehaviour
    {
        // Variables
        [SerializeField] private ButtonInformation[] LevelButtonInformation;

        [Header("Timers etc")]
        [SerializeField] private float FadeTime;
        [SerializeField] private float InbetweenTime;

        private int ButtonToFadeIndex = 0;
        private float timer = 0f;

        void Awake()
        {
            for (int x = 0; x < LevelButtonInformation.Length; x++)
            {
                LevelButtonInformation[x].buttonBackgroundImage = LevelButtonInformation[x].Parent.GetComponentsInChildren<Image>();
            }

            for (int i = 0; i < LevelButtonInformation.Length; i++)
            {
                for (int x = 0; x < LevelButtonInformation[i].buttonBackgroundImage.Length; x++)
                {
                    if (x == 0)
                    {
                        LevelButtonInformation[i].buttonBackgroundImage[x].gameObject.SetActive(true);
                    }
                    else
                    {
                        LevelButtonInformation[i].buttonBackgroundImage[x].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void Update()
        {
            // Timer to cycle the images shown on the Level select buttons
            if(timer > FadeTime + (InbetweenTime * ButtonToFadeIndex))
            {
                StartFade(ButtonToFadeIndex);

                Debug.Log(LevelButtonInformation.Length);

                if(ButtonToFadeIndex < LevelButtonInformation.Length - 1)
                {
                    ButtonToFadeIndex += 1;
                }
                else
                {
                    ButtonToFadeIndex = 0;
                    timer = 0f;
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        #region Fade Coroutine

        // Fade level select images, this will cycle the images when called in an update fuction.
        public void StartFade(int _levelIndex)
        {
            int local = 0;
            if (LevelButtonInformation[_levelIndex].imageIndex < LevelButtonInformation[_levelIndex].buttonBackgroundImage.Length)
            {
                local = LevelButtonInformation[_levelIndex].imageIndex;
            }
            
            if (LevelButtonInformation[_levelIndex].imageIndex < LevelButtonInformation[_levelIndex].buttonBackgroundImage.Length - 1)
            {
                LevelButtonInformation[_levelIndex].buttonBackgroundImage[local + 1].gameObject.SetActive(true);
            
                StartCoroutine(FadeImage(LevelButtonInformation[_levelIndex].buttonBackgroundImage[local + 1], LevelButtonInformation[_levelIndex].buttonBackgroundImage[local], false));
            
                LevelButtonInformation[_levelIndex].imageIndex += 1;
            }
            else
            {
                LevelButtonInformation[_levelIndex].buttonBackgroundImage[0].gameObject.SetActive(true);
                StartCoroutine(FadeImage(LevelButtonInformation[_levelIndex].buttonBackgroundImage[LevelButtonInformation[_levelIndex].imageIndex],
                    LevelButtonInformation[_levelIndex].buttonBackgroundImage[0], true));
            
                LevelButtonInformation[_levelIndex].imageIndex = 0;
            }
        }

        // This function will fade in a new image and can fade out the previous image.
        IEnumerator FadeImage(Image img, Image disableImage, bool fadeAway)
        {
            if (fadeAway)
            {
                for (float i = 1; i >= 0; i -= 1f * Time.deltaTime)
                {
                    img.color = new Color(1, 1, 1, i);
                    yield return null;
                }
                img.gameObject.SetActive(false);
            }
            else
            {
                for (float i = 0; i <= 1; i += 1f * Time.deltaTime)
                {
                    img.color = new Color(1, 1, 1, i);
                    yield return null;
                }
                disableImage.gameObject.SetActive(false);
            }
        }

        #endregion
    }

    [Serializable]
    public struct ButtonInformation
    {
        public GameObject Parent;
        [HideInInspector] public Image[] buttonBackgroundImage;
        [HideInInspector] public int imageIndex;
    }
}