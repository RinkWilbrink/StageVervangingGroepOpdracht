using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchSceneOnActive : MonoBehaviour
{
    [SerializeField] SaveLoad _SaveLoad;
    [SerializeField] string _SceneName;

    [SerializeField] Image _Panel;
    [SerializeField] Color _PanelColor;
    Coroutine _Corouting;

    [SerializeField] FadeIn _FadeIn;

    void Update()
    {
        if (_SaveLoad.GetLoadThread() == false && _FadeIn._FadeDone == true)
        {
            _Panel.color = _PanelColor;
            _Corouting = StartCoroutine(FadingOut());
        }
    }

    IEnumerator FadingOut()
    {
        _Panel.gameObject.SetActive(true);

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            _Panel.color = new Color(_Panel.color.r, _Panel.color.g, _Panel.color.b, i);
            yield return null;
        }
        _Panel.color = _PanelColor;
        SceneManager.LoadScene(_SceneName);
    }
}
