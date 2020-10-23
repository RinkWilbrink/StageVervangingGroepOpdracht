using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuUI
{
    public class MainMenuLevelSelectButtons : MonoBehaviour
    {
        // Variables
        [SerializeField] private CoolStuff[] LevelButtonInformation;

        void Awake()
        {
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

        #region Fade Coroutine

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
                StartCoroutine(
                FadeImage(LevelButtonInformation[_levelIndex].buttonBackgroundImage[LevelButtonInformation[_levelIndex].imageIndex], LevelButtonInformation[_levelIndex].buttonBackgroundImage[0], true));

                LevelButtonInformation[_levelIndex].imageIndex = 0;
            }
        }

        IEnumerator FadeImage(Image img, Image disableImage, bool fadeAway)
        {
            if (fadeAway)
            {
                for (float i = 1; i >= 0; i -= Time.deltaTime)
                {
                    img.color = new Color(1, 1, 1, i);
                    yield return null;
                }
                img.gameObject.SetActive(false);
            }
            else
            {
                for (float i = 0; i <= 1; i += Time.deltaTime)
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
    public struct CoolStuff
    {
        public Image[] buttonBackgroundImage;
        public int imageIndex;
    }
}