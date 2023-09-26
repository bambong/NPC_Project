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

    private Transform parentTransform;

    private int curIndex = 0;
    private float animSpeed = 1f;
    private float minValue = 1f;
    private float maxValue = 2f;
    private float progress = 0f;

    private void Start()
    {
        txtCanvas = txtCanvas.GetComponent<CanvasGroup>();
        txtCanvas.alpha = 0f;
        parentTransform = transform;

        PlayNextAnimation();
    }

    private void FixedUpdate()
    {
        Debug.Log(animSpeed);
        //if (Input.GetKey(KeyCode.Space) && !isIncreasing)
        //{
        //    isIncreasing = true;
        //    animSpeed = maxValue;
        //}
        //else if (!Input.GetKey(KeyCode.Space) && isIncreasing)
        //{
        //    isIncreasing = false;
        //    animSpeed = minValue;
        //}

        bool isSpacePressed = Input.GetKey(KeyCode.Space);

        if (isSpacePressed && progress < 1f)
        {
            // 스페이스바를 누르고 있고, 아직 최대값에 도달하지 않았다면
            progress += Time.deltaTime; // progress를 증가시켜서 전환을 부드럽게 함
        }
        else if (!isSpacePressed && progress > 0f)
        {
            // 스페이스바를 떼고 있고, 아직 최소값에 도달하지 않았다면
            progress -= Time.deltaTime; // progress를 감소시켜서 전환을 부드럽게 함
        }

        // minValue에서 maxValue로 보간
        animSpeed = Mathf.Lerp(minValue, maxValue, progress);
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    private void PlayNextAnimation()
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
        GameObject spriteInstance = Instantiate(spritePrefab, parentTransform);
        if (spriteInstance != null)
        {
            Image spriteImage = spriteInstance.GetComponent<Image>();
            spriteInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            spriteImage.sprite = creditItems[curIndex].creditSprite;
        }
    }
}
    