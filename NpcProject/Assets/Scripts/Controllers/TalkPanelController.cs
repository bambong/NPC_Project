using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

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
    public void SetText(Speak speak) 
    {
        curSpeak = speak;
        curIndex = 0;
        speakImage.sprite = speak.speakerImage;
        spekerName.text = speak.speakerName;
    }
    public bool MoveNext() 
    {
        if(curSpeak.dialogues.Count <= curIndex)
        {
            return false;
        }
        dialogueText.text = "";        
        dialogueText.DOText(curSpeak.dialogues[curIndex].text, 1.0f).SetEase(Ease.OutCubic);
        curIndex++;
        return true;
    }
}
