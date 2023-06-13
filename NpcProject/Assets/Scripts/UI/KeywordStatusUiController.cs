using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Analytics;

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
    
}
