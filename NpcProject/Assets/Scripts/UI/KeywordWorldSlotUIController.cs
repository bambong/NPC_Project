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
    private TextMeshProUGUI keywordText;
    [SerializeField]
    private Image image;

    private readonly string DEFAULT_TEXT = "NULL";
    
    public override void Init()
    {
        
    }
    public void SetSlotUI(Color color ,string text) 
    {
        this.keywordText.text = text;
        image.color = color;
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
        image.color = Color.black;
        keywordText.text = DEFAULT_TEXT;
    }
}
