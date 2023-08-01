using DG.Tweening;
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
    private PlayerKeywordLayoutTrigger trigger;
    [SerializeField]
    private CanvasGroup debugModGroup;

    private Dictionary<string,KeywordMakerGaugeController> keywordMakers = new Dictionary<string, KeywordMakerGaugeController>();

    public Transform LayoutParent { get => layoutParent; }
    public Transform DebugGaugePanel { get => debugGaugePanel; }

    private const float OPEN_ANIM_TIME = 0.5f;
    private const float CLOSE_ANIM_TIME = 0.5f;
    private const string KETWORD_MAKER_NAME = "KeywordMakerGauge";
    public override void Init()
    {

        
    }
    public void AddKeywordMakerGauge(string id, int amount) 
    {
        if (!keywordMakers.ContainsKey(id)) 
        {
            var maker =  Managers.UI.MakeSubItem<KeywordMakerGaugeController>(transform, KETWORD_MAKER_NAME);
            maker.name = $"KeywordMakerGauge_{id}";
            maker.SetTarget(id, 3);
            keywordMakers.Add(id,maker);
        }
        keywordMakers[id].AddCount(amount);
    }
    public void Open() 
    {
        DebugModGroupOpen();
        //gameObject.SetActive(true);
       // Managers.Keyword.CurDebugZone.OpenPlayerLayout();
    }
    public void ClearForPool()
    {
        foreach(var item in keywordMakers) 
        {
            item.Value.StopAllCoroutines();
        }
        //keywordMakerGaugeController.StopAllCoroutines();
    }
   
    public void Close() 
    {
        gameObject.SetActive(false);
    }


    public void DebugModGroupOpen()
    {
        debugModGroup.DOKill();
        var animTime = OPEN_ANIM_TIME * (1 - debugModGroup.alpha);
        debugModGroup.DOFade(1, animTime).SetUpdate(true).OnComplete(()=> DebugGroupInteractAble(true));
    }

    public void DebugModGroupClose()
    {
        DebugGroupInteractAble(false);
        debugModGroup.DOKill();
        var animTime = CLOSE_ANIM_TIME * debugModGroup.alpha;
        debugModGroup.DOFade(0, animTime).SetUpdate(true);
    }
    private void DebugGroupInteractAble(bool isOn) 
    {
        debugModGroup.interactable = isOn;
        debugModGroup.blocksRaycasts = isOn;
    }

}
