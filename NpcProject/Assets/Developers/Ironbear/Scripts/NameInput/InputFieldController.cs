using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class InputFieldController : UI_Base
{
    public Speaker player;

    [SerializeField]
    private TMP_Text warning;
    [SerializeField]
    private GameObject confirmUI;
    [SerializeField]
    private TextMeshProUGUI test;
    [SerializeField]
    private TMP_InputField playerNameInput;
    [SerializeField]
    private Button submitBtn;

    [SerializeField]
    private Vector3 targetScale;

    private string playerName = null;
    private bool isRestrict = false;

    public override void Init()
    {
        
    }

    private void Awake()
    {
        playerNameInput.characterLimit = 7;
    }

    public void ConfirmUIOpen()
    {
        confirmUI.transform.localScale = new(0f, 0f, 0f);
        confirmUI.SetActive(true);
        confirmUI.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InOutBack);
    }

    public void ConfirmUiClose()
    {
        confirmUI.transform.localScale = new(1f, 1f, 1f);
        confirmUI.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.InOutBack).OnComplete(TurnOffUi);           
    }

    private void TurnOffUi()
    {
        confirmUI.SetActive(false);
    }

    public void LoadNextScene()
    {
        SaveUserName();
        SceneManager.LoadScene("Chapter_01");
    }   

    public void StringCheck()
    {
        isRestrict = false;
        playerName = test.text;

        char[] restrictChars = { 'た', 'ち', 'っ', 'づ', 'で', 'に', 'ぬ', 'ば', 'ぱ', 'び', 'だ', 'ぢ', 'つ', 'て', 'な', 'は', 'ど', 'の', 'ね', 'ぁ', 'い', 'ぇ', 'ぉ', 'け', 'げ', 'さ', 'し', 'じ', 'ず', 'せ', 'ぜ', 'そ', 'ぞ', 'ざ', 'え', 'あ', ' ' };
        char[] nameChars = new char[playerName.Length];

        for (int i = 0; i < nameChars.Length; i++)
        {
            nameChars[i] = (char)playerName[i];
        }

        for(int i=0; i < nameChars.Length; i++)
        {
            for (int j = 0; j < restrictChars.Length; j++)
            {
                if (nameChars[i] == restrictChars[j] || playerName.Length <= 1)
                {
                    isRestrict = true;
                    StartCoroutine(WarningTextEffect());
                }             
            }
        }

        if (!isRestrict && playerName != null) 
        {
            ConfirmUIOpen();
        }        
    }

    public void SaveUserName()
    {
        player.charName = playerName;
    }

    private IEnumerator WarningTextEffect()
    {
        warning.transform.localScale = new Vector3(1f, 1f, 1f);
        submitBtn.interactable = false;

        Transform warningTransfom = warning.gameObject.transform;

        warning.gameObject.SetActive(true);

        warningTransfom.DOScale(targetScale, 1f).SetEase(Ease.OutElastic);
        warningTransfom.DOShakePosition(1f);

        yield return new WaitForSeconds(1.5f);
        warning.gameObject.SetActive(false);

        submitBtn.interactable = true;
    }

    public void TextShake()
    {
        test.gameObject.transform.DOShakePosition(0.3f, 4);
    }
}
