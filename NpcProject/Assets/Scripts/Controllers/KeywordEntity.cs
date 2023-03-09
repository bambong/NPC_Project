using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

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

[Serializable]
class CreateKeywordOption 
{
    public bool isLock = false;
    public GameObject keywordGo;
}

public class KeywordEntity : MonoBehaviour
{

    [SerializeField]
    private float maxHeight = 3;
    [SerializeField]
    private Vector3 maxScale = Vector3.one * 2;

    [Header("Make Keyword")]
    [SerializeField]
    private CreateKeywordOption[] keywords;

    private Dictionary<string,KeywordAction> keywrodOverrideTable = new Dictionary<string,KeywordAction>();
    private Dictionary<KeywordController,KeywordAction> currentRegisterKeyword = new Dictionary<KeywordController,KeywordAction>();
   
    private List<KeywordFrameController> keywordFrames = new List<KeywordFrameController>();
    private List<KeywordWorldSlotUIController> keywordtWorldFrames = new List<KeywordWorldSlotUIController>();

    private Action<KeywordEntity> fixedUpdateAction = null;
    private Rigidbody rigidbody;
    private BoxCollider col;
    private Transform keywordSlotLayout;
    private KeywordWorldSlotLayoutController keywordWorldSlotLayout;

    public Dictionary<KeywordController,KeywordAction> CurrentRegisterKeyword { get => currentRegisterKeyword; }
    private DebugZone parentDebugZone;
    public virtual Transform KeywordTransformFactor { get => transform; }
    public Vector3 OriginScale { get; private set; }
    public Vector3 MaxScale { get => maxScale; }
    public bool IsAvailable { get => parentDebugZone == Managers.Keyword.CurDebugZone; }

    private void Start()
    {
        OriginScale = transform.lossyScale;
        Managers.Keyword.AddSceneEntity(this);
        keywordSlotLayout = Managers.Resource.Instantiate("UI/KeywordSlotLayout",Managers.Keyword.PlayerKeywordPanel.transform).transform;
        keywordWorldSlotLayout = Managers.UI.MakeWorldSpaceUI<KeywordWorldSlotLayoutController>(null,"KeywordWorldSlotLayout");
        keywordWorldSlotLayout.RegisterEntity(transform);

        InitCrateKeywordOption();

        if (!TryGetComponent<BoxCollider>(out col))
        {
            Collider temp;
            if(TryGetComponent<Collider>(out temp))
            {
                temp.enabled = false;
            }
            col = Util.GetOrAddComponent<BoxCollider>(gameObject);
        }
        TryGetComponent<Rigidbody>(out rigidbody);
        keywordWorldSlotLayout.SortChild(2.1f);
        DecisionKeyword();
    }
   
    private void InitCrateKeywordOption()
    {
        for (int i = 0; i < keywords.Length; ++i)
        {
            var frame =  CreateKeywordFrame();
            CreateKeywordWorldSlotUI();
            if (keywords[i].keywordGo == null) 
            {
                continue;
            }

            var keyword = Managers.UI.MakeSubItem<KeywordController>(null, "KeywordPrefabs/" + keywords[i].keywordGo.name);

            frame.SetKeyWord(keyword);
            keyword.SetFrame(frame);
            keyword.SetDebugZone(parentDebugZone);
            if (keywords[i].isLock) 
            {
                frame.SetLockFrame(true);
            }

        }
    }
    //private bool RegisterKeyword(KeywordController keyword)
    //{
    //    for (int i = 0; i < keywordSlotUI.Count; ++i)
    //    {
    //        if (keywordSlotUI[i].HasKeyword)
    //        {
    //            continue;
    //        }
    //        keywordSlotUI[i].SetKeyWord(keyword);
    //        keyword.SetFrame(keywordSlotUI[i]);
    //        keyword.SetDebugZone(parentDebugZone);
    //        return true;
    //    }

    //    return false;
    //}
    public void SetDebugZone(DebugZone zone) => parentDebugZone = zone;
    private KeywordFrameController CreateKeywordFrame() 
    {
        var frame = Managers.UI.MakeSubItem<KeywordFrameController>(keywordSlotLayout, "KeywordSlotUI");
        keywordFrames.Add(frame);
        return frame;
    }
    private void CreateKeywordWorldSlotUI() 
    {
        keywordtWorldFrames.Add(Managers.UI.MakeWorldSpaceUI<KeywordWorldSlotUIController>(keywordWorldSlotLayout.Panel, "KeywordSlotWorldSpace"));
    }

    public virtual void EnterDebugMod()
    {
        OpenWorldSlotUI();
    }
    public virtual void ExitDebugMod() 
    {
        CloseWorldSlotUI();
    }

