using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CapPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject mouse;

    private bool isAnim = false;
    private float effectDuration = 1.5f;
    private CanvasGroup capPanel;
    private CanvasGroup mousePanel;
    private float intervalTime = 2f;

    void Start()
    {
        capPanel = GetComponent<CanvasGroup>();
        mousePanel = mouse.GetComponent<CanvasGroup>();

        capPanel.alpha = 0f;

        capPanel.DOFade(1f, 1f);
    }

    private void Awake()
    {
        Anim();
    }

    private IEnumerator AnimCoroutine()
    {
        Anim();
        yield return new WaitForSeconds(intervalTime);
        StartCoroutine(AnimCoroutine());
    }

    private void Anim()
    {
        if (isAnim)
        {
            return;
        }

        isAnim = true;

        Sequence seq = DOTween.Sequence();


        seq.Append(mouse.transform.DOLocalMoveX(mouse.transform.localPosition.x + 570f, effectDuration).SetEase(Ease.OutQuad));
        seq.OnComplete(() =>
        {
            mousePanel.DOFade(0f, 1f).SetEase(Ease.OutQuad);
            isAnim = false;
        });

        seq.Play();
    }
}
