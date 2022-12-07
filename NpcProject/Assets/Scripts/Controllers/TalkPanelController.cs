using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

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
    private string textDialogue;
    private bool isNext = false;

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
    }
    public bool MoveNext()
    {
        textTime = 0.0f;
        isNext = false;

        if (curSpeak.dialogues.Count <= curIndex)
        {
            return false;
        }

        DotweenTextani();

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
        }).OnComplete(()=>
        {
            isNext = true;
        });
    }

    public bool IsAni()
    {
        return isNext;
    }

    IEnumerator SkipTextani()
    {
        while(!isNext)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                dialogueText.DOKill();
                dialogueText.text = textDialogue;
                isNext = true;
                break;
            }
            yield return null;
        }
    }

   
}
