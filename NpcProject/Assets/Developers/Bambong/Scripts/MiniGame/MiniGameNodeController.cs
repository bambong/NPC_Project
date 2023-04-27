using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MiniGameNodeController : UI_Base ,IPointerEnterHandler ,IPointerClickHandler ,IPointerExitHandler , IPointerDownHandler
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Image image;
    [SerializeField]
    private RectTransform rectTransform;
    

    [ColorUsage(true, true)]
    [SerializeField]
    private Color pickColor;
    [ColorUsage(true, true)]
    [SerializeField]
    private Color enableColor;
    [ColorUsage(true, true)]
    [SerializeField]
    private Color disableColor;
    [ColorUsage(true, true)]
    [SerializeField]
    private Color lookUpColor;

    private MiniGameManager miniGameManager;
    private Vector2Int posIndex;
    private string answerKey;
    private bool isAbailable = true;
    private bool isDelete = false;

    private Material myMat;
    private readonly string GLOW_COLOR_PROPERTY = "_GlowColor";
    public Vector2Int PosIndex { get => posIndex; }
    public string AnswerKey { get => answerKey;  }
    public override void Init()
    {
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.zero;
        
    }
    public void OpenAnim(float interval) 
    {
        isDelete = false;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(interval);
        sequence.Append(rectTransform.DOScale(1, 0.2f));
        sequence.Play();
        SetImageColor(enableColor);
    }
    
    public void SetLookUpmod() 
    {
        text.color = Color.white;
        SetImageColor(lookUpColor);
    }
    public void ClearLookUpmod() 
    {

        UpdateAvailableColor();
    }
    public void SetData(MiniGameManager manager , Vector2Int pos , string key) 
    {
        answerKey = key;
        text.text = key;
        miniGameManager = manager;
        posIndex = pos;
    }
    public void EnableNode()
    {
        if (isDelete) 
        {
            return;
        }
        isAbailable = true;
        UpdateAvailableColor();
    }
    public void DisableNode()
    {
        if (isDelete)
        {
            return;
        }
        isAbailable = false;
        UpdateAvailableColor();
    }
    public void UpdateAvailableColor()
    {
        if (isAbailable) 
        {
            text.color = Color.white;
            SetImageColor(enableColor);
        }
        else 
        {
            text.color = new Color(0.3f,0.3f,0.3f,1);
            SetImageColor(disableColor);
        }

    }
    public void CloseAnim(float interval)
    {
        if(isDelete)
        {
            return;
        }
        isDelete = true;
        isAbailable = false;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(interval);
        sequence.Append(rectTransform.DOScale(0, 0.2f));
        sequence.Play();
    }
    private void SetImageColor(Color color) 
    {
        //myMat.SetColor(GLOW_COLOR_PROPERTY, color);
        image.color = color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isAbailable) 
        {
            return;
        }
        SetImageColor(pickColor);
        miniGameManager.PointEnter(posIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isAbailable) 
        {
            return;
        }
        miniGameManager.PointExit(posIndex);
        miniGameManager.ClickNode(this);
        CloseAnim(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isAbailable)
        {
            return;
        }
        miniGameManager.PointExit(posIndex);
        UpdateAvailableColor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
