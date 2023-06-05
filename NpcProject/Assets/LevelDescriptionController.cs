using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDescriptionController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textUi;
    [SerializeField]
    private Image panel;
    [SerializeField]
    private float startDelay = 1f;
    [SerializeField]
    private float animTime = 1.5f;
    [SerializeField]
    private float fontDiff = 0.7f;


    [TextArea]
    [SerializeField]
    private string text;
    private readonly float constantDiff = 15f;
    // Start is called before the first frame update

    void Start()
    {
        text = textUi.text;
        textUi.text = "";
        panel.rectTransform.sizeDelta = new Vector2(0, panel.rectTransform.sizeDelta.y);
        var seq = DOTween.Sequence();
        seq.AppendInterval(startDelay);
        seq.Append(textUi.DOText(text, animTime));
        seq.Join(panel.rectTransform.DOSizeDelta(new Vector2(text.Length * textUi.fontSize * fontDiff + constantDiff, panel.rectTransform.sizeDelta.y), animTime));
        seq.Play();
    }

}
