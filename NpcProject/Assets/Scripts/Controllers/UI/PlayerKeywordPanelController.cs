using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKeywordPanelController : UI_Base
{
    [SerializeField]
    private Transform layoutParent;
    private Dictionary<DebugZone,HorizontalLayoutGroup> debugZoneLayout = new Dictionary<DebugZone,HorizontalLayoutGroup>();
    public HorizontalLayoutGroup Layout { get => debugZoneLayout[Managers.Keyword.CurDebugZone]; }
    
    public override void Init()
    {
        
    }
   
    public void RegisterDebugZone(DebugZone debugZone)
    {
        if(!debugZoneLayout.ContainsKey(debugZone)) 
        {
            var layout = Managers.Resource.Instantiate("Layout",layoutParent);
            debugZoneLayout.Add(debugZone,layout.GetComponent<HorizontalLayoutGroup>());
        }
    }

    public HorizontalLayoutGroup GetDebugLayout(DebugZone zone) => debugZoneLayout[zone];

    public void AddKeyword(DebugZone zone, KeywordController keywordController) 
    {
        keywordController.transform.parent = debugZoneLayout[zone].transform;
    }

    public void Open() 
    {
        gameObject.SetActive(true);
        Layout.gameObject.SetActive(true);
    }
    public void Close() 
    {
        gameObject.SetActive(false);
        Layout.gameObject.SetActive(false);
    }
}
