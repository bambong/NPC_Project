using UnityEngine;
using TMPro;
using DG.Tweening;

public class StartPanelController : UI_Base
{
    [SerializeField]
    private string[] texts;
    [SerializeField]
    private TMP_Text tmpText;

    private float typeSpeed = 0.1f;
    private Sequence seq;


    public override void Init()
    {
        
    }

    private void Start()
    {
        seq = DOTween.Sequence();

        TypeAnimation();
    }

    private void TypeAnimation()
    {
        for (int j = 0; j < texts.Length; j++)
        {
            seq.Append(tmpText.DOText(texts[j], texts[j].Length * typeSpeed));
            seq.AppendInterval(1.0f);
            seq.OnComplete(() =>
            {
                tmpText.text = " ";
            });
        }
        seq.Play();
    }


}
