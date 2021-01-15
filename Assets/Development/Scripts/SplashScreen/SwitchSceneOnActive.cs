using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchSceneOnActive : MonoBehaviour
{
    [SerializeField] SaveLoad _SaveLoad;
    [SerializeField] string _SceneName;

    [SerializeField] Image _Kappa;
    Coroutine _Corouting;

    [SerializeField] FadeIn _FadeIn;

    void Update()
    {
        if (_SaveLoad.GetLoadThread() == false && _FadeIn._FadeDone == true)
        {
            _Corouting = StartCoroutine(FadingOut());
        }
    }

    IEnumerator FadingOut()
    {
        _Kappa.gameObject.SetActive(true);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            _Kappa.color = new Color(_Kappa.color.r, _Kappa.color.g, _Kappa.color.b, i);
            yield return null;
        }
        _Kappa.color = new Color(0, 0, 0, 0);
        SceneManager.LoadScene(_SceneName);
    }
}
