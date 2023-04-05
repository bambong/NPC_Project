using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
                    warning.gameObject.SetActive(true);
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
}
