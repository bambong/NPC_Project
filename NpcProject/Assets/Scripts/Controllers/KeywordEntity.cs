using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordEntity : MonoBehaviour
{
    [SerializeField]
    private string entityId;
    [SerializeField]
    private int keywordSlot = 1;

    private Action updateAction = null;
    private KeywordFameController keywordSlotUI;


    private void Start()
    {
        keywordSlotUI = Managers.UI.MakeSubItem<KeywordFameController>(Managers.Keyword.PlayerKeywordPanel.transform,"KeywordSlotUI");
        keywordSlotUI.transform.localScale = Vector3.one;
    }
    public void SetKeyword(KeywordController keywordController)
    {
        keywordSlotUI.SetKeyWord(keywordController);
    }

    public void OpenKeywordSlot() 
    {
        keywordSlotUI.Open();
    }
    public void CloseKeywordSlot()
    { 
        keywordSlotUI.Close();
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
