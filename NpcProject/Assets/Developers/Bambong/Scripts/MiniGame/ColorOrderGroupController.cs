using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class ColorOrderGroupController : UI_Base
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private CanvasGroup imageGroup;
    [SerializeField]
    private Transform resultNodeLayout;

    private List<ResultNodeController> orderNodes = new List<ResultNodeController>();
    private int curIndex = 0;
    private int answerKey;
    private MiniGameManager miniGameManager;
    public bool IsStart { get => curIndex > 0; } 
    public bool IsEnd { get => curIndex >= orderNodes.Count; }

    public bool CompareKey(string key)
    {
        return orderNodes[curIndex].AnswerKey == key; 
    }

    public void PushKey(string key) 
    {
        if (IsEnd) 
        {
            return;
        }
        if (!CompareKey(key)) 
        {
            if (!IsStart) 
            {
                return;
            }

            miniGameManager.SetStateGameReset();
            //orderNodes[curIndex].SetIsSuccess(fa);
            return;
        }
      
        orderNodes[curIndex].SetIsSuccess(true);
        curIndex++;

        if (IsEnd) 
        {
            miniGameManager.PushColorNode(answerKey);
        }
    }

    private ResultNodeController CreateResultNode()
    {
        var node = Managers.UI.MakeSubItem<ResultNodeController>(resultNodeLayout, "MiniGame/ResultNode");
        orderNodes.Add(node);
        return node;
    }
    public void InitData(MiniGameManager manager , MiniGameColorGroupData data)
    {
        image.color = manager.GetOrderKeyColor(data.Key);
        imageGroup.alpha = 0;
        miniGameManager = manager;
        string[] splitKey = data.OrderKey.Split(',');
        answerKey = data.Key;
        for (int i =0; i < splitKey.Length; ++i) 
        {
            var temp = CreateResultNode();
            temp.SetKey(splitKey[i]);
            temp.ClearSize();
        }

    }

    public void OpenAnim(float interval) 
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(interval);
        seq.Append(imageGroup.DOFade(1, 2f));
        seq.Play();
        for (int i =0; i< orderNodes.Count; ++i) 
        {
            orderNodes[i].OpneAnim(interval);
            interval += 0.1f;
        }
    }
    public void CloseAnim(float interval) 
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(interval);
        seq.Append(imageGroup.DOFade(0, 0.5f));
        seq.Play();
        for (int i = 0; i < orderNodes.Count; ++i)
        {
            orderNodes[i].CloseAnim(interval);
            interval += 0.1f;
        }
    }
    public void ResetOrderGroup() 
    {
        curIndex = 0;
        for (int i = 0; i < orderNodes.Count; ++i)
        {
            orderNodes[i].ResetNode();
        }
    }

    public void PointerEnter(string key) 
    {
        if (IsEnd)
        {
            return;
        }
        if (CompareKey(key)) 
        {
            orderNodes[curIndex].SetOutline(true);
        }
    }
    public void PointerExit(string key) 
    {
        if (IsEnd)
        {
            return;
        }
        if (CompareKey(key))
        {
            orderNodes[curIndex].SetOutline(false);
        }
    }
    public override void Init()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

    }
}
