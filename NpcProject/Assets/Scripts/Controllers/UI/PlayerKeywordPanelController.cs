using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKeywordPanelController : UI_Base
{
    [SerializeField]
    private Transform layoutParent;
    [SerializeField]
    private Transform debugGaugePanel; 
    [SerializeField]
    private KeywordMakerGaugeController keywordMakerGaugeController;

    public Transform LayoutParent { get => layoutParent; }
    public Transform DebugGaugePanel { get => debugGaugePanel; }

    public override void Init()
    {
        
    }
    public void AddKeywordMakerGauge(int amount) 
    {
        keywordMakerGaugeController.AddCount(amount);
    }
    public void Open() 
    {
        //gameObject.SetActive(true);
        Managers.Keyword.CurDebugZone.OpenPlayerLayout();

    }
    public void ClearForPool()
    {
        keywordMakerGaugeController.StopAllCoroutines();
    }
    public void Close() 
    {
        gameObject.SetActive(false);
    }
}
