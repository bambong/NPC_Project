using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntutorialController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private float openAnimTime = 0.5f;

    private bool isOpen;
    public void Open() 
    {
        if(isOpen)
        {
            return;
        }
        isOpen = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1, openAnimTime);
        //StartCoroutine(CheckRun());
    }
    IEnumerator CheckRun() 
    {
        while (Managers.Game.Player.PlayerStateController.CurState != PlayerRun.Instance)
        {
            yield return null;
        }
        Close();
    }

    public void Close()
    {
        if (!isOpen)
        {
            return;
        }
        isOpen = false;
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, openAnimTime);

    }
}
