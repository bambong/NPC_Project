using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EffectController : MonoBehaviour
{
    public GameObject glitch;
    public Image Fade;

    private float time = 0f;
    private float fadeDuration = 1f;


    private void Awake()
    {
        GlitchOn();
        FadeEffect();
    }


    //glitch effect
    #region
    public void GlitchOn()
    {
        glitch.SetActive(true);
        Debug.Log("Glitch On");
    }

    public void GlitchOff()
    {
        glitch.SetActive(false);
        Debug.Log("Glitch Off");
    }
    #endregion



    public void FadeEffect()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Debug.Log("Fade In and Out");

        time = 0f;
        Fade.gameObject.SetActive(true);
        Color alpha = Fade.color;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / fadeDuration;
            alpha.a = Mathf.Lerp(0, 1, time);
            Fade.color = alpha;
            yield return null;
        }

        time = 0;
        yield return new WaitForSeconds(1f);
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / fadeDuration;
            alpha.a = Mathf.Lerp(1, 0, time);
            Fade.color = alpha;
            yield return null;
        }

        Fade.gameObject.SetActive(false);
        yield return null;
    }

    //아직 미사용
    IEnumerator FadeIn()
    {
        Debug.Log("Black FadeIn");

        time = 0f;
        Color alpha = Fade.color;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / fadeDuration;
            alpha.a = Mathf.Lerp(1, 0, time);
            Fade.color = alpha;
            yield return null;
        }

        Fade.gameObject.SetActive(false);
        yield return null;
    }
}
