using DG.Tweening;
using TMPro;
using UnityEngine;


public class KeyMappingStatusTextController : MonoBehaviour
{
    [SerializeField]
    private GameObject textGo;
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Color successColor;
    [SerializeField]
    private Color failColor;

    private readonly string SUCCESS_TEXT = "키 변경이 적용되었습니다.";
    private readonly string FAIL_TEXT = "중복되는 키가 있습니다.";
    private bool isPlay = false;
    private bool isSuccess = false;
    private Sequence sequence;
    private void Awake()
    {
        isPlay = false;
        isSuccess = false;
        sequence = null;
    }
    private void Start()
    {
        textGo.transform.localScale = Vector3.zero;
    }
    public void SetMappingSuccess(bool isOn) 
    {
        if (isPlay && isSuccess == isOn)
        {
            return;
        }
        isPlay = true;
        isSuccess = isOn;
        
        if (isSuccess) 
        {
            text.text = SUCCESS_TEXT;
            text.color = successColor;
        }
        else 
        {
            text.text = FAIL_TEXT;
            text.color = failColor;
        }
        WarningCoroutine();
    }
    public void WarningCoroutine()
    {
        if (sequence != null)
        {
            sequence.Kill();
        }
        textGo.SetActive(true);
        textGo.transform.localScale = new Vector3(1f, 1f, 1f);
        Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
        Transform warningTransfom = textGo.gameObject.transform;
        sequence = DOTween.Sequence().SetUpdate(true);
        sequence.Append(warningTransfom.DOScale(targetScale, 1f).SetEase(Ease.OutElastic));
        sequence.Join(warningTransfom.DOShakePosition(1f));
        sequence.AppendInterval(1.5f);
        sequence.AppendCallback(
            () =>
            {
                isPlay = false;
                textGo.gameObject.SetActive(false);
                sequence = null;
            }
        );
        sequence.Play();
    }

}
