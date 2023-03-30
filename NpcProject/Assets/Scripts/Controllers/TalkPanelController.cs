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
    private ChoiceButtonController choiceButton;

    public Transform TalkPanelInner { get => talkPanelInner; }
    public bool IsNext { get => isNext;}
    public bool IsChoice { get => isChoice; }

    private Dialogue curDialogue;

    private float typingTime;    
    private System.Random random;
    
    private string textDialogue = null;

    private bool isNext = false;
    private bool isTrans = false;
    private bool inputKey = false;
    private bool isChoice = false;
    private int buttonCount;

    public override void Init()
    {
        choiceButton.AddButtonEvent();
    }

    public void SetDialogue(Dialogue dialogue)
    {
        speakImage.sprite = dialogue.speaker.sprite;
        spekerName.text = dialogue.speaker.name;
        choiceButton.Inactive();
        dialogueText.text = "";
    }
    public void PlayDialogue(Dialogue dialogue) 
    {
        dialogueText.text = "";
        isNext = false;
        curDialogue = dialogue;
        speakImage.sprite = dialogue.speaker.sprite;
        spekerName.text = dialogue.speaker.name;
        StartCoroutine(PlayTextAnimation());
    }

    #region TextAnimation
    IEnumerator PlayTextAnimation()
    {
        int randomSize = 0;
        string textStore = null;
        inputKey = false;
        textDialogue = null;

        SettingTextAnimation();

        if (isChoice)
        {
            textDialogue = TextExtraction(textDialogue);
        }

        int textSize = ClcTextLength(textDialogue, textDialogue.Length);
        typingTime = textSize * TEXT_SPEED;

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
            DotweenTextAnimation(textDialogue);
            inputKey = true;
        }
    }

    private void DotweenTextAnimation(string text)
    {
        textDialogue = text;
        typingTime = textDialogue.Length * TEXT_SPEED;

        dialogueText.text = "";
        dialogueText.DOKill();
        dialogueText.DOText(textDialogue, typingTime).OnStart(() =>
        {
            StartCoroutine(SkipTextani());
        })
        .OnComplete(() =>
        {
            if (isChoice == true)
            {
                choiceButton.Active(buttonCount);
                StartCoroutine(ChoiceSelect());
            }
        });
    }

    IEnumerator SkipTextani()
    {
        while (!isNext)
        {

            if (Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.SKIP_KEY)))
            {
                if (isChoice == true)
                {
                    choiceButton.Active(buttonCount);
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
                if (isTrans == true && inputKey == true)
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

    private void SettingTextAnimation()
    {
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
    }
    #endregion

    #region TextParsing
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
            buttonCount = matchedStrings.Count;
            SetChoiceText(matchedStrings);
        }

        return textDialogue;
    }

    private void SetChoiceText(List<string> matchedStrings)
    {
        if (matchedStrings.Count == 2)
        {
            choiceButton.choiceTextA.text = matchedStrings[0];
            choiceButton.choiceTextB.text = matchedStrings[1];
        }
        if (matchedStrings.Count == 3)
        {
            choiceButton.choiceTextA.text = matchedStrings[0];
            choiceButton.choiceTextB.text = matchedStrings[1];
            choiceButton.choiceTextC.text = matchedStrings[2];
        }
    }
    #endregion

    #region TextFunction
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
    private string RandomText(int length)
    {
        random = new System.Random();
        string charcters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return "<color=black>" + new string(Enumerable.Repeat(charcters, length).Select(s => s[random.Next(s.Length)]).ToArray()) + "</color>";
    }
    #endregion

    IEnumerator ChoiceSelect()
    {
        while (choiceButton.IsSelect == false)
        {
            yield return null;
        }
        isChoice = false;
        isNext = true;
    }

    public void InputIsSelect(bool value)
    {
        choiceButton.SetisSelect(value);
    }

    public bool GetIsSelect()
    {
        return choiceButton.IsSelect;
    }
}
