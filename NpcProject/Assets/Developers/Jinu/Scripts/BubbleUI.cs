using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class BubbleUI :  MonoBehaviour
{
    private readonly float Y_POS_REVISION_AMOUNT = 2f;
    private readonly string DEFALUT_TEXT = ". . .";
    [SerializeField]
    private CanvasGroup bubbleUI;
    [SerializeField]
    private CanvasGroup bubbleDefalutUI;
    [SerializeField]
    private TextMeshProUGUI bubbleTextUI;
    [SerializeField]
    private TextMeshProUGUI bubbleDefalutTextUI;
    [SerializeField]
    private List <string> bubbleText;

    private GameObject parent;

    private bool isStart = true;
    private bool isEnter = false;
    private int count = 0;

    public void Start()
    {
        //parent = transform.parent.gameObject;
        //transform.position = parent.transform.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y / 2) * Y_POS_REVISION_AMOUNT);
        bubbleDefalutUI.alpha = 1;
        bubbleUI.alpha = 0;
        
        //if (parent.transform.localScale.x < 0)
        //{
        //    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        //}
    }

    public void Update()
    {

        //transform.rotation = Camera.main.transform.rotation;
        if (isStart && !isEnter)
        {
            isStart = false;
            bubbleDefalutTextUI.DOText(DEFALUT_TEXT, 2.0f).OnStart(()=>
            {
            }).OnComplete(()=>
            {
                bubbleDefalutTextUI.text = "";
                isStart = true;
            });
        }
    }

    public void OpenBubbleUI()
    {
        bubbleDefalutTextUI.DOKill();
        isEnter = true;
        bubbleTextUI.text = "";

        bubbleDefalutUI.alpha = 0;
        bubbleUI.transform.DOScale(0f, 0f).OnStart(() =>
        {
            bubbleUI.alpha = 1;
            bubbleUI.transform.DOScale(0.02f, 0.2f);
        }).OnComplete(() =>
        {
            MoveNext();
        });
    }

    public void CloseBubbleUI()
    {
        isEnter = false;
        bubbleUI.alpha = 0;
        bubbleDefalutTextUI.text = "";
        bubbleDefalutTextUI.DOKill();
        bubbleDefalutUI.alpha = 1;
        isStart = true;
        bubbleTextUI.text = "";
        bubbleTextUI.DOKill();
    }

    private void  MoveNext()
    {
        if (count < bubbleText.Count)
        {
            bubbleTextUI.DOText(bubbleText[count], 2f).OnStart(()=>
            {                    
            }).OnComplete(() =>
            {
                bubbleTextUI.text = "";
                count++;
                MoveNext();
            });
        }
        else
        {
            count = 0;
            if (isEnter == true)
            {
                MoveNext();
            }
        }
    }
}
