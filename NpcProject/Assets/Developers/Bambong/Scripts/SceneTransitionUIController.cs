using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionUIController : UI_Base
{
    [SerializeField]
    public Canvas canvas;
    [SerializeField]
    public CanvasGroup canvasGroup;

    [SerializeField]
    private TextMeshProUGUI loadingText;

    [SerializeField]
    private Image rotateImage;


    public override void Init()
    {
        canvas.sortingOrder = 100;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
       // StartCoroutine(StartEffect());
    }
    public void LoadingStart() 
    {
        rotateImage.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        rotateImage.rectTransform.DORotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart);
    }
    public void LoadingEnd()
    {
        rotateImage.rectTransform.DOKill();
        rotateImage.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(false);
    }
    public void OpenPanel(float time) 
    {
  
        canvasGroup.DOFade(1, time).OnComplete(
            () =>
            {
                LoadingStart();
            }
            );
    }
    public void ClosePanel(float time) 
    {
        canvasGroup.DOFade(0, time).OnComplete(()=> LoadingEnd());
    }
    public void SetPercentUpdate(int amount)
    {
        loadingText.text = $"LOADING {amount}%";
    }
    private IEnumerator StartEffect()
    {
        float alpha = 1;
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(0.2f);
        //testEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(alpha, 0, progress);
            yield return null;
        }
        //testEffect.SetActive(false);

    }

}
