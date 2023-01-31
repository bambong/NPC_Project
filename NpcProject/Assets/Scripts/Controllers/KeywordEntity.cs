using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum KeywordActionType 
{
    OnUpdate,
    OneShot
}

public class KeywordAction 
{
    private Action<KeywordEntity> action;
    private KeywordActionType actiontype;

    public Action<KeywordEntity> Action { get => action; }
    public KeywordActionType ActionType { get => actiontype;  }

    public KeywordAction(Action<KeywordEntity> action,KeywordActionType actiontype) 
    {
        this.action = action;
        this.actiontype = actiontype;
    }

}



public class KeywordEntity : MonoBehaviour
{
    [SerializeField]
    private int keywordSlot = 1;

    [SerializeField]
    private OutlineEffect outlineEffect;

    private static Dictionary<string,KeywordAction> keywrodOverrideTable = new Dictionary<string,KeywordAction>();
    private List<KeywordFrameController> keywordSlotUI = new List<KeywordFrameController>();
    private List<KeywordWorldSlotUIController> keywordSlotWorldUI = new List<KeywordWorldSlotUIController>();

    private Action<KeywordEntity> updateAction = null;
    private Action<KeywordEntity> oneShotAction = null;
    private Collider col;
    private Transform keywordSlotLayout;
    private KeywordWorldSlotLayoutController keywordWorldSlotLayout;
    private void Start()
    {
        Managers.Keyword.AddSceneEntity(this);
        keywordSlotLayout = Managers.Resource.Instantiate("UI/KeywordSlotLayout",Managers.Keyword.PlayerKeywordPanel.transform).transform;
        keywordWorldSlotLayout = Managers.UI.MakeWorldSpaceUI<KeywordWorldSlotLayoutController>(transform,"KeywordWorldSlotLayout");
        for(int i = 0; i < keywordSlot; ++i) 
        {
            CreateKeywordFrame();
            CreateKeywordWorldSlotUI();
        }

        col = Util.GetOrAddComponent<Collider>(gameObject);
        keywordWorldSlotLayout.SortChild(2.1f);
    }

    private void CreateKeywordFrame() 
    {
        var slot = Managers.UI.MakeSubItem<KeywordFrameController>(keywordSlotLayout, "KeywordSlotUI");
        keywordSlotUI.Add(slot);
    }
    private void CreateKeywordWorldSlotUI() 
    {
        keywordSlotWorldUI.Add(Managers.UI.MakeWorldSpaceUI<KeywordWorldSlotUIController>(keywordWorldSlotLayout.Panel, "KeywordSlotWorldSpace"));
    }

    public virtual void EnterDebugMod()
    {
        OpenWorldSlotUI();
        outlineEffect.OutLineGo.SetActive(true);
    }
    public virtual void ExitDebugMod() 
    {
        CloseWorldSlotUI();
        outlineEffect.OutLineGo.SetActive(false);
    }

    public void CloseWorldSlotUI() 
    {
        foreach(var slot in keywordSlotWorldUI) 
        {
            slot.Close();
        }
    }
    public void OpenWorldSlotUI() 
    {
        foreach (var slot in keywordSlotWorldUI)
        {
            slot.Open();
        }
    }

    public void OpenKeywordSlot() 
    {
        foreach (var slot in keywordSlotUI)
        {
            slot.Open();
        }
    }
    public void CloseKeywordSlot()
    {
        foreach (var slot in keywordSlotUI)
        {
            slot.Close();
        }
    }
    public void AddOverrideTable(string id,KeywordAction action) 
    {
        if(keywrodOverrideTable.ContainsKey(id)) 
        {
            return;
        }
        keywrodOverrideTable.Add(id,action);
    }
    public void AddAction(KeywordAction action) 
    {
        switch(action.ActionType) 
        {
            case KeywordActionType.OnUpdate:
                updateAction -= action.Action;
                updateAction += action.Action;
                break;

            case KeywordActionType.OneShot:
                oneShotAction -= action.Action;
                oneShotAction += action.Action;
                break;
        }
    }
    public void DecisionKeyword()
    {
        ClearAction();
        for(int i = 0; i< keywordSlotUI.Count; ++i)
        {
            if (keywordSlotUI[i].KeywordController == null)
            {
                keywordSlotWorldUI[i].ResetSlotUI();
                continue;
            }
            keywordSlotWorldUI[i].SetSlotUI(keywordSlotUI[i].KeywordController.Image.color, keywordSlotUI[i].KeywordController.KeywordText.text);

            var keywordId = keywordSlotUI[i].KeywordController.KewordId;
            KeywordAction keywordAciton;
            if(!keywrodOverrideTable.TryGetValue(keywordId,out keywordAciton)) 
            {
                keywordAciton = new KeywordAction(keywordSlotUI[i].KeywordController.KeywordAction,keywordSlotUI[i].KeywordController.KeywordType);
            }
            
            AddAction(keywordAciton);
        }
        oneShotAction?.Invoke(this);
    }

    public void ClearAction() 
    {
        oneShotAction = null;
        updateAction = null;
    }
    public void Update() 
    {
        updateAction?.Invoke(this);
    }

    public bool ColisionCheckMove(Vector3 vec)
    {
        var pos = transform.position;
        
        RaycastHit hit;
        int layer = 1;
        foreach(var name in Enum.GetNames(typeof(Define.ColiiderMask))) 
        {
            layer += (1 << (LayerMask.NameToLayer(name)));
        }

        Physics.BoxCast(pos,col.bounds.extents,vec.normalized,out hit ,Quaternion.identity, col.bounds.extents.magnitude/2,layer);
        if(hit.collider != null && hit.collider != col) 
        {
            return false;
        }

        pos += vec*Time.deltaTime;
        transform.position = pos;
        return true;

    }
    public void Init() 
    {
        
    }
}
