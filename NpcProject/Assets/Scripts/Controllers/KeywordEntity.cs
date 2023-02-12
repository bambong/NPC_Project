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
            Debug.LogError("포함되지않은 키워드 삭제 시도");
            return;
        }
        // 다른 슬롯에 들어가 있는지 확인
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
        // 키워드 프레임을 순회
        for(int i = 0; i< keywordSlotUI.Count; ++i)  
        {
            // 현재 프레임 안에 들어있는 키워드
            var curFrameInnerKeyword = keywordSlotUI[i].CurFrameInnerKeyword;
            // 기존 프레임에 등록되어 있던 키워드
            var frameRegisterKeyword = keywordSlotUI[i].RegisterKeyword;
            KeywordAction keywordAciton;
            //기존 키워드가 제거 혹은 변경됬다면 
            if(keywordSlotUI[i].IsKeywordRemoved)
            {
                //키워드 Remove 이벤트 발생 
                //Entity 에 등록된 키워드 리스트에서 키워드 제거
                RemoveAction(frameRegisterKeyword);
            }
            //현재 FrameInnerKeyword 를 프레임에 등록
            keywordSlotUI[i].OnDecisionKeyword();

            // 프레임안에 키워드가 없다면 
            if(curFrameInnerKeyword == null)
            {
                //월드 키워드 UI 를 리셋하고 다시 순회 
                keywordSlotWorldUI[i].ResetSlotUI();
                continue;
            }
            //월드 키워드 UI 설정  
            keywordSlotWorldUI[i].SetSlotUI(curFrameInnerKeyword.Image.color,curFrameInnerKeyword.KeywordText.text);

            // 이미 등록된 키워드라면 다시 순회  
            if(currentRegisterKeyword.ContainsKey(curFrameInnerKeyword)) 
            {
                continue;
            }

            var keywordId = curFrameInnerKeyword.KewordId;
            // 키워드가 오버라이딩 되어 있는지 확인하고 키워드 액션에 할당
            if(!keywrodOverrideTable.TryGetValue(keywordId,out keywordAciton))
            {
                keywordAciton = new KeywordAction(curFrameInnerKeyword.KeywordAction,curFrameInnerKeyword.KeywordType,curFrameInnerKeyword.OnRemove);
            }
            // Entity 가  OnRemove 이벤트 핸들러를 오버라이딩 안했다면 Default 핸들러를 넣어준다 
            if(keywordAciton.OnRemove == null) 
            {
                keywordAciton.AddOnRemoveEvent(curFrameInnerKeyword.OnRemove);
            }
            //OneShot Action 의 경우 실행 
            //키워드 액션을 추가
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
