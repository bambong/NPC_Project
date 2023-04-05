using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InputFieldController : UI_Base
{
    public Speaker speaker;

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

    private void Awake()
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
        SceneManager.LoadScene("Chapter_01");
    }

    public void StringCheck()
    {
        playerName = test.text;

        char[] restrictChars = { 'た', 'ち', 'っ', 'づ', 'で', 'に', 'ぬ', 'ば', 'ぱ', 'び', 'だ', 'ぢ', 'つ', 'て', 'な', 'は', 'ど', 'の', 'ね', 'ぁ', 'い', 'ぇ', 'ぉ', 'け', 'げ', 'さ', 'し', 'じ', 'ず', 'せ', 'ぜ', 'そ', 'ぞ', 'ざ', 'え', 'あ' };
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

    private void OnInputFieldValueChanged(string newValue)
    {
        speaker.charName = newValue;
    }
}
