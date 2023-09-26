using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreditPanelController : MonoBehaviour
{
    [System.Serializable]
    public class CreditItem
    {
        public string creditTitle;
        public string creditName;
        public Sprite creditSprite;
    }

    [SerializeField]
    private List<CreditItem> creditItems = new List<CreditItem>();
    [SerializeField]
    private CanvasGroup txtCanvas;
    [SerializeField]
    private TMP_Text cTitle;
    [SerializeField]
    private TMP_Text cName;
    [SerializeField]
    private GameObject spritePrefab;

    private Transform parentTransform;
    private int curIndex = 0;

    private void Start()
    {
        txtCanvas = txtCanvas.GetComponent<CanvasGroup>();
        txtCanvas.alpha = 0f;
        parentTransform = transform;

        PlayNextAnimation();
    }

    private void PlayNextAnimation()
    {
        var textSequence = DOTween.Sequence();

        cTitle.text = creditItems[curIndex].creditTitle;
        cName.text = creditItems[curIndex].creditName;

        textSequence.Append(txtCanvas.DOFade(1f, 1.5f).SetEase(Ease.Linear));
        textSequence.AppendCallback(() => CreateBalls());
        textSequence.AppendInterval(2f);
        textSequence.Append(txtCanvas.DOFade(0f, 1.5f).SetEase(Ease.Linear)).OnComplete(() =>
        {
            curIndex++;
            if (curIndex < creditItems.Count)
            {
                PlayNextAnimation();
            }
        });
    }

    private void CreateBalls()
    {
        GameObject spriteInstance = Instantiate(spritePrefab, parentTransform);
        if (spriteInstance != null)
        {
            Image spriteImage = spriteInstance.GetComponent<Image>();
            spriteInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            spriteImage.sprite = creditItems[curIndex].creditSprite;
        }
    }
}
    