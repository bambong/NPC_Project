using DG.Tweening;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class ResultTextController : MonoBehaviour
{


    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private TextMeshProUGUI codeText;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [TextArea]
    [SerializeField]
    private string successText;
    [TextArea]
    [SerializeField]
    private string failText;

    [SerializeField]
    private int maxShowCount = 20;

    [SerializeField]
    private float panelSuccessMoveY;
    [SerializeField]
    private float successTextTime = 2f;
    [SerializeField]
    private float failTextTime = 2f;

    private float height;
   
    private void Start()
    {
        height = rect.sizeDelta.y;
    }

    public void OnSuccess(float interval) 
    {

        rect.anchoredPosition = Vector2.zero;
        resultText.color = Color.green;
        StartCoroutine(SuccessAnim(interval));
        //Sequence seq = DOTween.Sequence();
        //seq.AppendInterval(interval);
        //seq.Append(codeText.DOText(successText, successTextTime).SetEase(Ease.Linear));
        //seq.Join(codeText.rectTransform.DOAnchorPosY(panelSuccessMoveY,successTextTime).SetEase(Ease.Linear));
        //seq.Append(codeText.DOFade(0, 0.5f));
        //seq.Append(resultText.DOText("SUCCESS", 0.5f)); ;
        //seq.Append(resultText.DOFade(0, 0)); ;
        //seq.Append(resultText.DOFade(1, 0.2f).SetLoops(2));
        //seq.Play();
    }

    private IEnumerator SuccessAnim(float interval) 
    {
        float moveHight = height / maxShowCount;
        yield return new WaitForSeconds(interval);
        var splitStrs = successText.Split('\n');
        StringBuilder curStr = new StringBuilder();
        float textTime = successTextTime / splitStrs.Length;
        float moveTime = textTime/4;
        for (int i =0; i < splitStrs.Length; ++i) 
        {
            curStr.Append(splitStrs[i]);
            curStr.Append('\n');
            if(i >= maxShowCount) 
            {
                //var pos = rect.anchoredPosition;
                //pos.y += moveHight;
                //rect.anchoredPosition = pos;
                rect.DOAnchorPosY(rect.anchoredPosition.y + moveHight, moveTime).SetEase(Ease.Linear);
            }
            codeText.DOText(curStr.ToString(), textTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(textTime);
        }
        Sequence seq = DOTween.Sequence();
        seq.Append(codeText.DOFade(0, 0.5f));
        seq.Append(resultText.DOText("SUCCESS", 0.5f)); ;
        seq.Append(resultText.DOFade(0, 0)); ;
        seq.Append(resultText.DOFade(1, 0.2f).SetLoops(2));
        seq.Play();
    }

    public void OnFail(float interval) 
    {
        Sequence seq = DOTween.Sequence();
        resultText.color = Color.red;
        seq.AppendInterval(interval);
        seq.Append(resultText.DOText(failText, failTextTime));
        seq.Append(resultText.DOFade(0, 0));
        seq.Append(resultText.DOFade(1, 0.2f).SetLoops(2));
        seq.Play();
       ;
    }
    public void ClearText() 
    {
        resultText.text = "";
        codeText.text = "";
    }
}
