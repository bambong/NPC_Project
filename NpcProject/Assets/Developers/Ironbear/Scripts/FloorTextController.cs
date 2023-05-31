using UnityEngine;
using DG.Tweening;
using TMPro;

public class FloorTextController : MonoBehaviour
{
    [TextArea]
    [SerializeField]
    private string[] texts;
    [SerializeField]
    private TMP_Text[] floorTexts;

    private Sequence seq;
    private float typeSpeed = 0.8f;

    private void Start()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            floorTexts[i].text = "";
        }
        seq = DOTween.Sequence();
        TypeAnimation();
        Managers.Keyword.OnEnterDebugModEvent += CloseText;
        Managers.Keyword.OnExitDebugModEvent += OpenText;
    }
    private void OpenText() 
    {
        for (int i = 0; i < texts.Length; i++)
        {
            floorTexts[i].DOKill();
            floorTexts[i].text = texts[i];
            floorTexts[i].DOFade(1, 1f);
        }
    }
    private void CloseText() 
    {
        for (int i = 0; i < texts.Length; i++)
        {
            floorTexts[i].DOKill();
            floorTexts[i].DOFade(0, 1f);
        }
    }
    private void TypeAnimation()
    {
        seq.AppendInterval(1f);
        for (int i = 0; i < texts.Length; i++)
        {
            string text = texts[i];
            text = text.Replace("\\n", "\n");

            seq.Append(floorTexts[i].DOText(text, typeSpeed).SetEase(Ease.Linear));
            seq.Play();
        }
        
    }
}
