using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Linq;

public class TalkPanelController : UI_Base
{ 
    private readonly float TEXT_SPEED = 0.05f;
    
    [SerializeField]
    private Image speakImage;
    [SerializeField]
    private TextMeshProUGUI spekerName;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private Transform talkPanelInner;

    public Transform TalkPanelInner { get => talkPanelInner; }
    public bool IsNext { get => isNext;}

    private Dialogue curDialogue;

    private float typingTime;    
    private System.Random random;
    
    private string textDialogue = null;
    private string textStore = null;

    private bool isNext = false;
    private bool isTrans = false;
    private bool inputKey = false;

    private int textSize = 0;
    private int randomSize = 0;

    public override void Init()
    {
    }

    public void SetDialogue(Dialogue dialogue)
    {
        speakImage.sprite = dialogue.speaker.sprite;
        spekerName.text = dialogue.speaker.charName;
        dialogueText.text = "";
    }
    public void PlayDialogue(Dialogue dialogue) 
    {
        dialogueText.text = "";
        isNext = false;
        curDialogue = dialogue;
        speakImage.sprite = dialogue.speaker.sprite;
        spekerName.text = dialogue.speaker.charName;
        StartCoroutine(TransText());

    }

    private void DotweenTextani()
    {
        textDialogue = curDialogue.text;
        typingTime = textDialogue.Length * TEXT_SPEED;

        dialogueText.text = "";
        dialogueText.DOKill();
        dialogueText.DOText(textDialogue, typingTime).OnStart(()=>
        {
            StartCoroutine(SkipTextani());
        })     
        .OnComplete(()=>
        {
            isNext = true;
        });
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
            else
            {
                textDialogue += item;
            }
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
            DotweenTextani();
            inputKey = true;
        }
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
                if(isTrans == false && inputKey == true)
                {
                    Debug.Log("skipdialog");
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
}
