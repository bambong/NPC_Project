using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TalkPanelController : MonoBehaviour
{
    [SerializeField]
    private Image speakImage;

    [SerializeField]
    private TextMeshProUGUI spekerName;
    
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private Speak curSpeak;
    public void SetText(Speak speak) 
    {
        curSpeak = speak;
        speakImage.sprite = speak.speakerImage;
        spekerName.text = speak.speakerName;
    }
    public bool MoveNext() 
    {
        bool result = false;
        Dialogue dialogue;
        result = curSpeak.MoveNext(out dialogue);
        dialogueText.text = dialogue.text;
        return result;
    }
}
