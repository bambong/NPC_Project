using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CreditPanelController : UI_Base
{
    [System.Serializable]
    public class CreditItem
    {
        public string creditTitle;
        public string creditName;
        public Sprite creditSprite;
    }

    [HideInInspector]
    public static float animSpeed { get; set; } = 1f;

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
    private BoxCollider2D boxArea;
    [SerializeField]
    private GameObject makingBtn;

    private Transform parentTransform;
    private Sequence textSequence;

    private int curIndex = 0;
    private float minValue = 1f;
    private float maxValue = 3f;
    private float progress = 0f;

    

    private void Start()
    {
        txtCanvas = txtCanvas.GetComponent<CanvasGroup>();
        txtCanvas.alpha = 0f;
        parentTransform = transform;
        boxArea.size = new Vector2(Screen.width - 200f, Screen.height + 1800f);
    }

    public override void Init()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivatedCredit();
        }
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

    public void PlayNextAnimation()
    {
        textSequence = DOTween.Sequence();

        cTitle.text = creditItems[curIndex].creditTitle;
        cName.text = creditItems[curIndex].creditName;

        
        textSequence.Append(txtCanvas.DOFade(1f, 1.5f / animSpeed).SetEase(Ease.Linear));
        textSequence.AppendCallback(() => CreateBalls());
        textSequence.AppendInterval(5f / animSpeed);
        textSequence.Append(txtCanvas.DOFade(0f, 1.5f / animSpeed).SetEase(Ease.Linear)).OnComplete(() =>
        {
            curIndex++;
            if (curIndex < creditItems.Count)
            {
                PlayNextAnimation();
            }
            else
            {
                DeactivatedCredit();
            }            
        });

        textSequence.Play();
    }

    private void CreateBalls()
    {
        GameObject spriteInstance = Instantiate(spritePrefab, parentTransform);
        if (spriteInstance != null)
        {
            Image spriteImage = spriteInstance.GetComponent<Image>();
            float randomX = UnityEngine.Random.Range(-Screen.width / 2, Screen.width / 2);
            spriteInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            spriteInstance.transform.localPosition = new Vector3(randomX, -(Screen.height / 2 + 150f), 0f);
            spriteImage.sprite = creditItems[curIndex].creditSprite;
        }
    }

    private void DeactivatedCredit()
    {
        textSequence.Kill();

        DeleteClones();

        CanvasGroup creditCanvasGroup = this.GetComponent<CanvasGroup>();
        creditCanvasGroup.DOFade(0f, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            var canvasGroup = makingBtn.GetComponent<CanvasGroup>();
            var button = makingBtn.GetComponent<Button>();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            button.interactable = true;

            curIndex = 0;
            txtCanvas.alpha = 0f;

            this.gameObject.SetActive(false);
            DOTween.Clear(this);
        });
    }

    private void DeleteClones()
    {
        GameObject[] clones = GameObject.FindGameObjectsWithTag(spritePrefab.tag);

        foreach(GameObject clone in clones)
        {
            Destroy(clone);
        }
    }
}
    