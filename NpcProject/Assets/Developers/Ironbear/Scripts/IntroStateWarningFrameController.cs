using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class IntroStateWarningFrameController : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject warning;


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
