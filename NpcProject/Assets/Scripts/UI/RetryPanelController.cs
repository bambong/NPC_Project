using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryPanelController : UI_Base
{

    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private RectTransform resetButtonRect;
    [SerializeField]
    private RectTransform debugEnableNoti;

    [SerializeField]
    private Vector3 startResetButtonPos;
    [SerializeField]
    private Vector3 desireResetButtonPos;


    [SerializeField]
    private Vector3 startNotiButtonPos;
    [SerializeField]
    private Vector3 desireNotiButtonPos;


    [SerializeField]
    private TextMeshProUGUI debugKeyText;
    private bool isOpenPanel;
    private bool isOpenDebugUi;
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
        resetButtonRect.anchoredPosition = startResetButtonPos;
        resetButton.gameObject.SetActive(true);
        resetButtonRect.DOAnchorPos(desireResetButtonPos, 1f);
    }
    public void DebugKeyTextUpdate() => debugKeyText.text = KeySetting.Instance.currentKeys[KEY_TYPE.DEBUGMOD_KEY].ToString();
    public void OpenDebugEnableNoti()
    {
        if (!isOpenDebugUi) return;

        DebugKeyTextUpdate();
        debugEnableNoti.anchoredPosition = startNotiButtonPos;
        debugEnableNoti.gameObject.SetActive(true);
        debugEnableNoti.DOAnchorPos(desireNotiButtonPos, 1f);
        
    }
    public void CloseDebugEnableNoti() 
    {
        debugEnableNoti.DOKill();
        debugEnableNoti.gameObject.SetActive(false);
    }
    public void CloseResetButton()
    {
        resetButtonRect.DOKill();
        resetButton.gameObject.SetActive(false);
    }

    public void OpenDebugUi()
    {
        if (isOpenPanel || isOpenDebugUi)
        {
            return;
        }
        isOpenDebugUi = true;
        OpenDebugEnableNoti();
        OpenResetButton();
    }
    public void CloseDebugUi() 
    {
        isOpenDebugUi = false;
        CloseDebugEnableNoti();
        CloseResetButton();
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
        if (isOpenDebugUi)
        {
            CloseDebugUi();
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
