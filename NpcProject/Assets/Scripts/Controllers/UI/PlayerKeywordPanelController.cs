using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKeywordPanelController : UI_Base
{
    [SerializeField]
    private Transform layoutParent;
    public Transform LayoutParent { get => layoutParent; }

    public override void Init()
    {
        
    }
   
    public void Open() 
    {
        gameObject.SetActive(true);
        Managers.Keyword.CurDebugZone.OpenPlayerLayout();

    }
    public void Close() 
    {
        gameObject.SetActive(false);
        Managers.Keyword.CurDebugZone.ClosePlayerLayout();
    }
}
