using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordEntity : MonoBehaviour
{
    [SerializeField]
    private string entityId;
    [SerializeField]
    private int keywordSlot = 1;

    private Action updateAction = null;



    public void ShowKeywordSlot() 
    {
        
    }

    public void AddAction(Action action) 
    {
        updateAction += action;
    }

    public void ClearAction() 
    {
        updateAction = null;
    }
    public void Update() 
    {
        updateAction?.Invoke();
    }

    public void Init() 
    {
        
    }

}
