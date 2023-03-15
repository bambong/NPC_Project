using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;


public class KeywordAction 
{
    private Action<KeywordEntity> onEnter;
    private Action<KeywordEntity> onUpdate;
    private Action<KeywordEntity> onFixecUpdate;
    private Action<KeywordEntity> onRemove;
    public Action<KeywordEntity> OnEnter { get => onEnter; set => onEnter = value; }
    public Action<KeywordEntity> OnUpdate { get => onUpdate; set => onUpdate = value; }
    public Action<KeywordEntity> OnFixecUpdate { get => onFixecUpdate; set => onFixecUpdate = value; }
    public Action<KeywordEntity> OnRemove { get => onRemove; set => onRemove = value; }

    public KeywordAction() 
    {
    }
    public KeywordAction(KeywordController keywordController) 
    {
        OnEnter += keywordController.OnEnter;
        OnFixecUpdate += keywordController.OnFixedUpdate;
        OnUpdate += keywordController.OnUpdate;
        OnRemove += keywordController.OnRemove;
    }
    public void OverrideKeywordAction(KeywordAction overrideAction) 
    {
        OverrideAction(ref onEnter, overrideAction.OnEnter);
        OverrideAction(ref onFixecUpdate, overrideAction.OnFixecUpdate);
        OverrideAction(ref onUpdate, overrideAction.OnUpdate);
        OverrideAction(ref onRemove, overrideAction.OnRemove);
    }
    private void OverrideAction(ref Action<KeywordEntity> origin, Action<KeywordEntity> overAction) 
    {
        if(overAction == null) 
        {
            return;
        }
        origin = overAction;

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
    //private List<KeywordWorldSlotUIController> keywordtWorldFrames = new List<KeywordWorldSlotUIController>();

    private Action<KeywordEntity> updateAction = null;
    private Action<KeywordEntity> fixedUpdateAction = null;
    private Rigidbody rigidbody;
    private BoxCollider col;
    private KeywordSlotUiController keywordSlotUiController;
    //private KeywordWorldSlotLayoutController keywordWorldSlotLayout;
    private LookAtCameraController keywordDebugIcon;
    private DebugZone parentDebugZone;
    private readonly float SLOT_UI_DISTANCE = 100f;
    public Dictionary<KeywordController,KeywordAction> CurrentRegisterKeyword { get => currentRegisterKeyword; }
    public virtual Transform KeywordTransformFactor { get => transform; }
    public Vector3 OriginScale { get; private set; }
    public Vector3 MaxScale { get => maxScale; }
    public bool IsAvailable { get => parentDebugZone == Managers.Keyword.CurDebugZone; }
    public KeywordSlotUiController KeywordSlotUiController { get => keywordSlotUiController;}
    private readonly float SCREEN_OFFSET = new Vector2(1920, 1080).magnitude; 
    private void Start()
    {
        OriginScale = transform.lossyScale;
        Managers.Keyword.AddSceneEntity(this);
        keywordSlotUiController = Managers.UI.MakeSubItem<KeywordSlotUiController>(Managers.Keyword.KeywordEntitySlots, "KeywrodSlotController");
        keywordSlotUiController.RegisterEntity(this);
        keywordDebugIcon = Managers.UI.MakeWorldSpaceUI<LookAtCameraController>(null, "KeywordWorldUI");
        keywordDebugIcon.RegisterEntity(transform);
        //keywordWorldSlotLayout = Managers.UI.MakeWorldSpaceUI<KeywordWorldSlotLayoutController>(null,"KeywordWorldSlotLayout");
        //keywordWorldSlotLayout.RegisterEntity(transform);

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
        //keywordWorldSlotLayout.SortChild(2.1f);
        DecisionKeyword();
    }
    private void Update()
    {
        updateAction?.Invoke(this);
    }
    public void FixedUpdate()
    {
        if (Managers.Game.IsDebugMod)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            var factor = SCREEN_OFFSET / new Vector2(Screen.width, Screen.height).magnitude;
            pos.z = 0;
            if ((Input.mousePosition - pos).magnitude * factor <= SLOT_UI_DISTANCE)
            {
                OpenKeywordSlot();
            }
            else
            {
                CloseKeywordSlot();
            }
        }
        fixedUpdateAction?.Invoke(this);
    }
    private void InitCrateKeywordOption()
    {
        for (int i = 0; i < keywords.Length; ++i)
        {
            var frame = Managers.UI.MakeSubItem<KeywordFrameController>(keywordSlotUiController.KeywordSlotLayout, "KeywordSlotUI");
            keywordFrames.Add(frame);
            //var worldFrame = Managers.UI.MakeWorldSpaceUI<KeywordWorldSlotUIController>(keywordWorldSlotLayout.Panel, "KeywordSlotWorldSpace");
          //  keywordtWorldFrames.Add(worldFrame);
            frame.RegisterEntity(this);
            if (keywords[i].keywordGo == null) 
            {
                continue;
            }

            var keyword = Managers.UI.MakeSubItem<KeywordController>(null, "KeywordPrefabs/" + keywords[i].keywordGo.name);

            frame.InitKeyword(keyword);
            keyword.SetDebugZone(parentDebugZone);
            if (keywords[i].isLock) 
            {
                frame.SetLockFrame(true);
            }
        }
    }
    public void SetDebugZone(DebugZone zone) => parentDebugZone = zone;
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
        keywordDebugIcon.gameObject.SetActive(false);
        //foreach(var slot in keywordtWorldFrames) 
        //{
        //    slot.Close();
        //}
    }
    public void OpenWorldSlotUI() 
    {
        //foreach (var slot in keywordtWorldFrames)
        keywordDebugIcon.gameObject.SetActive(true);
        //{
        //    slot.Open();
        //}
    }

    public void OpenKeywordSlot() 
    {
        keywordSlotUiController.Open();
    }
    public void CloseKeywordSlot()
    {
        keywordSlotUiController.Close();
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
        action.OnEnter.Invoke(this);
        fixedUpdateAction += action.OnFixecUpdate;
        updateAction += action.OnUpdate;
        currentRegisterKeyword[controller] = action;
    }
    public void RemoveAction(KeywordFrameController keywordFrame)
    {
        if(!currentRegisterKeyword.ContainsKey(keywordFrame.RegisterKeyword))
        {
            Debug.LogError("포함되지않은 키워드 삭제 시도");
            return;
        }
        // 다른 슬롯에 들어가 있는지 확인
        for(int i = 0; i < keywordFrames.Count; ++i) 
        {
            if(keywordFrames[i].CurFrameInnerKeyword == keywordFrame.RegisterKeyword)
            {
                return;
            }
        }
        var action = currentRegisterKeyword[keywordFrame.RegisterKeyword];

        fixedUpdateAction -= action.OnFixecUpdate;
        updateAction -= action.OnUpdate;
        if (!keywordFrame.HasKeyword || keywordFrame.RegisterKeyword.KewordId != keywordFrame.CurFrameInnerKeyword.KewordId) 
        {
            currentRegisterKeyword[keywordFrame.RegisterKeyword]?.OnRemove(this);
        }
        currentRegisterKeyword.Remove(keywordFrame.RegisterKeyword);
    }
    public void DecisionKeyword(KeywordFrameController keywordFrame) 
    {
        // 현재 프레임 안에 들어있는 키워드
        var curFrameInnerKeyword = keywordFrame.CurFrameInnerKeyword;
        // 기존 프레임에 등록되어 있던 키워드
        var frameRegisterKeyword = keywordFrame.RegisterKeyword;
        //기존 키워드가 제거 혹은 변경됬다면 
        if (keywordFrame.IsKeywordRemoved)
        {
            //키워드 Remove 이벤트 발생 
            //Entity 에 등록된 키워드 리스트에서 키워드 제거
            RemoveAction(keywordFrame);
        }
        //현재 FrameInnerKeyword 를 프레임에 등록
        keywordFrame.OnDecisionKeyword();

        // 프레임안에 키워드가 없다면 
        if (curFrameInnerKeyword == null)
        {
            //월드 키워드 UI 를 리셋하고 다시 순회 
            //keywordFrame.KeywordWorldSlot.ResetSlotUI();
            return;
        }
        ////월드 키워드 UI 설정  
        //keywordFrame.KeywordWorldSlot.SetSlotUI(curFrameInnerKeyword.Image);

        // 이미 등록된 키워드라면 다시 순회  
        if (currentRegisterKeyword.ContainsKey(curFrameInnerKeyword))
        {
            return;
        }

        var keywordId = curFrameInnerKeyword.KewordId;
        var keywordAciton = new KeywordAction(curFrameInnerKeyword);
        KeywordAction overAction;
        // 키워드가 오버라이딩 되어 있는지 확인하고 키워드 액션에 할당
        if (keywrodOverrideTable.TryGetValue(keywordId, out overAction))
        {
            keywordAciton.OverrideKeywordAction(overAction);
        }
        //키워드 액션을 추가
        AddAction(curFrameInnerKeyword, keywordAciton);
    }
    public void DecisionKeyword()
    {
        // 키워드 프레임을 순회
        for(int i = 0; i< keywordFrames.Count; ++i)  
        {
            DecisionKeyword(keywordFrames[i]);
        }
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
        boxSize.y = boxSize.y * 0.99f;
        var vecXSize = Mathf.Abs(vec.x);
        var vecZSize = Mathf.Abs(vec.z);
        var boxXdiff = boxSize.x * 0.01f;
        var boxZdiff = boxSize.z * 0.01f;

        if (vecXSize == 0 || vecXSize >= boxXdiff)
        { 
            boxSize.x *= 0.99f;
        }
        if(vecZSize == 0 || vecZSize >= boxZdiff)
        { 
            boxSize.z *= 0.99f;
        }
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
        rigidbody.isKinematic = isOn;
    }
    public void ClearVelocity()=> rigidbody.velocity = Vector3.zero;
    #endregion

}
