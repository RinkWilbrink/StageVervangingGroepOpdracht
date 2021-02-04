using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{
    [SerializeField] private float _WaitTime;
    private Animator _MusicAnimator;
    [SerializeField] private Animator _SplashScreenAnimator;
    [SerializeField] private AudioManagement _AudioManagement;

    private void Start()
    {
        try
        {
            _MusicAnimator = GameObject.Find("MusicAudioObject").GetComponent<Animator>();
        }
        catch (System.Exception e)
        {
            Debug.Log("This is only shown when the starting scene is not the MainMenu: " + e);
        }
    }

    public void LoadScene(int index)
    {
        StartCoroutine(ChangeScene(index));
        _AudioManagement.DisableMusicLowPass();
        Time.timeScale = 1;
    }

    private IEnumerator ChangeScene(int index)
    {
        if (_MusicAnimator != null)
        {
            _MusicAnimator.SetTrigger("FadeOut");
        }
        _SplashScreenAnimator.SetTrigger("ScreenDown");

        yield return new WaitForSeconds(_WaitTime);
        if (index > 0)
        {
            SceneManager.LoadScene("Level" + index, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("SplashScreen", LoadSceneMode.Single);
        }

        if (_MusicAnimator != null)
        {
            _MusicAnimator.SetTrigger("FadeIn");
        }
        _SplashScreenAnimator.SetTrigger("ScreenUp");
    }
}
