using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class TalkPanelController : UI_Base
{
    private const string pattern = "@(.*?)@";
    private readonly float TEXT_SPEED = 0.05f;
    
    [SerializeField]
    private Image speakImage;
    [SerializeField]
    private TextMeshProUGUI spekerName;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private Transform talkPanelInner;
    [SerializeField]
    private Button choiceA;
    [SerializeField]
    private Button choiceB;
    [SerializeField]
    private Button choiceC;
    [SerializeField]
    private TextMeshProUGUI choiceTextA;
    [SerializeField]
    private TextMeshProUGUI choiceTextB;
    [SerializeField]
    private TextMeshProUGUI choiceTextC;

    public Transform TalkPanelInner { get => talkPanelInner; }
    public bool IsNext { get => isNext;}
    public bool IsChoice { get => isChoice; }
    public bool IsSelect { get => isSelect; }

    private Dialogue curDialogue;

    private float typingTime;    
    private System.Random random;
    
    private string textDialogue = null;
    private string textStore = null;

    private bool isNext = false;
    private bool isTrans = false;
    private bool inputKey = false;
    private bool isChoice = false;
    private bool isSelect = false;

    private int textSize = 0;
    private int randomSize = 0;

    public override void Init()
    {
        choiceA.onClick.AddListener(Selected);
        choiceB.onClick.AddListener(Selected);
        //choiceC.onClick.AddListener(Selected);
    }

    public void SetDialogue(Dialogue dialogue)
    {
        speakImage.sprite = dialogue.speaker.sprite;
        spekerName.text = dialogue.speaker.name;
        choiceA.gameObject.SetActive(false);
        choiceB.gameObject.SetActive(false);
        dialogueText.text = "";
    }
    public void PlayDialogue(Dialogue dialogue) 
    {
        dialogueText.text = "";
        isNext = false;
        curDialogue = dialogue;
        speakImage.sprite = dialogue.speaker.sprite;
        spekerName.text = dialogue.speaker.name;
        StartCoroutine(TransText());
    }

    private void DotweenTextani(string text)
    {
        textDialogue = text;
        typingTime = textDialogue.Length * TEXT_SPEED;

        dialogueText.text = "";
        dialogueText.DOKill();
        dialogueText.DOText(textDialogue, typingTime).OnStart(()=>
        {
            StartCoroutine(SkipTextani());
        })     
        .OnComplete(()=>
        {
            if(isChoice == true)
            {
                Invoke("ChoicePanelActive", 0.2f);
                StartCoroutine(ChoiceSelect());                
            }
        });
    }

    private void ChoicePanelActive()
    {
        for(int i = 0; i < 2; i++)
        {
            choiceA.gameObject.SetActive(true);
            choiceB.gameObject.SetActive(true);
            //choiceC.gameObject.SetActive(true);
        }
    }

    private string RandomText(int length)
    {
        random = new System.Random();
        string charcters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return "<color=black>" + new string(Enumerable.Repeat(charcters, length).Select(s => s[random.Next(s.Length)]).ToArray()) + "</color>";
    }

    IEnumerator TransText()
    {
        inputKey = false;
        textStore = null;
        textDialogue = null;
        randomSize = 0;
        typingTime = curDialogue.text.Length * TEXT_SPEED;

        char[] sep = { '#', '#' };

        string[] result = curDialogue.text.Split(sep);

        foreach (var item in result)
        {
            if (item == "dummy")
            {
                isTrans = true;
            }
            else if (item == "choice")
            {
                isChoice = true;
            }
            else
            {
                textDialogue += item;
            }
        }

        if (isChoice)
        {
            textDialogue = TextExtraction(textDialogue);
        }

        textSize = ClcTextLength(textDialogue, textDialogue.Length);

        if (isTrans == true)
        {
            StartCoroutine(SkipTextani());

            for (int i = 0; i < textDialogue.Length; i++)
            {
                if (textDialogue[i] == '<')
                {
                    for (int j = i; !(textDialogue[j] == '>'); j++)
                    {
                        textStore += textDialogue[i];
                        i++;
                    }
                    i--;
                }
                else
                {
                    randomSize++;
                    textStore += textDialogue[i];
                    dialogueText.text = textStore + RandomText(textSize - randomSize);
                }
                yield return new WaitForSeconds(TEXT_SPEED);


                if (isTrans == false)
                {
                    dialogueText.text = textDialogue;
                    yield break;
                }
                inputKey = true;
            }
            isTrans = false;
            isNext = true;
        }
        else
        {
            DotweenTextani(textDialogue);
            inputKey = true;
        }
    }

    private string TextExtraction(string textDialogue)
    {

        MatchCollection matches = Regex.Matches(textDialogue, pattern);

        List<string> matchedStrings = new List<string>();
        StringBuilder replacedStrings = new StringBuilder();        

        int lastIndex = 0;
        foreach(Match match in matches)
        {
            string value = match.Groups[1].Value;
            int index = match.Index;
            int length = match.Length;

            matchedStrings.Add(value);

            string replacedString = textDialogue.Substring(lastIndex, index - lastIndex);
            replacedStrings.Append(replacedString);

            lastIndex = index + length;
        }

        string lastString = textDialogue.Substring(lastIndex);
        replacedStrings.Append(lastString);

        textDialogue = replacedStrings.ToString();

        if(!(matchedStrings.Count == 0))
        {
            choiceTextA.text = matchedStrings[0];
            choiceTextB.text = matchedStrings[1];
            //choiceTextC.text = matchedStrings[2];
        }        

        return textDialogue;
    }

    private int ClcTextLength(string text, int length)
    {
        int textLength = length;
        for (int i = 0; i < length; i++)
        {
            if (text[i] == '<')
            {
                for (int j = i; !(text[j] == '>'); j++)
                {
                    textLength--;
                    i++;
                }
                i--;
            }
        }
        return textLength;
    }

    IEnumerator SkipTextani()
    {
        while(!isNext)
        {

            if(Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.SKIP_KEY)))
            {
                if(isChoice == true)
                {
                    ChoicePanelActive();
                    StartCoroutine(ChoiceSelect());
                }
                if (isTrans == false && inputKey == true)
                {
                    dialogueText.DOKill();                    
                    dialogueText.text = textDialogue;
                    yield return new WaitForSeconds(0.1f);
                    isNext = true;
                    inputKey = false;
                    yield break;
                }
                if(isTrans == true && inputKey == true)
                {
                    dialogueText.text = "";
                    dialogueText.text = textDialogue;
                    yield return new WaitForSeconds(0.1f);
                    isNext = true;
                    isTrans = false;
                    inputKey = false;
                    yield break;
                }
            }
            yield return null;
        }
    }   

    IEnumerator ChoiceSelect()
    {
        while (isSelect == false)
        {
            yield return null;
        }
        isChoice = false;
        isNext = true;
    }

    public void Selected()
    {
        Debug.Log("select");
        isSelect = true;
        choiceA.gameObject.SetActive(false);
        choiceB.gameObject.SetActive(false);
    }
    public void InputIsSelect(bool value)
    {
         isSelect = value;
    }
}
