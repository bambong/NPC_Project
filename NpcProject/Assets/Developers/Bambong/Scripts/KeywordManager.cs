using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordManager
{

    private KeywordEntity curKeywordEntity;
    private PlayerKeywordPanelController playerKeywordPanel;
    public Transform playerKeywordPanelLayout { get => playerKeywordPanel.Layout; }
    public PlayerKeywordPanelController PlayerKeywordPanel { get => playerKeywordPanel;}

    private List<KeywordController> curPlayerKeywords = new List<KeywordController>();
    private GraphicRaycaster graphicRaycaster;
    public void Init()
    {
        playerKeywordPanel = Managers.UI.MakeSceneUI<PlayerKeywordPanelController>(null,"PlayerKeywordPanel");
        graphicRaycaster = playerKeywordPanel.gameObject.GetOrAddComponent<GraphicRaycaster>();
    }

    public List<RaycastResult> GetRaycastList(PointerEventData pointerEventData)
    {
        var raycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData,raycastResults);
        return raycastResults;
    }
    public void EnterKeywordMod(KeywordEntity keywordEntity) 
    {
        Managers.Game.SetStateKeywordMod();
        curKeywordEntity = keywordEntity;
        playerKeywordPanel.Open();
        keywordEntity.OpenKeywordSlot();
    }
    public void ExitKeywordMod()
    {
        Managers.Game.SetStateDebugMod();
        playerKeywordPanel.Close();
        curKeywordEntity.CloseKeywordSlot();
    }
    public bool AddKeywordToPlayer(KeywordController keywordController) 
    {
        if(curPlayerKeywords.Contains(keywordController)) 
        {
            return false;
        }
        curPlayerKeywords.Add(keywordController);
        playerKeywordPanel.AddKeyword(keywordController);
        return true;
    }
    public bool RemoveKeywordToPlayer(KeywordController keywordController) 
    {
        if(!curPlayerKeywords.Contains(keywordController))
        {
            return false;
        }
        curPlayerKeywords.Remove(keywordController);
        return true;
    }
    public void SetKeyWord(KeywordController keywordController)
    {
        keywordController.ResetKeyword();
    }
    public void Interaction() 
    {
    
    }
}
