using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Services.Analytics.Platform;
using UnityEngine;
using UnityEngine.UI;

public class DebugGaugeUiController : UI_Base
{
    [SerializeField]
    private Image fillImage;

    [Header("Normal Color Option")]
    [SerializeField]
    private List<Color> normalColors;

    [Header("Inpinity Color Option")]
    [SerializeField]
    private List<Color> infinityColors;
    [SerializeField]
    private float colorTime = 0.2f;

    private bool isOpen = false;
    
  
    public void Open(float amount)
    {
        if (isOpen) 
        {
            return;
        }
        gameObject.SetActive(true);
        isOpen = true;
        fillImage.fillAmount = 1;
        if (amount < 0) 
        {
            StartCoroutine(InfinityColorAnim());
        }
    }
    public void Close()
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;
        gameObject.SetActive(false);
    }
    private void ColorAnim(float amount) 
    {
        float indexAmount = 1f / normalColors.Count;
        int index =  (int)(amount/indexAmount);
        amount -= (indexAmount*index); 
        int nextIndex = Mathf.Min(index +1, normalColors.Count - 1);
        fillImage.color = Color.Lerp(normalColors[index], normalColors[nextIndex],amount/indexAmount);
    }
    private IEnumerator InfinityColorAnim()
    {
        float curTime = 0;
        int curIndex = 0;
        while (isOpen) 
        {
            curTime += Time.deltaTime;
            if (curTime >= colorTime)
            {
                curTime = 0;
                curIndex = (curIndex + 1) % infinityColors.Count;
            }
            int nextIndex = (curIndex + 1) % infinityColors.Count;
            fillImage.color = Color.Lerp(infinityColors[curIndex], infinityColors[nextIndex], curTime * (1 / colorTime));

            yield return null;
        }
    }

    public void GaugeUiUpdate(float amount) 
    {
        fillImage.fillAmount = amount;
        ColorAnim(amount);
    }

    public override void Init()
    {
    }
}
