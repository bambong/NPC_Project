using UnityEngine;
using DG.Tweening;
using TMPro;

public class FloorTextController : MonoBehaviour
{
    [SerializeField]
    private string[] texts;
    [SerializeField]
    private TMP_Text[] floorTexts;

    private Sequence seq;
    private float typeSpeed = 0.8f;

    private void Start()
    {
        seq = DOTween.Sequence();
        TypeAnimation();
    }

    private void TypeAnimation()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            string text = texts[i];

            seq.Append(floorTexts[i].DOText(text, typeSpeed).SetEase(Ease.Linear)).OnComplete(() =>
            {

            });
            seq.Play();
        }
    }
}
