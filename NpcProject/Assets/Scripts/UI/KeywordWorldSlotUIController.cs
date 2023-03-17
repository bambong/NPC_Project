using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeywordWorldSlotUIController : UI_Base
{
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private GameObject defaultTextGo;
    public override void Init()
    {
        
    }
    public void SetSlotUI(Image image) 
    {
        this.image.sprite = image.sprite;
        defaultTextGo.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close() 
    {
        parent.gameObject.SetActive(false);
    }


    public void ResetSlotUI() 
    {
        image.sprite = defaultSprite;
        defaultTextGo.SetActive(true);
    }
}
