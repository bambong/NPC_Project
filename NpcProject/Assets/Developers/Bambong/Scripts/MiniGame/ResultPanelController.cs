using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.Analytics;

public class ResultPanelController : MonoBehaviour
{
    [SerializeField]
    private MiniGameManager miniGameManager;

    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private TextMeshProUGUI codeText;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [TextArea]
    [SerializeField]
    private string successText;

    [SerializeField]
    private string failTitleText;
    [SerializeField]
    private string successTitleText;

    [SerializeField]
    private int maxShowCount = 20;

    [SerializeField]
    private float panelSuccessMoveY;
    [SerializeField]
    private float successTextTime = 2f;
    [SerializeField]
    private float titleTextAnimTime = 1.5f;

    [SerializeField]
    private List<ResultButtonController> resultButtons;

    [SerializeField]
    private string retrybutText;
    [SerializeField]
    private string exitbutText;

    [SerializeField]
    private Color successButtonColor;
    [SerializeField]
    private Color failButtonColor;

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
                rect.DOAnchorPosY(rect.anchoredPosition.y + moveHight, moveTime).SetEase(Ease.Linear);
            }
            codeText.DOText(curStr.ToString(), textTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(textTime);
        }
        Sequence seq = DOTween.Sequence();
        seq.Append(codeText.DOFade(0, 1f));
       // seq.Append(resultText.DOText("··", 1f));
      //  seq.Append(resultText.DOText("···", 0.5f).SetLoops(3,LoopType.Yoyo));
        //seq.Append(resultText.DOText(successTitleText, titleTextAnimTime));
        seq.AppendCallback(() => StartCoroutine(PlayTextAnimation(successTitleText, titleTextAnimTime)));
        seq.AppendInterval(titleTextAnimTime+0.3f);
        seq.Append(resultText.DOFade(0, 0));
        seq.AppendCallback(() => {
            Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleClear);
            OnSuccessButtonSet();  });
        seq.Append(resultText.DOFade(1, 0.2f).SetLoops(2));
        seq.Play();
    }
    private void OnSuccessButtonSet() 
    {
        resultButtons[0].Open(0.2f, exitbutText,successButtonColor, () => { OnExitButtonClick(true); });
        resultButtons[1].Clear();
    }
    private void OnFailButtonSet()
    {
        resultButtons[0].Open(0.2f, retrybutText,failButtonColor, () => { OnRetryButtonClick(); });
        resultButtons[1].Open(0.3f, exitbutText,failButtonColor, () => { OnExitButtonClick(false); });
      
    }
    public void OnRetryButtonClick() 
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        resultButtons[0].Close(0);
        resultButtons[1].Close(0.1f, miniGameManager.ResetPuzzle);
    }
    public void OnExitButtonClick(bool isSuccess)
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        resultButtons[0].Close(0);
        resultButtons[1].Close(0.1f, () => 
        {
            if (isSuccess)
            {
                Managers.Data.ClearEvent(miniGameManager.MiniGameLevelData.guId);
            }
            var data = Managers.Data.LastGameData;
            Managers.Scene.LoadScene(data.sceneName);
        });
    }
    public void OnFail(float interval) 
    {
        Sequence seq = DOTween.Sequence();
        resultText.color = Color.red;
        resultText.text = "";
        seq.AppendInterval(interval);
        seq.AppendCallback(() => Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleFail));
       //seq.AppendInterval(3f);
        seq.Append(resultText.DOText(RandomText(failTitleText.Length)
            , 2.5f));
        //seq.Append(resultText.DOText("···", 0.5f).SetLoops(3, LoopType.Yoyo));
        //seq.Append(resultText.DOText( failTitleText, titleTextAnimTime));
        seq.AppendCallback(()=>StartCoroutine(PlayTextAnimation(failTitleText, titleTextAnimTime)));
        seq.AppendInterval(titleTextAnimTime+0.3f);
        seq.Append(resultText.DOFade(0, 0));
        seq.AppendCallback(() => { OnFailButtonSet(); });
        seq.Append(resultText.DOFade(1, 0.2f).SetLoops(2));
        seq.Play();
       ;
    }
    public void ClearText() 
    {
        rect.anchoredPosition = Vector2.zero;
        codeText.DOFade(1, 0);
        resultText.text = "";
        codeText.text = "";
    }
    IEnumerator PlayTextAnimation(string text, float time)
    {
        string textStore = null;
        float textSpeed = time / text.Length;
        for(int i =0; i < text.Length; ++i)  
        {
            textStore += text[i];
            resultText.text = textStore + RandomText(text.Length - textStore.Length);
            Managers.Sound.PlaySFX(Define.SOUND.TextSound);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private string RandomText(int length)
    {
        var random = new System.Random();
        string charcters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return   new string(Enumerable.Repeat(charcters, length).Select(s => s[random.Next(s.Length)]).ToArray()) ;
    }
}
