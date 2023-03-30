using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInputController : MonoBehaviour
{
    [SerializeField]
    private GameObject paper;

    private float fadeTime = 0.5f;
    private float time = 0f;
    private float start = 1f;
    private float end = 0f;

    private Animator paperAnim;
    private Image paperRend;

    private void Awake()
    {
        paperAnim = paper.GetComponent<Animator>();
        paperRend = paper.GetComponent<Image>();  
    }

    private void FixedUpdate()
    {
        InputCheck();
    }

    private void InputCheck()
    {
        if(Input.GetMouseButtonDown(0))
        {
            paperAnim.SetTrigger("paper");
            StartCoroutine(PaperInvisible());
        }
    }

    IEnumerator PaperInvisible()
    {
        Color fadeColor = paperRend.color;
        fadeColor.a = Mathf.Lerp(start, end, time);

        yield return new WaitForSeconds(0.35f);

        while (fadeColor.a > 0f)
        {
            time += Time.deltaTime / fadeTime;
            fadeColor.a = Mathf.Lerp(start, end, time);
            paperRend.color = fadeColor;
            yield return null;
        }
    }
}
