using Michsky.UI.MTP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationPanelController : UI_Base
{
    [SerializeField]
    private StyleManager styleText;

    public override void Init()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Open(string str)
    {
        styleText.textItems.First().text = str;
        styleText.gameObject.SetActive(true);
        styleText.Play();
    }
}