    public void CloseWorldSlotUI() 
    {
        foreach(var slot in keywordtWorldFrames) 
        {
            slot.Close();
        }
    }
    public void OpenWorldSlotUI() 
    {
        foreach (var slot in keywordtWorldFrames)
        {
            slot.Open();
        }
    }

    public void OpenKeywordSlot() 
    {
        foreach (var slot in keywordFrames)
        {
            slot.Open();
        }
    }
    public void CloseKeywordSlot()
    {
        foreach (var slot in keywordFrames)
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
                fixedUpdateAction += action.Action;
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
        for(int i = 0; i < keywordFrames.Count; ++i) 
        {
            if(keywordFrames[i].CurFrameInnerKeyword == keywordController)
            {
                return;
            }
        }
        var action = currentRegisterKeyword[keywordController];

        switch(action.ActionType)
        {
            case KeywordActionType.OnUpdate:
                fixedUpdateAction -= action.Action;
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
        for(int i = 0; i< keywordFrames.Count; ++i)  
        {
            // 현재 프레임 안에 들어있는 키워드
            var curFrameInnerKeyword = keywordFrames[i].CurFrameInnerKeyword;
            // 기존 프레임에 등록되어 있던 키워드
            var frameRegisterKeyword = keywordFrames[i].RegisterKeyword;
            KeywordAction keywordAciton;
            //기존 키워드가 제거 혹은 변경됬다면 
            if(keywordFrames[i].IsKeywordRemoved)
            {
                //키워드 Remove 이벤트 발생 
                //Entity 에 등록된 키워드 리스트에서 키워드 제거
                RemoveAction(frameRegisterKeyword);
            }
            //현재 FrameInnerKeyword 를 프레임에 등록
            keywordFrames[i].OnDecisionKeyword();

            // 프레임안에 키워드가 없다면 
            if(curFrameInnerKeyword == null)
            {
                //월드 키워드 UI 를 리셋하고 다시 순회 
                keywordtWorldFrames[i].ResetSlotUI();
                continue;
            }
            //월드 키워드 UI 설정  
            keywordtWorldFrames[i].SetSlotUI(curFrameInnerKeyword.Image);

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
        fixedUpdateAction = null;
    }
    public void FixedUpdate() 
    {
        fixedUpdateAction?.Invoke(this);
    }
    #region Keyword_Control

    public bool ColisionCheckRotate(Vector3 vec)
    {
        var pos = col.transform.position;
        RaycastHit hit;
        int layer = 1;
        foreach(var name in Enum.GetNames(typeof(Define.ColiiderMask)))
        {
            layer += (1 << (LayerMask.NameToLayer(name)));
        }
        var boxSize = Util.VectorMultipleScale(col.size/2,transform.lossyScale)* 0.99f;
        var rot = KeywordTransformFactor.rotation * Quaternion.Euler(vec);
#if UNITY_EDITOR
        ExtDebug.DrawBox(pos,boxSize,rot,Color.blue);
#endif
        var hits = Physics.OverlapBox(pos,boxSize,rot,layer,QueryTriggerInteraction.Ignore);
        if(hits.Length > 1)
        {
            return false;
        }

        KeywordTransformFactor.Rotate(vec);
        return true;

    }
    public bool ColisionCheckMove(Vector3 vec)
    {
        var pos = col.transform.position;

        RaycastHit hit;
        int layer = 1;
        foreach (var name in Enum.GetNames(typeof(Define.ColiiderMask)))
        {
            layer += (1 << (LayerMask.NameToLayer(name)));
        }
        var boxSize = Util.VectorMultipleScale(col.size / 2, transform.lossyScale);
        boxSize *= 0.99f;
#if UNITY_EDITOR
        ExtDebug.DrawBox(pos + vec, boxSize, KeywordTransformFactor.rotation, Color.blue);
#endif
        var hits = Physics.OverlapBox(pos+vec, boxSize, KeywordTransformFactor.rotation, layer, QueryTriggerInteraction.Ignore);
        
        for(int i = 0; i< hits.Length; ++i) 
        {
            if(hits[i] != col) 
            {
                return false;
            }
        }
        KeywordTransformFactor.position += vec;
        return true;

    }

    public bool FloatMove(float speed)
    {
        var pos = col.transform.position;
        RaycastHit hit;
        int layer = 1;
        foreach(var name in Enum.GetNames(typeof(Define.ColiiderMask)))
        {
            layer += (1 << (LayerMask.NameToLayer(name)));
        }
        var boxSize = Util.VectorMultipleScale(col.size / 2,transform.lossyScale);
        boxSize *= 0.99f;
        boxSize.y = 0;
        var rayDis = maxHeight + col.bounds.size.y;
#if UNITY_EDITOR
        ExtDebug.DrawBoxCastBox(pos,boxSize,KeywordTransformFactor.rotation, Vector3.down,rayDis,Color.red);
#endif
        Physics.BoxCast(pos,boxSize,Vector3.down,out hit,KeywordTransformFactor.rotation,rayDis,layer);
        if (hit.collider != null)
        {
            var realDis = hit.distance - col.bounds.extents.y;
            if(realDis <= maxHeight) 
            {
                var remainDis = maxHeight - realDis;
                if(remainDis < 0.1f)
                {
                    return true;
                }
                if(remainDis < speed)
                {
                    speed =  remainDis;
                }
                var upVec = Vector3.up * speed;
                ColisionCheckMove(upVec);
                return true;
            }
            else 
            {
                var remainDis = realDis - maxHeight;
                if (remainDis < 0.1f)
                {
                    return true;
                }
                if (remainDis < speed)
                {
                    speed = remainDis;
                }
                var downVec = Vector3.down * speed;
                KeywordTransformFactor.position += downVec;
                return true;
            }
           
        }
        KeywordTransformFactor.position += Vector3.down * speed;
        return false;
    }
    public bool ColisionCheckScale(Vector3 desireScale, GameObject dummyParent) 
    {
        var desireBoxSize = Util.VectorMultipleScale(col.size / 2, desireScale);
        var curBoxSize = Util.VectorMultipleScale(col.size / 2, transform.lossyScale);
        var boxScaleDiff = (desireBoxSize - curBoxSize);

        Vector3 parentPos = Vector3.zero;
        Vector3 rayBox = curBoxSize * 0.99f;

        if (CheckAxis(transform.right * desireBoxSize.x, new Vector3(0, rayBox.y, rayBox.z), boxScaleDiff.x, curBoxSize.x, ref parentPos)
        || CheckAxis(transform.up * desireBoxSize.y, new Vector3(rayBox.x, 0, rayBox.z), boxScaleDiff.y, curBoxSize.y, ref parentPos)
        || CheckAxis(transform.forward * desireBoxSize.z, new Vector3(rayBox.x, rayBox.y, 0), boxScaleDiff.z, curBoxSize.z, ref parentPos))
        {
            return false;
        }

        dummyParent.transform.localScale = transform.lossyScale;
        dummyParent.transform.rotation = transform.rotation;
        dummyParent.transform.position = transform.position + parentPos;
        transform.SetParent(dummyParent.transform);
        dummyParent.transform.localScale = desireScale;
        transform.SetParent(null);
        return true;
    }
    private bool CheckAxis(Vector3 rayDir, Vector3 rayBox, float boxScaleDiff, float parentPosFactor, ref Vector3 parentPos)
    {
        if (ColisionCheckBox(rayDir, rayBox))
        {
            if (ColisionCheckBox(AddDis(-rayDir, boxScaleDiff), rayBox))
            {
                return true;
            }
            parentPos += rayDir.normalized * parentPosFactor;
        }
        else if (ColisionCheckBox(-rayDir, rayBox))
        {
            if (ColisionCheckBox(AddDis(rayDir, boxScaleDiff), rayBox))
            {
                return true;
            }
            parentPos -= rayDir.normalized * parentPosFactor;
        }
        return false;
    }
    public bool ColisionCheckBox(Vector3 vec , Vector3 boxSize) 
    {
        var pos = col.transform.position;

        RaycastHit hit;
        int layer = 1;
        foreach (var name in Enum.GetNames(typeof(Define.ColiiderMask)))
        {
            layer += (1 << (LayerMask.NameToLayer(name)));
        }
#if UNITY_EDITOR
        ExtDebug.DrawBox(pos + vec, boxSize, KeywordTransformFactor.rotation, Color.blue);
#endif
        var hits = Physics.OverlapBox(pos + vec, boxSize, KeywordTransformFactor.rotation, layer, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i] != col)
            {
                return true;
            }
        }
        return false;
    }
    private Vector3 AddDis(Vector3 vec, float amount)
    {
        return vec + vec.normalized * amount;
    }
    public void SetGravity(bool isOn) 
    {
        rigidbody.useGravity = isOn;
    }
    public void SetKinematic(bool isOn) 
    {
        //if(isOn)
        //{
        //    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        //}
        //else 
        //{
        //    rigidbody.constraints = RigidbodyConstraints.FreezeAll^RigidbodyConstraints.FreezePositionY;
        //}

        rigidbody.isKinematic = isOn;
    }
    public void ClearVelocity()=> rigidbody.velocity = Vector3.zero;
    #endregion
    public void Init() 
    {
        
    }
}
