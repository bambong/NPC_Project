using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeywordStatusUiController : UI_Base
{
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private Image image;


    public override void Init()
    {
        
    }
    public void UpdateUI(bool hasKeyword) 
    {
        if (hasKeyword) 
        {
            this.image.color = Color.green;
        }
        else 
        {
            this.image.color = Color.white;
        }
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        parent.gameObject.SetActive(false);
    }
}
