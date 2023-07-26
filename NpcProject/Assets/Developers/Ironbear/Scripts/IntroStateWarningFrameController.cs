using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class IntroStateWarningFrameController : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject warning;


    private bool isPlay = false;

    private void Start()
    {
        warning.transform.localScale = Vector3.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            warning.SetActive(true);
            StartCoroutine(WarningTextEffect());
        }
    }

    public void WarningCoroutine()
    {
        if (isPlay) 
        {
            return;
        }
        isPlay = true;
        warning.SetActive(true);
        warning.transform.localScale = new Vector3(1f, 1f, 1f);
        Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
        Transform warningTransfom = warning.gameObject.transform;
        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(warningTransfom.DOScale(targetScale, 1f).SetEase(Ease.OutElastic));
        seq.Join(warningTransfom.DOShakePosition(1f));
        seq.AppendInterval(1.5f);
        seq.AppendCallback(
            () =>
            {
                isPlay = false;
                warning.gameObject.SetActive(false);
            }
        );
        seq.Play();
    }

    private IEnumerator WarningTextEffect()
    {
        warning.transform.localScale = new Vector3(1f, 1f, 1f);
        Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);

        Transform warningTransfom = warning.gameObject.transform;

        warning.gameObject.SetActive(true);

        warningTransfom.DOScale(targetScale, 1f).SetEase(Ease.OutElastic);
        warningTransfom.DOShakePosition(1f);

        yield return new WaitForSeconds(1.5f);
        warning.gameObject.SetActive(false);
    }
}
