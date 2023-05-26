using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContractPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject contract;
    [SerializeField]
    private CanvasGroup signCanvasGroup;
    [SerializeField]
    private GameObject inputField;
    [SerializeField]
    private GameObject kakaoPanel;

    private CanvasGroup contractCanvasGroup;
    private CanvasGroup inputFiledCanvasGroup;
    private Image contractImage;

    private float effectDuration = 1f;
    private float targetOn = 0f;
    private float targetOff = -100f;
    private float delayTime = 4.5f;

    private void Start()
    {
        Managers.Game.Player.SetstateStop();

        contractCanvasGroup = contract.GetComponent<CanvasGroup>();
        contractImage = contract.GetComponent<Image>();
        inputFiledCanvasGroup = inputField.GetComponent<CanvasGroup>();
        inputFiledCanvasGroup.alpha = 0f;
        contractCanvasGroup.alpha = 0f;

        inputField.SetActive(false);
        kakaoPanel.SetActive(false);

        DOVirtual.DelayedCall(delayTime, ContractOnEffect);
    }

    public void ClickSignBtn()
    {
        Color targetColor = new Color(170f / 255f, 170f / 255f, 170f / 255f, 1f);

        signCanvasGroup.DOFade(0f, effectDuration);
        contractImage.DOColor(targetColor, effectDuration);

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
                Managers.Game.Player.SetStateIdle();
                kakaoPanel.SetActive(true);
            });           
        });
    }
}
