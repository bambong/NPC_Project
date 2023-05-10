using UnityEngine;
using DG.Tweening;

public class LogoPanelController : UI_Base
{
    [SerializeField]
    private GameObject logos;
    [SerializeField]
    private GameObject btns;
    [SerializeField]
    private GameObject bar;

    private float moveDuration = 0.8f;


    public override void Init()
    {
    }

    private void Start()
    {
        logos.transform.localPosition = new Vector3(-1100, 0, 0);
        btns.transform.localPosition = new Vector3(650, 0, 0);
        bar.transform.localPosition = new Vector3(-1600, 160, 0);

        MoveObject();
    }

    private void MoveObject()
    {
        logos.transform.DOLocalMoveX(logos.transform.localPosition.x + 1100, moveDuration).SetEase(Ease.OutQuad);
        btns.transform.DOLocalMoveX(btns.transform.localPosition.x - 600, moveDuration).SetEase(Ease.OutQuad);
        bar.transform.DOLocalMoveX(bar.transform.localPosition.x + 1150, moveDuration).SetEase(Ease.OutQuad);
    }
}
