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
    private Action<KeywordEntity> onRemove;
    public Action<KeywordEntity> Action { get => action; }
    public Action<KeywordEntity> OnRemove { get => onRemove; }
    public KeywordActionType ActionType { get => actiontype;  }

    public KeywordAction(Action<KeywordEntity> action,KeywordActionType actiontype,Action<KeywordEntity> onRemove = null) 
    {
        this.action = action;
        this.actiontype = actiontype;
        this.onRemove = onRemove;
    }
    public void AddOnRemoveEvent(Action<KeywordEntity> onRemove) 
    {
        this.onRemove += onRemove;
    }

}



public class KeywordEntity : MonoBehaviour
{
    [SerializeField]
    private int keywordSlot = 1;

    [SerializeField]
    private OutlineEffect outlineEffect;

    private static Dictionary<string,KeywordAction> keywrodOverrideTable = new Dictionary<string,KeywordAction>();
    private Dictionary<KeywordController,Action<KeywordEntity>> currentRegisterKeyword = new Dictionary<KeywordController,Action<KeywordEntity>>();
    private List<KeywordFrameController> keywordSlotUI = new List<KeywordFrameController>();
    private List<KeywordWorldSlotUIController> keywordSlotWorldUI = new List<KeywordWorldSlotUIController>();

    private Action<KeywordEntity> updateAction = null;
    private Action<KeywordEntity> oneShotAction = null;
    private Action<KeywordEntity> onRemove = null;
    private Collider col;
    private Transform keywordSlotLayout;
    private KeywordWorldSlotLayoutController keywordWorldSlotLayout;
    private void Start()
    {
        Managers.Keyword.AddSceneEntity(this);
        keywordSlotLayout = Managers.Resource.Instantiate("UI/KeywordSlotLayout",Managers.Keyword.PlayerKeywordPanel.transform).transform;
        keywordWorldSlotLayout = Managers.UI.MakeWorldSpaceUI<KeywordWorldSlotLayoutController>(null,"KeywordWorldSlotLayout");
        keywordWorldSlotLayout.RegisterEntity(transform);
        //keywordWorldSlotLayout.transform.SetParent(transform,false);
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
            var keywordController = keywordSlotUI[i].KeywordController;
            KeywordAction keywordAciton;
            if(!keywordSlotUI[i].HasKeyword)
            {
                keywordSlotWorldUI[i].ResetSlotUI();
                
                if(keywordController == null) 
                {
                    continue;
                }

                if(!keywrodOverrideTable.TryGetValue(keywordController.KewordId,out keywordAciton))
                {
                    keywordController?.OnRemove(this);
                }
                else 
                {
                    keywordAciton?.OnRemove(this);
                }
                
                keywordSlotUI[i].OnRemove();
                
                continue;
            }
           
            keywordSlotWorldUI[i].SetSlotUI(keywordController.Image.color,keywordController.KeywordText.text);

            var keywordId = keywordController.KewordId;
            
            if(!keywrodOverrideTable.TryGetValue(keywordId,out keywordAciton))
            {
                keywordAciton = new KeywordAction(keywordController.KeywordAction,keywordController.KeywordType,keywordController.OnRemove);
            }
            if(keywordAciton.OnRemove == null) 
            {
                keywordAciton.AddOnRemoveEvent(keywordController.OnRemove);
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
