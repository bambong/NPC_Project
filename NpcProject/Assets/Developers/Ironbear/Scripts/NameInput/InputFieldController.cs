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
    private TextMeshProUGUI test;
    [SerializeField]
    private TMP_InputField playerNameInput;
    [SerializeField]
    private Button submitBtn;
    [SerializeField]
    private TMP_Text outputText;

    [SerializeField]
    private Vector3 targetScale;

    private ContractPanelController contractPanel;
    private TMP_InputField inputField;

    private string playerName = null;
    private bool isRestrict = false;

    public override void Init()
    {        
    }

    private void Awake()
    {
        contractPanel = GetComponentInParent<ContractPanelController>();
        inputField = GetComponent<TMP_InputField>();

        inputField.onValueChanged.AddListener(UpdateOutputText);
        playerNameInput.characterLimit = 10;
    }

    public void SubmitName()
    {
        SaveUserName();
    }   

    public void StringCheck()
    {
        isRestrict = false;
        playerName = test.text;

        char[] restrictChars = { 'ㅏ', 'ㅑ', 'ㅓ', 'ㅕ', 'ㅗ', 'ㅛ', 'ㅜ', 'ㅠ', 'ㅡ', 'ㅣ', 'ㅐ', 'ㅒ', 'ㅔ', 'ㅖ', 'ㅚ', 'ㅟ', 'ㅙ', 'ㅞ', 'ㅝ', 'ㄱ', 'ㄴ', 'ㄷ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅅ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ', 'ㅆ', 'ㄸ', 'ㄲ', ' ' };
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
            SubmitName();
            if(contractPanel!=null)
            {
                contractPanel.UiOff();
            }           
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

    private void UpdateOutputText(string inputValue)
    {
        outputText.text = inputValue;
    }
}
