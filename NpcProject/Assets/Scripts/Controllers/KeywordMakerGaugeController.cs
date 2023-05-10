using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordMakerGaugeController : MonoBehaviour
{
    [SerializeField]
    private int needForMakeCount = 3;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image fillImage;

    [SerializeField]
    private string targeKeywordName;
    private Queue<Func<IEnumerator>> addAnimQueue = new Queue<Func<IEnumerator>>();


    private readonly float ONCE_AMOUNT_ANIMTIME = 0.3f;  
    private readonly float MAKE_ANIMTIME = 0.5f;  
    private readonly float ACTIVE_ANIMTIME = 0.2f;  

    private int curCount = 0;
    private bool isOpne = false;
    private void Start()
    {
        Managers.Scene.OnSceneUnload += () => { 
            StopAllCoroutines(); };
    }
    public void AddCount(int amount)
    {
        for( int i = 0; i< amount; ++i) 
        { 
            addAnimQueue.Enqueue(AddOnceAnim);
        }
            Open();
    }
    public IEnumerator AddOnceAnim() 
    {
        curCount++;
        float fillAmount =  curCount / (float)needForMakeCount;
        fillImage.DOFillAmount(fillAmount, ONCE_AMOUNT_ANIMTIME);
        Managers.Sound.PlaySFX("Enlargement Keyword");
        yield return new WaitForSeconds(ONCE_AMOUNT_ANIMTIME);
    }
  
    IEnumerator PlayOpenAnim()
    {
        canvasGroup.DOFade(1, ACTIVE_ANIMTIME);

        while(addAnimQueue.Count >0) 
        {
            yield return addAnimQueue.Dequeue().Invoke();
            if(curCount >= needForMakeCount) 
            {
                yield return MakeKeyword();
            }
        }
        Close();
    }
    public IEnumerator MakeKeyword()
    {
        curCount -= needForMakeCount;
        var keywordMake = Managers.Keyword.MakeKeywordToCurPlayerPanel(targeKeywordName);

        if(keywordMake != null && keywordMake.gameObject.activeSelf) 
        {
            keywordMake.transform.localScale = Vector3.zero;
            keywordMake.transform.DOScale(Vector3.one, MAKE_ANIMTIME);
        }
        fillImage.DOFillAmount(0, MAKE_ANIMTIME);
        yield return new WaitForSeconds(MAKE_ANIMTIME);
    }

    private void Open()
    { 
        if(isOpne)
        {
            return;
        }
        isOpne = true;
        StartCoroutine(PlayOpenAnim());
    }
    private void Close() 
    {
        if (!isOpne)
        {
            return;
        }
        isOpne = false;
        canvasGroup.DOFade(0, ACTIVE_ANIMTIME);
    }


}
