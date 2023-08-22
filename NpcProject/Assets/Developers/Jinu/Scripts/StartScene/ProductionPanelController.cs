using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProductionPanelController : MonoBehaviour
{

    [SerializeField]
    private Image[] CharImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField][TextArea]
    private string[] createrName;


    public void Open()
    {
        var seq = DOTween.Sequence();

        for (int i = 0; i < createrName.Length; i++)
        {
            nameText.text = createrName[i];
            seq.Append(CharImage[i].DOFade(1.0f, 1.0f));
            seq.Append(nameText.DOFade(1.0f, 1.0f));
            seq.AppendInterval(1.0f);
            seq.Append(CharImage[i].DOFade(0.0f, 1.0f));
            seq.Append(nameText.DOFade(0.0f, 1.0f));
            seq.AppendInterval(1.0f);
        }

    }
}
