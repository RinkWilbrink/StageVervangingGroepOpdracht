using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{
    [SerializeField] private float _WaitTime;
    [SerializeField] private Animator _MusicAnimator;
    [SerializeField] private AudioManagement _AudioManagement;

    private void Start()
    {
        _MusicAnimator = GameObject.Find("MusicAudioObject").GetComponent<Animator>();
    }

    public void LoadScene(int index)
    {
        StartCoroutine(ChangeScene(index));
        _AudioManagement.DisableMusicLowPass();
        Time.timeScale = 1;
    }

    private IEnumerator ChangeScene(int index)
    {
        _MusicAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(_WaitTime);
        if (index > 0)
        {
            SceneManager.LoadScene("Level" + index, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        _MusicAnimator.SetTrigger("FadeIn");
    }
}
