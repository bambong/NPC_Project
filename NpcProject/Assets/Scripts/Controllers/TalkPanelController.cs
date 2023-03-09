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

    public override void Init()
    {
    }

    public void SetDialogue(Dialogue dialogue)
    {
        speakImage.sprite = dialogue.speaker.sprite;
        spekerName.text = dialogue.speaker.name;
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
        return new string(Enumerable.Repeat(charcters, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    IEnumerator TransText()
    {
        inputKey = false;
        textStore = null;
        textDialogue = null;
        typingTime = curDialogue.text.Length * TEXT_SPEED;

        char[] sep = { '<', '>' };
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

        if (isTrans == true)
        {
            StartCoroutine(SkipTextani());
            
            for (int i = 0; i < textDialogue.Length; i++)
            {
                textStore += textDialogue[i];
                dialogueText.text = textStore + RandomText(textDialogue.Length - (i + 1));
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
