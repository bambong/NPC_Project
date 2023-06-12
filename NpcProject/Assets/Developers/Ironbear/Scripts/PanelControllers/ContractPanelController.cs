using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContractPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject contract;
    [SerializeField]
    private GameObject contractImage;
    [SerializeField]
    private CanvasGroup signCanvasGroup;
    [SerializeField]
    private GameObject inputField;
    [SerializeField]
    private GameObject kakaoPanel;
    [SerializeField]
    private GameObject clickMouse;

    private CanvasGroup contractCanvasGroup;
    private CanvasGroup inputFiledCanvasGroup;
    private CanvasGroup clickMouseCanvasGroup;
    private Image contractColor;

    private float effectDuration = 1f;
    private float targetOn = 0f;
    private float targetOff = -100f;
    private float delayTime = 3f;

    private void Start()
    {
        Managers.Game.Player.SetstateStop();

        clickMouseCanvasGroup = clickMouse.GetComponent<CanvasGroup>();
        contractCanvasGroup = contract.GetComponent<CanvasGroup>();
        contractColor = contractImage.GetComponent<Image>();
        inputFiledCanvasGroup = inputField.GetComponent<CanvasGroup>();
        clickMouseCanvasGroup.alpha = 0f;
        inputFiledCanvasGroup.alpha = 0f;
        contractCanvasGroup.alpha = 0f;

        inputField.SetActive(false);
        kakaoPanel.SetActive(false);

        signCanvasGroup.blocksRaycasts = false;

        DOVirtual.DelayedCall(delayTime, ContractOnEffect);
    }

    public void ClickSignBtn()
    {
        Managers.Sound.PlaySFX(Define.SOUND.ClickButton);
        Color targetColor = new Color(0.67f, 0.67f, 0.67f, 1f);

        signCanvasGroup.DOFade(0f, effectDuration);
        contractColor.DOColor(targetColor, effectDuration);

        InputFieldOnEffect();
    }

    private void InputFieldOnEffect()
    {
        inputField.SetActive(true);
        inputFiledCanvasGroup.DOFade(1f, effectDuration);

        RectTransform inputFieldRectTransform = inputField.GetComponent<RectTransform>();
        inputFieldRectTransform.DOAnchorPosY(targetOn, effectDuration);
    }

    private void ContractOnEffect()
    {
        contract.SetActive(true);
        contractCanvasGroup.DOFade(1f, effectDuration).OnComplete(() =>
        {
            //click mouse active and animation effect
            Sequence clickMouseSeq = DOTween.Sequence();
            float originalYPos = clickMouse.transform.localPosition.y;
            clickMouseSeq.Append(clickMouseCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad));
            clickMouseSeq.Append(clickMouse.transform.DOLocalMoveY(originalYPos + 100f, 2f).SetEase(Ease.OutQuad));
            clickMouseSeq.Join(clickMouseCanvasGroup.DOFade(0f, 2f).SetEase(Ease.OutQuad));
            clickMouseSeq.Play().OnComplete(() =>
            {
                signCanvasGroup.blocksRaycasts = true;
            });
        });

        RectTransform contractRectTransform = contract.GetComponent<RectTransform>();
        contractRectTransform.DOAnchorPosY(targetOn, effectDuration);
    }

    private void InputFieldOffEffect(System.Action onComplete = null)
    {
        Color targetColor = new Color(1f, 1f, 1f, 1f);
        contractColor.DOColor(targetColor, effectDuration);

        RectTransform inputFieldRectTransform = inputField.GetComponent<RectTransform>();
        inputFieldRectTransform.DOAnchorPosY(targetOff, effectDuration);

        inputFiledCanvasGroup.DOFade(0f, effectDuration).OnComplete(() =>
        {
            inputField.SetActive(false);
            onComplete?.Invoke();
        });
        
    }

    private void ContractOffEffect(System.Action onComplete = null)
    {
        RectTransform contractRectTransform = contract.GetComponent<RectTransform>();
        contractRectTransform.DOAnchorPosY(targetOff, effectDuration);

        contractCanvasGroup.DOFade(0f, effectDuration).OnComplete(() =>
        {
            contract.SetActive(false);
            onComplete?.Invoke();
        });        
    }

    public void UiOff()
    {
        InputFieldOffEffect(() =>
        {
            ContractOffEffect(()=>
            {                
                kakaoPanel.SetActive(true);
            });           
        });
    }

    public void Noninteractive()
    {
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Image clickedImage = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        CanvasGroup clickedCanvasGroup = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<CanvasGroup>();

        clickedCanvasGroup.interactable = false;
        clickedCanvasGroup.blocksRaycasts = false;
        clickedImage.raycastTarget = false;
        clickedButton.interactable = false;
    }
}
