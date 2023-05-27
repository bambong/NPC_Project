using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatePanelController : MonoBehaviour
{
    public Speaker player;

    [SerializeField]
    private TMP_Text text1;

    void Start()
    {
        UpdateName();
    }

    private void UpdateName()
    {
        string dialogue = "현재 <Player>의 근무 상태를 변경합니다.";
        dialogue = dialogue.Replace("<Player>", player.charName);
        text1.text = dialogue;
    }
}
