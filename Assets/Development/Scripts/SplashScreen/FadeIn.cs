using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{

    [SerializeField] Image _Panel;
    Coroutine _Corouting;
    public bool _FadeDone;

    private void Start()
    {
        _Corouting = StartCoroutine(FadingIn());
    }

    IEnumerator FadingIn()
    {
        _Panel.gameObject.SetActive(true);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            _Panel.color = new Color(_Panel.color.r, _Panel.color.g, _Panel.color.b, i);
            yield return null;
        }

        _FadeDone = true;
        _Panel.gameObject.SetActive(false);
    }
}
