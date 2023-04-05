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

    private string playerName = null;
    private bool isRestrict = false;

    public override void Init()
    {
             
    }

    public void ConfirmUIOpen()
    {
        confirmUI.SetActive(true);
    }

    public void ConfirmUiClose()
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

        char[] restrictChars = { '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��' };
        char[] nameChars = new char[playerName.Length];

        for (int i = 0; i < nameChars.Length; i++)
        {
            nameChars[i] = (char)playerName[i];
        }

        for(int i=0; i < nameChars.Length; i++)
        {
            for (int j = 0; j < restrictChars.Length; j++)
            {
                if (nameChars[i] == restrictChars[j])
                {
                    isRestrict = true;
                    StartCoroutine(WarningTextEffect());
                }             
            }
        }

        if(!isRestrict)
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
        //Transform warningTransfom = warning.transform;

        warning.gameObject.SetActive(true);
        //warningTransfom.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.InOutCubic).onComplete();
        yield return new WaitForSeconds(1.5f);
        warning.gameObject.SetActive(false);
    }
}
