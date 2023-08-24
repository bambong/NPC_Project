using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryPanelController : UI_Base
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private RectTransform resetButtonRect;

    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector3 desirePos;

    private bool isOpenPanel;
    private bool isOpenButton;
    private readonly float OPEN_INTERVAL = 3f;
    public override void Init()
    {
    }
    public void PasueButtonOpen() 
    {
        pauseButton.gameObject.SetActive(true);
    }
    public void PauseButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleDigital);
        Managers.Game.SetStatePause();
    }
    public void RetryButtonActive()
    {
        Managers.Sound.PlaySFX(Define.SOUND.ResetButton);
        Managers.Sound.StopBGM();
        resetButton.interactable = false;
        Managers.Data.DataRemoveForResetButton(SceneManager.GetActiveScene().name);
        Managers.Scene.ReLoadCurrentScene(()=>Managers.Game.Player.PurposePanel.Open());
    }
    public void OpenResetButton() 
    {
        if (isOpenPanel || isOpenButton)
        {
            return;
        }
        resetButtonRect.anchoredPosition = startPos;
        resetButton.gameObject.SetActive(true);
        resetButtonRect.DOAnchorPos(desirePos, 1f);
        isOpenButton = true;
    }
    public void CloseResetButton()
    {
        isOpenButton = false;
        resetButtonRect.DOKill();
        resetButton.gameObject.SetActive(false);
    }
    public void ClosePauseButton()
    {
        pauseButton.gameObject.SetActive(false);
    }
    public void OpenRetryPanel()
    {
        if (isOpenPanel)
        {
            return; 
        }
        isOpenPanel = true;
        if (isOpenButton)
        {
            CloseResetButton();
        }
        ClosePauseButton();
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(OPEN_INTERVAL);
        seq.AppendCallback(() =>
        {
            StartCoroutine(RetryInputCheck());
        });
        seq.Play();
    }
    public void Close()
    {
        isOpenPanel = false;
        gameObject.SetActive(false);
    }

    IEnumerator RetryInputCheck()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                Managers.Sound.PlaySFX(Define.SOUND.ResetButton);
                Managers.Sound.StopBGM();
                Managers.Data.DataRemoveForResetButton(SceneManager.GetActiveScene().name);
                Managers.Scene.ReLoadCurrentScene(() => Managers.Game.Player.PurposePanel.Open());
                //  Close();
                yield break;
            }
            yield return null;
        }

    }
}
