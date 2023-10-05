using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

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
    [SerializeField]
    private CircleCollider2D circleArea;

    private Transform parentTransform;

    private int curIndex = 0;
    private float animSpeed = 1f;
    private float minValue = 1f;
    private float maxValue = 2f;
    private float progress = 0f;
    private float distance;

    private void Start()
    {
        txtCanvas = txtCanvas.GetComponent<CanvasGroup>();
        txtCanvas.alpha = 0f;
        parentTransform = transform;
        circleArea.radius = Mathf.Sqrt((Screen.width) * (Screen.width) + (Screen.height) * (Screen.height)) + 100f;
        distance = circleArea.radius;

        //PlayNextAnimation();
    }

    private void FixedUpdate()
    {
        Debug.Log(animSpeed);

        bool isSpacePressed = Input.GetKey(KeyCode.Space);

        if (isSpacePressed && progress < 1f)
        {
            progress += Time.deltaTime;
        }
        else if (!isSpacePressed && progress > 0f)
        {
            progress -= Time.deltaTime;
        }

        animSpeed = Mathf.Lerp(minValue, maxValue, progress);
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    public void PlayNextAnimation()
    {
        var textSequence = DOTween.Sequence();

        cTitle.text = creditItems[curIndex].creditTitle;
        cName.text = creditItems[curIndex].creditName;

        textSequence.Append(txtCanvas.DOFade(1f, 1.5f / animSpeed).SetEase(Ease.Linear));
        textSequence.AppendCallback(() => CreateBalls());
        textSequence.AppendInterval(2f / animSpeed);
        textSequence.Append(txtCanvas.DOFade(0f, 1.5f / animSpeed).SetEase(Ease.Linear)).OnComplete(() =>
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
        float angle = UnityEngine.Random.Range(0f, 360f);
        Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * distance;

        GameObject spriteInstance = Instantiate(spritePrefab, parentTransform);
        if (spriteInstance != null)
        {
            Image spriteImage = spriteInstance.GetComponent<Image>();
            spriteInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            spriteImage.sprite = creditItems[curIndex].creditSprite;
        }
    }
}
    