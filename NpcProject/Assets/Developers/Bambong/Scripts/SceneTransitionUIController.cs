using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionUIController : UI_Base
{
    [SerializeField]
    public Canvas canvas;
    [SerializeField]
    public CanvasGroup canvasGroup;
    [SerializeField]
    private GameObject testEffect;
    public override void Init()
    {
        canvas.sortingOrder = 100;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(StartEffect());
    }

    private IEnumerator StartEffect()
    {
        float alpha = 1;
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(0.2f);
        testEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(alpha, 0, progress);
            yield return null;
        }
        testEffect.SetActive(false);

    }

}
