using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class BubbleUI :  MonoBehaviour
{
    private readonly float Y_POS_REVISION_AMOUNT = 1;
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

    private bool isStart = true;
    private bool isEnter;
    private int count = 0;

    public void Start()
    {
        bubbleDefalutUI.alpha = 1;
        bubbleDefalutUI.transform.position = transform.position + Vector3.up * (transform.position.y + Y_POS_REVISION_AMOUNT);
        bubbleDefalutUI.transform.rotation = Camera.main.transform.rotation;

        bubbleUI.alpha = 0;
        bubbleUI.transform.position = transform.position + Vector3.up * (transform.position.y + Y_POS_REVISION_AMOUNT);
        bubbleUI.transform.rotation = Camera.main.transform.rotation;
    }

    public void Update()
    {
        bubbleDefalutUI.transform.rotation = Camera.main.transform.rotation;
        if(isStart)
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

    private void OnTriggerEnter(Collider other)
    {
        bubbleDefalutTextUI.DOKill();
        isEnter = true;
        if(other.tag == "Player")
        {
            bubbleDefalutUI.alpha = 0;
            bubbleUI.transform.DOScale(0f, 0f).OnStart(() =>
            {
                bubbleUI.alpha = 1;
                bubbleUI.transform.DOScale(0.02f, 0.2f);
            }).OnComplete(()=>
            {
                MoveNext();
            });                   
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            bubbleUI.transform.rotation = Camera.main.transform.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isEnter = false;
        bubbleUI.alpha = 0;
        bubbleDefalutTextUI.text = "";
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
