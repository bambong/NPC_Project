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
    private TextMeshProUGUI retryText;
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
    private readonly float OPEN_INTERVAL = 0.5f;
    private readonly float FADE_OPEN_TIME = 1f;
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
        Managers.Scene.ReLoadCurrentScene();
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
        canvasGroup.gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(OPEN_INTERVAL);
        seq.Append(canvasGroup.DOFade(1, FADE_OPEN_TIME));
        seq.AppendCallback(() =>
        {
            StartCoroutine(RetryInputCheck());
            retryText.gameObject.SetActive(true);
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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Managers.Sound.PlaySFX(Define.SOUND.ResetButton);
                Managers.Sound.StopBGM();
                Managers.Data.DataRemoveForResetButton(SceneManager.GetActiveScene().name);
                Managers.Scene.ReLoadCurrentScene();
                //  Close();
                yield break;
            }
            yield return null;
        }

    }
}
