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

    private Dictionary<string,KeywordAction> keywrodOverrideTable = new Dictionary<string,KeywordAction>();
    private Dictionary<KeywordController,KeywordAction> currentRegisterKeyword = new Dictionary<KeywordController,KeywordAction>();
    private List<KeywordFrameController> keywordSlotUI = new List<KeywordFrameController>();
    private List<KeywordWorldSlotUIController> keywordSlotWorldUI = new List<KeywordWorldSlotUIController>();

    private Action<KeywordEntity> updateAction = null;

    private Collider col;
    private Transform keywordSlotLayout;
    private KeywordWorldSlotLayoutController keywordWorldSlotLayout;

    public Dictionary<KeywordController,KeywordAction> CurrentRegisterKeyword { get => currentRegisterKeyword; }

    protected Transform transformTarget;

    public Transform TransformTarget { get => transformTarget; }

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
    public void AddAction(KeywordController controller,KeywordAction action) 
    {
        switch(action.ActionType) 
        {
            case KeywordActionType.OnUpdate:
                updateAction += action.Action;
                break;

            case KeywordActionType.OneShot:
                action.Action?.Invoke(this);
                break;
        }
        currentRegisterKeyword[controller] = action;
    }
    public void RemoveAction(KeywordController keywordController)
    {
        if(!currentRegisterKeyword.ContainsKey(keywordController))
        {
            Debug.LogError("���Ե������� Ű���� ���� �õ�");
            return;
        }
        // �ٸ� ���Կ� �� �ִ��� Ȯ��
        for(int i = 0; i < keywordSlotUI.Count; ++i) 
        {
            if(keywordSlotUI[i].CurFrameInnerKeyword == keywordController)
            {
                return;
            }
        }
        var action = currentRegisterKeyword[keywordController];

        switch(action.ActionType)
        {
            case KeywordActionType.OnUpdate:
                updateAction -= action.Action;
                break;

            case KeywordActionType.OneShot:
                break;
        }
        currentRegisterKeyword[keywordController]?.OnRemove(this);
        currentRegisterKeyword.Remove(keywordController);
    }
    public void DecisionKeyword()
    {
        // Ű���� �������� ��ȸ
        for(int i = 0; i< keywordSlotUI.Count; ++i)  
        {
            // ���� ������ �ȿ� ����ִ� Ű����
            var curFrameInnerKeyword = keywordSlotUI[i].CurFrameInnerKeyword;
            // ���� �����ӿ� ��ϵǾ� �ִ� Ű����
            var frameRegisterKeyword = keywordSlotUI[i].RegisterKeyword;
            KeywordAction keywordAciton;
            //���� Ű���尡 ���� Ȥ�� �����ٸ� 
            if(keywordSlotUI[i].IsKeywordRemoved)
            {
                //Ű���� Remove �̺�Ʈ �߻� 
                //Entity �� ��ϵ� Ű���� ����Ʈ���� Ű���� ����
                RemoveAction(frameRegisterKeyword);
            }
            //���� FrameInnerKeyword �� �����ӿ� ���
            keywordSlotUI[i].OnDecisionKeyword();

            // �����Ӿȿ� Ű���尡 ���ٸ� 
            if(curFrameInnerKeyword == null)
            {
                //���� Ű���� UI �� �����ϰ� �ٽ� ��ȸ 
                keywordSlotWorldUI[i].ResetSlotUI();
                continue;
            }
            //���� Ű���� UI ����  
            keywordSlotWorldUI[i].SetSlotUI(curFrameInnerKeyword.Image.color,curFrameInnerKeyword.KeywordText.text);

            // �̹� ��ϵ� Ű������ �ٽ� ��ȸ  
            if(currentRegisterKeyword.ContainsKey(curFrameInnerKeyword)) 
            {
                continue;
            }

            var keywordId = curFrameInnerKeyword.KewordId;
            // Ű���尡 �������̵� �Ǿ� �ִ��� Ȯ���ϰ� Ű���� �׼ǿ� �Ҵ�
            if(!keywrodOverrideTable.TryGetValue(keywordId,out keywordAciton))
            {
                keywordAciton = new KeywordAction(curFrameInnerKeyword.KeywordAction,curFrameInnerKeyword.KeywordType,curFrameInnerKeyword.OnRemove);
            }
            // Entity ��  OnRemove �̺�Ʈ �ڵ鷯�� �������̵� ���ߴٸ� Default �ڵ鷯�� �־��ش� 
            if(keywordAciton.OnRemove == null) 
            {
                keywordAciton.AddOnRemoveEvent(curFrameInnerKeyword.OnRemove);
            }
            //OneShot Action �� ��� ���� 
            //Ű���� �׼��� �߰�
            AddAction(curFrameInnerKeyword,keywordAciton);         
        }
    }

    public void ClearAction() 
    {
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
        var moveVec = vec * Time.deltaTime;
        Physics.BoxCast(pos,col.bounds.extents,vec.normalized,out hit ,Quaternion.identity,moveVec.magnitude,layer);
        if(hit.collider != null && hit.collider != col) 
        {
            return false;
        }

        pos += moveVec;
        transform.position = pos;
        return true;

    }
    public void Init() 
    {
        
    }
}
