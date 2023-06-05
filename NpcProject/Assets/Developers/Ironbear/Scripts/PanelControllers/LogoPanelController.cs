using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LogoPanelController : UI_Base
{
    [SerializeField]
    private GameObject logos;
    [SerializeField]
    private GameObject btns;
    [SerializeField]
    private GameObject bar;
    [SerializeField]
    private GameObject arm;

    [SerializeField]
    private GameObject[] btnsList;

    private float moveDuration = 0.8f;
    private float moveDelay = 0.1f;


    public override void Init()
    {
        for (int i = 0; i < btnsList.Length; i++)
        {
            btnsList[i].GetComponent<Button>().interactable = false;
        }
    }

    private void Start()
    {
        logos.transform.localPosition = new Vector3(-1100, 0, 0);
     //   btns.transform.localPosition = new Vector3(-1150, -45, 0);
        bar.transform.localPosition = new Vector3(-1600, 160, 0);
        arm.transform.localRotation = Quaternion.Euler(0f, 0f, 1f);
        for (int i = 0; i < btnsList.Length; i++)
        {
            GameObject btn = btnsList[i];
            var pos = btn.transform.localPosition;
            pos.x -= 400;
            btn.transform.localPosition = pos; 
        }
        MoveObject();

        Sequence armSq = DOTween.Sequence();
        armSq.Append(arm.transform.DORotate(new Vector3(0f, 0f, 5f), 2f).SetEase(Ease.InOutSine));
        armSq.Append(arm.transform.DORotate(new Vector3(0f, 0f, 1f), 2f).SetEase(Ease.InOutSine));
        armSq.SetLoops(-1);
    }

    private void MoveObject()
    {
        logos.transform.DOLocalMoveX(logos.transform.localPosition.x + 1100, moveDuration).SetEase(Ease.OutQuad);
        bar.transform.DOLocalMoveX(bar.transform.localPosition.x + 1150, moveDuration).SetEase(Ease.OutQuad);

        for (int i = 0; i < btnsList.Length; i++)
        {
            GameObject btn = btnsList[i];
            float delay = i * moveDelay * 0.5f;

            btn.transform.DOLocalMoveX(btn.transform.localPosition.x + 400, moveDuration).SetEase(Ease.OutQuad).SetDelay(delay);
        }

        EnableButtonInteractable();
    }

    private void EnableButtonInteractable()
    {
        for (int i = 0; i < btnsList.Length; i++)
        {
            btnsList[i].GetComponent<Button>().interactable = true;
        }
    }

    public void DisableButtonInteractable()
    {
        for (int i = 0; i < btnsList.Length; i++)
        {
            CanvasGroup btnCanvas = btnsList[i].GetComponent<CanvasGroup>();

            btnCanvas.blocksRaycasts = false;
            btnCanvas.interactable = false;
        }
    }
}
