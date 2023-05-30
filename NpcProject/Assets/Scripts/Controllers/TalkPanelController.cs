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
    private const string PATTERN = "@(.*?)@";
    private readonly float TEXT_SPEED = 0.05f;
    private readonly float SKIP_DELAY_TIME = 0.1f;
    
    [SerializeField]
    private List<Image> leftRights;
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

    private bool isSkip = false;
    private bool isNext = false;
    private bool isTrans = false;
    private bool isChoice = false;
    private int buttonCount;

    private Renderer leftRenderer;
    private Renderer rightRenderer;
    private Material leftmaterial;
    private Material rightmaterial;

    private Color leftColor;
    private Color rightColor;
    private Color gray = new Color(56/255f, 56/255f, 56/255f);
 

    public override void Init()
    {
        choiceButton.AddButtonEvent();
    }

    #region TalkEvent
    public void SetDialogue(Dialogue dialogue)
    {
        for(int i =0; i < dialogue.leftRights.Length; ++i ) 
        {            
            if(dialogue.leftRights[i] == null) 
            {
                leftRights[i].DOFade(0, 0);
                continue;
            }
            leftRights[i].sprite = dialogue.leftRights[i].sprite;
            if (dialogue.speaker.sprite == dialogue.leftRights[i].sprite) 
            {
                leftRights[i].color = Color.white;
            }
            else 
            {
                leftRights[i].color = Color.gray;
            }
            leftRights[i].DOFade(1, 0);
        }
        spekerName.text = $"-{dialogue.speaker.name}-";
        choiceButton.Inactive();
        dialogueText.text = "";
    }

    public void PlayDialogue(Dialogue dialogue) 
    {        
        dialogueText.text = "";
        isNext = false;        
        curDialogue = dialogue;

        for (int i = 0; i < dialogue.leftRights.Length; ++i)
        {
            if (dialogue.leftRights[i] == null)
            {
                leftRights[i].DOFade(0, 0);
                continue;
            }
            leftRights[i].sprite = dialogue.leftRights[i].sprite;
            if (dialogue.speaker.sprite == dialogue.leftRights[i].sprite)
            {
                leftRights[i].color = Color.white;
            }
            else
            {
                leftRights[i].color = Color.gray;
            }
            leftRights[i].DOFade(1, 0);
        }

        spekerName.text = $"-{dialogue.speaker.name}-";
        StartCoroutine(SkipDelayTime());
        StartCoroutine(PlayTextAnimation());
    }
    #endregion
    #region CutSceneTalk
    public void SetDialogue(Dialogue dialogue, GameObject left, GameObject right)
    {
        //init
        leftRenderer = left.GetComponent<Renderer>();
        rightRenderer = right.GetComponent<Renderer>();
        leftmaterial = leftRenderer.material;
        rightmaterial = rightRenderer.material;
        leftColor = leftmaterial.color;
        rightColor = rightmaterial.color;        

        for (int i = 0; i < dialogue.leftRights.Length; ++i)
        {
            leftRights[i].DOFade(0, 0);
            continue;
        }

        if (dialogue.speaker.charName == left.name)
        {
            rightmaterial.color = gray;
            rightRenderer.material = rightmaterial;

            leftmaterial.color = leftColor;
            leftRenderer.material = leftmaterial;
        }
        if (dialogue.speaker.charName == right.name)
        {
            leftmaterial.color = gray;
            leftRenderer.material = leftmaterial;

            rightmaterial.color = rightColor;
            rightRenderer.material = rightmaterial;
        }

        spekerName.text = $"-{dialogue.speaker.name}-";
        choiceButton.Inactive();
        dialogueText.text = "";
    }

    public void PlayDialogue(Dialogue dialogue, GameObject left, GameObject right)
    {
        dialogueText.text = "";
        isNext = false;
        curDialogue = dialogue;
        
        for (int i = 0; i < dialogue.leftRights.Length; ++i)
        {
            leftRights[i].DOFade(0, 0);
            continue;
        }

        if (dialogue.speaker.charName == left.name)
        {
            rightmaterial.color = gray;
            rightRenderer.material = rightmaterial;

            leftmaterial.color = leftColor;
            leftRenderer.material = leftmaterial;
        }
        if (dialogue.speaker.charName == right.name)
        {
            leftmaterial.color = gray;
            leftRenderer.material = leftmaterial;

            rightmaterial.color = rightColor;
            rightRenderer.material = rightmaterial;
        }

        spekerName.text = $"-{dialogue.speaker.name}-";
        StartCoroutine(SkipDelayTime());
        StartCoroutine(PlayTextAnimation());
    }

    public void RestorationMat()
    {
        leftmaterial.color = leftColor;
        leftRenderer.material = leftmaterial;

        rightmaterial.color = rightColor;
        rightRenderer.material = rightmaterial;
    }
    #endregion

    #region TextAnimation
    IEnumerator PlayTextAnimation()
    {
        int randomSize = 0;
        string textStore = null;
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
                if (isTrans == false)
                {
                    yield break;
                }
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
            }

            if (isChoice == true)
            {
                choiceButton.Active(buttonCount);
                StartCoroutine(ChoiceSelect());
            }

            isTrans = false;
            isNext = true; 
        }
        else
        {
            DotweenTextAnimation(textDialogue);
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
            if (Input.GetKeyDown(Managers.Game.Key.ReturnKey(KEY_TYPE.SKIP_KEY)) && isSkip == true)
            {
                if (isChoice == true)
                {
                    choiceButton.Active(buttonCount);
                    StartCoroutine(ChoiceSelect());
                }
                if (isTrans == false)
                {
                    dialogueText.DOKill();
                    dialogueText.text = textDialogue;
                    yield return new WaitForSeconds(0.3f);
                    isSkip = false;
                    isNext = true;
                    break;                    
                }
                if (isTrans == true)
                {
                    dialogueText.text = "";
                    dialogueText.text = textDialogue;
                    isTrans = false;
                    yield return new WaitForSeconds(0.3f);
                    isSkip = false;
                    isNext = true;
                    break;
                }
            }
            yield return null;
        }
        isSkip = false;
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
                continue;
            }
            if (item == "choice")
            {
                isChoice = true;
                continue;
            }
            if(item == "player")
            {
                textDialogue += Managers.Talk.GetSpeakerName(101);
                continue;
            }
            textDialogue += item;
        }
    }

    IEnumerator SkipDelayTime()
    {
        yield return new WaitForSeconds(SKIP_DELAY_TIME);
        isSkip = true;
    }
    #endregion

    #region TextParsing
    private string TextExtraction(string textDialogue)
    {
        MatchCollection matches = Regex.Matches(textDialogue, PATTERN);

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

        buttonCount = matchedStrings.Count;
        SetChoiceText(matchedStrings);

        return textDialogue;
    }

    private void SetChoiceText(List<string> matchedStrings)
    {
        if (buttonCount == 2)
        {
            choiceButton.choiceTextA.text = matchedStrings[0];
            choiceButton.choiceTextB.text = matchedStrings[1];
        }
        if (buttonCount == 3)
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
        if(isTrans == true)
        {
            isChoice = false;
        }        
        isNext = true;
    }

    public void InputIsSelect(bool value)
    {
        choiceButton.SetisSelect(value);
        choiceButton.Inactive();
        isChoice = false;
    }

    public bool GetIsSelect()
    {
        return choiceButton.IsSelect;
    }
}
