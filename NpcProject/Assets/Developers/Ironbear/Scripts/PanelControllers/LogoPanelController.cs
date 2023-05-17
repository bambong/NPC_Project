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
        btns.transform.localPosition = new Vector3(-380, 0, 0);
        bar.transform.localPosition = new Vector3(-1600, 160, 0);

        MoveObject();
    }

    private void MoveObject()
    {
        logos.transform.DOLocalMoveX(logos.transform.localPosition.x + 1100, moveDuration).SetEase(Ease.OutQuad);
        bar.transform.DOLocalMoveX(bar.transform.localPosition.x + 1150, moveDuration).SetEase(Ease.OutQuad);

        for (int i = 0; i < btnsList.Length; i++)
        {
            GameObject btn = btnsList[i];
            float delay = i * moveDelay;

            btn.transform.DOLocalMoveX(btn.transform.localPosition.x + 380, moveDuration).SetEase(Ease.OutQuad).SetDelay(delay);
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
}
