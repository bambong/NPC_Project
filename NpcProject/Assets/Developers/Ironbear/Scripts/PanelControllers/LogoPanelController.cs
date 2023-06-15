using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEditor;

public class LogoPanelController : UI_Base
{
    [SerializeField]
    private GameObject logos;
    [SerializeField]
    private GameObject btns;
    [SerializeField]
    private GameObject bar;
    [SerializeField]
    private GameObject title;

    [SerializeField]
    private GameObject[] btnsList;

    [SerializeField]
    private Material effectMat;
    [SerializeField]
    private GameObject logo;

    private Material originalMat;
    private Image rend;

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
        rend = logo.GetComponent<Image>();
        originalMat = rend.material;

        logos.transform.localPosition = new Vector3(-1100, 0, 0);
        bar.transform.localPosition = new Vector3(-1600, 160, 0);

        for (int i = 0; i < btnsList.Length; i++)
        {
            GameObject btn = btnsList[i];
            var pos = btn.transform.localPosition;
            pos.x -= 400;
            btn.transform.localPosition = pos; 
        }
        MoveObject();
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

        //material 변경
        rend.material = effectMat;
        Invoke("RestoreMaterial", moveDuration + 0.8f);
    }

    private void RestoreMaterial()
    {
        rend.material = originalMat;
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
