using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EffectController : MonoBehaviour
{
    [SerializeField]
    private GameObject glitch;

    [SerializeField]
    private Image Fade;

    [SerializeField]
    private Animator transition;


    private void Awake()
    {
        GlitchOn();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(FadeEffect());
    }

    IEnumerator FadeEffect()
    {
        transition.SetTrigger("Start");
        yield return null;
    }

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
}
