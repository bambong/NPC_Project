using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    private bool isOpenPanel;
    private bool isOpenButton;
    private readonly float OPEN_INTERVAL = 0.5f;
    private readonly float FADE_OPEN_TIME = 1f;
    public override void Init()
    {
    }
    public void ButtonActive()
    {
        Managers.Sound.BGMControl(Define.BGM.Stop);
        resetButton.interactable = false;
        Managers.Scene.ReLoadCurrentScene();
    }
    public void OpenResetButton() 
    {
        if (isOpenPanel || isOpenButton)
        {
            return;
        }
        isOpenButton = true;
        resetButton.gameObject.SetActive(true);
    }
    public void CloseResetButton()
    {
        isOpenButton = false;
        resetButton.gameObject.SetActive(false);
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
            if (Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.RETRY_KEY)))
            {
                Managers.Sound.BGMControl(Define.BGM.Stop);
                Managers.Scene.ReLoadCurrentScene();
                //  Close();
                yield break;
            }
            yield return null;
        }

    }
}
