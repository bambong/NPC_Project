using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;


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
    [SerializeField]
    private float disableDuration = 1.5f;

    private ContractPanelController contractPanel;
    private TMP_InputField inputField;

    private Vector2 textInitPosition;

    private string previousText = "";
    private string playerName = null;

    public bool isSelect = false;
    private bool isRestrict = false;
    private bool isShake = false;
    private bool inputDisabled = false;

    private float disabledEndTime;
    

    public override void Init()
    {        
    }

    private void Start()
    {
        contractPanel = GetComponentInParent<ContractPanelController>();
        inputField = GetComponent<TMP_InputField>();

        inputField.onValueChanged.AddListener(UpdateOutputText);
        playerNameInput.characterLimit = 8;
        textInitPosition = test.gameObject.transform.position;

        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void Update()
    {
        if (inputDisabled && Time.time >= disabledEndTime)
        {
            inputDisabled = false;
        }
    }

    private void OnEndEdit(string text)
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StringCheck();
            SaveUserName();
        }
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
                    Managers.Sound.PlaySFX(Define.SOUND.DataPuzzleBad);
                    StartCoroutine(WarningTextEffect());
                }             
            }
        }

        if (!isRestrict && playerName != null) 
        {
            Managers.Sound.PlaySFX(Define.SOUND.Sign);
            SubmitName();
            if (contractPanel != null)
            {
                isRestrict = true;
                StartCoroutine(DisableInputForDurationCoroutine());
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
        StopCoroutine(WarningTextEffect());
    }

    public void TextShake()
    {
        if(!isShake)
        {
            isShake = true;
            test.gameObject.transform.DOScale(new Vector3(1.05f, 0.95f, 1f), 0.1f).SetLoops(2, LoopType.Yoyo).OnComplete(() => {
                isShake = false;
            });
        }       
    }

    public void UpdateOutputText(string inputValue)
    {
        previousText = inputValue;
        outputText.text = inputValue;
    }

    private IEnumerator DisableInputForDurationCoroutine()
    {
        inputDisabled = true;
        yield return new WaitForEndOfFrame();
        playerNameInput.interactable = false;
        inputDisabled = false;
    }

    public void LoadNextScene()
    {
        if(!isRestrict && playerName != null)
        {
            Managers.Scene.LoadScene(Define.Scene.Chapter_02_Forest.ToString());
        }
    }
}
