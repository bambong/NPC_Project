using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using System.Linq;

public class TalkPanelController : MonoBehaviour
{
    [SerializeField]
    private Image speakImage;

    [SerializeField]
    private TextMeshProUGUI spekerName;
    
    [SerializeField]
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
    public void SetText(Speak speak) 
    {
        curSpeak = speak;
        curIndex = 0;
        speakImage.sprite = speak.speaker.sprite;
        spekerName.text = speak.speaker.name;
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


    IEnumerator TransText()
    {
        textStore = null;
        textDialogue = null;
        char [] sep = { '<', '>'};
        string[] result = curSpeak.dialogues[curIndex].text.Split(sep);
        
        foreach (var item in result)
        {
            Debug.Log(item);
            if(item == "dummy") 
            {
                isTrans = true;
            }
            else
            {
                textDialogue += item;
            }
        }
        
        // textDialogue = curSpeak.dialogues[curIndex].text;
        if(isTrans == true)
        {
            for (int i = 0; i < textDialogue.Length; i++)
            {
                textStore += textDialogue[i];
                dialogueText.text = textStore + RandomText(textDialogue.Length - (i + 1));
                yield return new WaitForSeconds(textSpeed);
                StartCoroutine(SkipTextani());
                if(isTrans == false)
                {
                    yield break;
                }
            }
        }
        else
        {
            DotweenTextani();
        }


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

    IEnumerator SkipTextani()
    {
        while (isTrans == true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                dialogueText.text = "";
                dialogueText.text = curSpeak.dialogues[curIndex].text;
                isTrans = false;
                isNext = true;
                yield break;
            }
            yield return INPUT_CHECK_WAIT;
        }

        while(textTime < typingTime)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                dialogueText.DOKill();
                dialogueText.text = textDialogue;
                isNext = true;
                break;
            }
            textTime += 0.01f;
            yield return INPUT_CHECK_WAIT;
        }
    }
    
}
