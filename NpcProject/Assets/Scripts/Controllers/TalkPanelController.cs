using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Linq;

public class TalkPanelController : UI_Popup
{
    enum Images
    {
        SpeakerImage
    }

    enum TextMeshs
    {
        SpeakerName,
        DialogueText
    }

    private Image speakImage;
    private TextMeshProUGUI spekerName;
    private TextMeshProUGUI dialogueText;

    private Speak curSpeak;
    private int curIndex =0;
    private float textSpeed = 0.05f;
    private float typingTime;    
    private float textTime;
    private string textDialogue = null;
    private string textStore = null;
    private System.Random random;
    private bool isNext = false;
    private bool isTrans = false;

    private readonly WaitForSeconds INPUT_CHECK_WAIT = new WaitForSeconds(0.01f);

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(TextMeshs));
        Bind<Image>(typeof(Images));
        spekerName = Get<TextMeshProUGUI>((int)TextMeshs.SpeakerName);
        dialogueText = Get<TextMeshProUGUI>((int)TextMeshs.DialogueText);
        speakImage = Get<Image>((int)Images.SpeakerImage);
    }

    public void SetText(Speak speak) 
    {
        curSpeak = speak;
        curIndex = 0;
        speakImage.sprite = speak.speaker.sprite;
        spekerName.text = speak.speaker.name;
        dialogueText.text = "";
    }
    public bool MoveNext()
    {
        textTime = 0.0f;
        isNext = false;

        if (curSpeak.dialogues.Count <= curIndex)
        {
            return false;
        }

        StartCoroutine(TransText());
        // DotweenTextani();

        curIndex++;
        return true;
    }


    private void DotweenTextani()
    {
        textDialogue = curSpeak.dialogues[curIndex].text;
        typingTime = textDialogue.Length * textSpeed;

        dialogueText.text = "";
        dialogueText.DOKill();
        dialogueText.DOText(textDialogue, typingTime).OnStart(()=>
        {
            StartCoroutine(SkipTextani());
        })
        // .OnUpdate(()=>
        // {
        //     // dialogueText.text = RandomText(textDialogue.Length);

        // })
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

    public bool IsAni()
    {
        return isNext;
    }

    IEnumerator TransText()
    {
        textStore = null;
        textDialogue = null;
        typingTime = curSpeak.dialogues[curIndex].text.Length * textSpeed;

        char[] sep = { '<', '>' };
        string[] result = curSpeak.dialogues[curIndex].text.Split(sep);

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

        // textDialogue = curSpeak.dialogues[curIndex].text;
        if (isTrans == true)
        {
            for (int i = 0; i < textDialogue.Length; i++)
            {
                textStore += textDialogue[i];
                dialogueText.text = textStore + RandomText(textDialogue.Length - (i + 1));
                yield return new WaitForSeconds(textSpeed);
                if(i == 0)
                {
                    StartCoroutine(SkipTextani());
                }                
                if (isTrans == false)
                {
                    yield break;
                }
            }
            isTrans = false;
            isNext = true;
        }
        else
        {
            DotweenTextani();
        }
    }

    IEnumerator SkipTextani()
    {
        while(!isNext)
        {

            if(Input.GetKeyDown(Managers.Game.Key.skipKey))
            {
                if(isTrans == false)
                {
                    Debug.Log("skipdialog");
                    dialogueText.DOKill();
                    dialogueText.text = textDialogue;
                    yield return new WaitForSeconds(0.1f);
                    isNext = true;
                    yield break;
                }
                if(isTrans == true)
                {
                    dialogueText.text = "";
                    dialogueText.text = textDialogue;
                    isNext = true;
                    isTrans = false;
                    yield break;
                }
            }
            yield return null;
        }
    }

   
}
