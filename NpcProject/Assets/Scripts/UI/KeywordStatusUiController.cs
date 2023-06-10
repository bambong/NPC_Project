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
    
    
    [SerializeField]
    private Sprite hasImage;
    [SerializeField]
    private Sprite emptyImage;


    public override void Init()
    {
        
    }
    public void UpdateUI(bool hasKeyword) 
    {
        if (hasKeyword) 
        {
            this.image.sprite = hasImage;
        }
        else 
        {
            this.image.sprite = emptyImage;
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
