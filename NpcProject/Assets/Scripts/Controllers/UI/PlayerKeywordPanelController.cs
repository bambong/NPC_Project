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
    [SerializeField]
    private PlayerKeywordLayoutTrigger trigger;
    

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
        trigger.gameObject.SetActive(true);
        //gameObject.SetActive(true);
        Managers.Keyword.CurDebugZone.OpenPlayerLayout();

    }
    public void ClearForPool()
    {
        keywordMakerGaugeController.StopAllCoroutines();
    }
    public void TriggerClose() 
    {
        trigger.gameObject.SetActive(false);
    }
    public void Close() 
    {
        gameObject.SetActive(false);
    }
}
