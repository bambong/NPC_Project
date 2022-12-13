using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeywordPanelController : UI_Base
{
    [SerializeReference]
    private Transform layout;

    public Transform Layout { get => layout; }

    public override void Init()
    {
        
    }
    public void AddKeyword(KeywordController keywordController) 
    {
        keywordController.transform.parent = layout.transform;
    }

    public void Open() 
    {
        gameObject.SetActive(true);
    }
    public void Close() 
    {
        gameObject.SetActive(false);
    }
}
