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
    private GameObject mouse;

    private CanvasGroup contractCanvasGroup;
    private CanvasGroup inputFiledCanvasGroup;
    private Image contractColor;

    private float effectDuration = 1f;
    private float targetOn = 0f;
    private float targetOff = -100f;
    private float delayTime = 2.5f;

    private void Start()
    {
        Managers.Game.Player.SetstateStop();

        contractCanvasGroup = contract.GetComponent<CanvasGroup>();
        contractColor = contractImage.GetComponent<Image>();
        inputFiledCanvasGroup = inputField.GetComponent<CanvasGroup>();

        inputFiledCanvasGroup.alpha = 0f;
        contractCanvasGroup.alpha = 0f;
        mouse.GetComponent<CanvasGroup>().alpha = 0f;
        signCanvasGroup.blocksRaycasts = false;

        inputField.SetActive(false);
        kakaoPanel.SetActive(false);

        ContractOnEffect();

        Sequence openSeq = DOTween.Sequence();
        openSeq.PrependInterval(delayTime / 2);
        openSeq.Append(mouse.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetEase(Ease.OutQuad));
        openSeq.Join(mouse.transform.DOLocalMoveY(mouse.transform.localPosition.y + 80f, effectDuration).SetEase(Ease.OutQuad));
        openSeq.Play().OnComplete(() =>
        {
            signCanvasGroup.blocksRaycasts = true;
        });
    }

    public void ClickSignBtn()
    {
        Managers.Sound.PlaySFX(Define.SOUND.ClickButton);
        Color targetColor = new Color(0.67f, 0.67f, 0.67f, 1f);

        signCanvasGroup.DOFade(0f, effectDuration);
        contractColor.DOColor(targetColor, effectDuration);
        mouse.GetComponent<CanvasGroup>().DOFade(0f, effectDuration);

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
        contractCanvasGroup.DOFade(1f, effectDuration);

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
