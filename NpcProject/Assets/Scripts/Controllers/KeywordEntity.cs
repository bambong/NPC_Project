using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Searcher;
using UnityEngine;
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

public class KeywordEntity : GuIdBehaviour , IDataHandler
{
    [Header("DEBUG ZONE")]
    [SerializeField]
    private DebugZone parentDebugZone;

    [Space(10)]
    [Header("Keyword Stat")]
    [SerializeField]
    private E_KEYWORD_TYPE availableKeywordType = E_KEYWORD_TYPE.ALL;

    [SerializeField]
    private float maxHeight = 3;
    [SerializeField]
    private Vector3 maxScale = Vector3.one * 2;
    [SerializeField]
    private float revAbleDistance = 1000f;
    [Header("Make Keyword")]
    [SerializeField]
    private CreateKeywordOption[] keywords;

    [SerializeField]
    private string statusSlotLayoutName = "KeywordStatusLayoutController";

   
    #region NeedClearForRespawn
    private Dictionary<string,KeywordAction> keywrodOverrideTable = new Dictionary<string,KeywordAction>();
    private Dictionary<KeywordController,KeywordAction> currentRegisterKeyword = new Dictionary<KeywordController,KeywordAction>();
    private List<KeywordFrameController> keywordFrames = new List<KeywordFrameController>();
    private Action<KeywordEntity> updateAction = null;
    private Action<KeywordEntity> fixedUpdateAction = null;
    #endregion
    private Renderer mRenderer;
    private Material originMat;
   
    private Rigidbody rigidbody;
    protected BoxCollider col;
    protected int colisionCheckLayer;
    private KeywordSlotUiController keywordSlotUiController;
    private KeywordStatusLayoutController keywordWorldSlotLayout;
   
    private bool isInit = false;
    private WireColorStateController wireColorController;
    private bool isMoveAble = true;
    private bool isDestroy = false;
    public Dictionary<KeywordController,KeywordAction> CurrentRegisterKeyword { get => currentRegisterKeyword; }
    public virtual Transform KeywordTransformFactor { get => transform; }
    public Vector3 OriginScale { get; private set; }
    public Vector3 MaxScale { get => maxScale; }
    public bool IsAvailable { get => parentDebugZone == Managers.Keyword.CurDebugZone; }
    public KeywordSlotUiController KeywordSlotUiController { get => keywordSlotUiController;}
    public E_KEYWORD_TYPE AvailableKeywordType { get => availableKeywordType; }
    public Material OriginMat { get => originMat;}
    public Renderer MRenderer { get => mRenderer;}
    public WireColorStateController WireColorController { get => wireColorController; }
    public float RevAbleDistance { get => revAbleDistance; }
    public DebugZone ParentDebugZone { get => parentDebugZone;  }
    public bool IsMoveAble { get => isMoveAble; set => isMoveAble = value; }
    public bool IsDestroy { get => isDestroy; set => isDestroy = value; }

    private readonly float SLOT_UI_DISTANCE = 120f;
    private readonly float SCREEN_OFFSET = new Vector2(1920, 1080).magnitude;
    private readonly string WIRE_FRAME_COLOR_NAME = "_Wireframe_Color";
    private readonly string WIRE_FRAME_EMM_COLOR_NAME = "_EmmColor";
    private const float MINMUM_DIR_AMOUNT = 0.01f;
    protected override void Start()
    {
        base.Start();

        InitColisionLayer();
        if (!TryGetComponent<BoxCollider>(out col))
        {
            Collider temp;
            if (TryGetComponent<Collider>(out temp))
            {
                temp.enabled = false;
            }
            col = Util.GetOrAddComponent<BoxCollider>(gameObject);
        }
        TryGetComponent<Rigidbody>(out rigidbody);
        if(rigidbody == null) 
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.mass = 100;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
        wireColorController = new WireColorStateController();
        wireColorController.Init(this);
        //mRenderer = GetComponent<Renderer>();
        //if (originMat == null)
        //{
        //    originMat = mRenderer.material;
        //}
        
        SetDebugZoneWireMat();
        Init();

    }
    public virtual void Init()
    {
        if(isInit)
        {
            return;
        }
        isInit = true;
        
        mRenderer = GetComponent<Renderer>();
        if (originMat == null)
        {
            originMat = mRenderer.material;
        }

        OriginScale = transform.lossyScale;
        Managers.Keyword.AddSceneEntity(this);
        keywordSlotUiController = Managers.UI.MakeSubItem<KeywordSlotUiController>(Managers.Keyword.KeywordEntitySlots, "KeywordSlotController");
        keywordSlotUiController.RegisterEntity(this);
     
        keywordWorldSlotLayout = Managers.UI.MakeWorldSpaceUI<KeywordStatusLayoutController>(Managers.Keyword.EntityKeywordStatusList, statusSlotLayoutName);
        keywordWorldSlotLayout.RegisterEntity(transform, keywords.Length);
        ClearWireFrameColor();
        ClearWireEmmColor();

        InitCrateKeywordOption();
        
       // DecisionKeyword();
        StartCoroutine(CheckInitDebugMod());
       // wireColorController.UpdateColor();
    }
    IEnumerator CheckInitDebugMod() 
    {
        yield return null;
        if (Managers.Game.IsDebugMod)
        {
            EnterDebugMod();
        }
    }

    private void Update()
    {
        updateAction?.Invoke(this);
    }
    public virtual void FixedUpdate()
    {
     
        fixedUpdateAction?.Invoke(this);
    }
    public virtual void ClearForPool() 
    {
        if (!isInit) 
        {
            return;
        }

        Debug.Log("Clear For Pool");
        if(Managers.Game.Player.PlayerAncestor == transform) 
        {
            Managers.Game.Player.PlayerAncestor = null;
            Managers.Game.Player.transform.SetParent(null);
        }

        //keywrodOverrideTable.Clear();
        currentRegisterKeyword.Clear();
        foreach(var frame in keywordFrames) 
        {
            frame.ClearForPool();
        }
        keywordFrames.Clear();
        mRenderer.material = originMat;
        updateAction = null;
        fixedUpdateAction = null;
        StopAllCoroutines();
        Destroy(keywordWorldSlotLayout.gameObject);
        Destroy(keywordSlotUiController.gameObject);
        Managers.Keyword.RemoveSceneEntity(this);
        isInit = false;
        IsMoveAble = true;
    }

    public void DestroyKeywordEntity() 
    {
        ClearForPool();
        if (gameObject.GetComponent<Poolable>() != null) 
        {
            Managers.Resource.Destroy(gameObject);

        }
        else 
        {
            gameObject.SetActive(false);
        }
    }

    private void InitCrateKeywordOption()
    {
        for (int i = 0; i < keywords.Length; ++i)
        {
            var frame = Managers.UI.MakeSubItem<KeywordFrameController>(keywordSlotUiController.KeywordSlotLayout, "KeywordSlotUI");
            frame.SetKeywordType(availableKeywordType);
            keywordFrames.Add(frame);
            frame.RegisterEntity(this, keywordWorldSlotLayout.KeywordStatusUis[i]);
           
            // 키워드가 미리 생성되어 있는 슬롯인지 확인
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
            DecisionKeyword(frame, keyword);
        }
    }
    public void SetDebugZoneWireMat()
    {
        //parentDebugZone = zone;
        parentDebugZone.AddWireFrameMat(GetComponent<Renderer>().material);
    }
    public virtual void EnterDebugMod()
    {
        StartCoroutine(KeywordSlotUiUpdate());
        OpenWorldSlotUI();
    }
    IEnumerator KeywordSlotUiUpdate() 
    {
        while (Managers.Game.IsDebugMod) 
        {
            if (Managers.Game.IsPause) 
            {
                yield return null;
                continue;
            }
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            var factor = SCREEN_OFFSET / new Vector2(Screen.width, Screen.height).magnitude;
            pos.z = 0;
            if ((Input.mousePosition - pos).magnitude * factor <= SLOT_UI_DISTANCE)
            {
                OpenKeywordSlot();
                keywordWorldSlotLayout.Close(0);
            }
            else
            {
               CloseKeywordSlot();
               keywordWorldSlotLayout.Open(keywordSlotUiController.ConstantAnimTime);
            }
            yield return null;
        }
    }
   

    public virtual void ExitDebugMod() 
    {
        CloseWorldSlotUI();
        CloseKeywordSlot();
    }
    public void OpenWorldSlotUI()
    {
        keywordWorldSlotLayout.Open();
    }
    public void CloseWorldSlotUI() 
    {
        keywordWorldSlotLayout.Close();
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
    public virtual void AddAction(KeywordController controller,KeywordAction action) 
    {
        action.OnEnter.Invoke(this);
        fixedUpdateAction += action.OnFixecUpdate;
        updateAction += action.OnUpdate;
        currentRegisterKeyword[controller] = action;
    }

    public void RemoveAction(KeywordController registerkeyword , KeywordController newKeyword)
    {
        if(registerkeyword == null)
        {
            return;
        }

        if(!currentRegisterKeyword.ContainsKey(registerkeyword))
        {
            Debug.LogError("포함되지않은 키워드 삭제 시도");
            return;
        }
        // 다른 슬롯에 들어가 있는지 확인
        for(int i = 0; i < keywordFrames.Count; ++i) 
        {
            if(keywordFrames[i].CurFrameInnerKeyword == registerkeyword)
            {
                return;
            }
        }
        var action = currentRegisterKeyword[registerkeyword];

        fixedUpdateAction -= action.OnFixecUpdate;
        updateAction -= action.OnUpdate;
        if (newKeyword == null || registerkeyword.KewordId != newKeyword.KewordId) 
        {
            currentRegisterKeyword[registerkeyword]?.OnRemove(this);
        }
        currentRegisterKeyword.Remove(registerkeyword);
    }
    public void DecisionKeyword(KeywordFrameController keywordFrame , KeywordController newKeyword) 
    {
        // 프레임안에 키워드가 없다면 
        if (newKeyword == null)
        {
            //월드 키워드 UI 를 리셋하고 다시 순회 
            keywordFrame.KeywordWorldSlot.UpdateUI(false);
            return;
        }
        ////월드 키워드 UI 설정  
        keywordFrame.KeywordWorldSlot.UpdateUI(true);
        //keywordFrame.KeywordWorldSlot.SetSlotUI(curFrameInnerKeyword.Image);

        // 이미 등록된 키워드라면 다시 순회  
        if (currentRegisterKeyword.ContainsKey(newKeyword))
        {
            return;
        }

        var keywordId = newKeyword.KewordId;
        var keywordAciton = new KeywordAction(newKeyword);
        KeywordAction overAction;
        // 키워드가 오버라이딩 되어 있는지 확인하고 키워드 액션에 할당
        if (keywrodOverrideTable.TryGetValue(keywordId, out overAction))
        {
            keywordAciton.OverrideKeywordAction(overAction);
        }
        //키워드 액션을 추가
        AddAction(newKeyword, keywordAciton);
    }
    public void MoveAbleUpdate(bool isOn) 
    {
        isMoveAble = isOn;
        wireColorController.MoveAbleUpdate(isOn);
    }
    public void SetWireFrameColor(Color color) 
    {
        if (!originMat.HasProperty(WIRE_FRAME_COLOR_NAME))
        {
            return;
        }
        originMat.SetColor(WIRE_FRAME_COLOR_NAME, color);
    }
    public void ClearWireFrameColor() 
    {
        if (!originMat.HasProperty(WIRE_FRAME_COLOR_NAME))
        {
            return;
        }
        originMat.SetColor(WIRE_FRAME_COLOR_NAME, Managers.Keyword.GetColorByState(E_WIRE_COLOR_MODE.Default));
    }

    public void SetWireEmmColor(Color color)
    {
        if (!originMat.HasProperty(WIRE_FRAME_EMM_COLOR_NAME))
        {
            return;
        }
        originMat.SetColor(WIRE_FRAME_EMM_COLOR_NAME, color);
    }
    public void ClearWireEmmColor()
    {
        if (!originMat.HasProperty(WIRE_FRAME_EMM_COLOR_NAME))
        {
            return;
        }
        originMat.SetColor(WIRE_FRAME_EMM_COLOR_NAME, Color.clear);
    }
    private void InitColisionLayer() 
    {
        colisionCheckLayer = 1;
        foreach (var name in Enum.GetNames(typeof(Define.ColiiderMask)))
        {
            colisionCheckLayer += (1 << (LayerMask.NameToLayer(name)));
        }
    }
    public bool HasKeyword(string id) 
    {
        foreach(var item in currentRegisterKeyword) 
        {
            if(item.Key.KewordId == id) 
            {
                return true;
            }
        }
        return false;
    }
    #region Keyword_Control
    public bool ColisionCheckRotate(Vector3 vec)
    {
        var pos = col.transform.position;
        RaycastHit hit;
        var boxSize = Util.VectorMultipleScale(col.size/2,transform.lossyScale)* 0.99f;
        var rot = KeywordTransformFactor.rotation * Quaternion.Euler(vec);
#if UNITY_EDITOR
        ExtDebug.DrawBox(pos,boxSize,rot,Color.blue);
#endif
        var hits = Physics.OverlapBox(pos,boxSize,rot, colisionCheckLayer, QueryTriggerInteraction.Ignore);
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

        vec.x = math.abs(vec.x) < MINMUM_DIR_AMOUNT ? 0 : vec.x;
        vec.z = math.abs(vec.z) < MINMUM_DIR_AMOUNT ? 0 : vec.z;
        if (vec.magnitude == 0) 
        {
            return false;
        }
    
        var boxSize = Util.VectorMultipleScale(col.size / 2, transform.lossyScale);
        boxSize.y = boxSize.y * 0.99f;
        //boxSize.x = boxSize.x * 0.99f;
        //boxSize.z = boxSize.z * 0.99f;
#if UNITY_EDITOR
        ExtDebug.DrawBox(pos + vec, boxSize, KeywordTransformFactor.rotation, Color.blue);
        //ExtDebug.DrawBoxCastBox(pos , boxSize, KeywordTransformFactor.rotation, vec.normalized, vec.magnitude, Color.blue);
#endif
        var hits = Physics.OverlapBox(pos + vec, boxSize, KeywordTransformFactor.rotation, colisionCheckLayer, QueryTriggerInteraction.Ignore);
        //var castHits = Physics.BoxCastAll(pos , boxSize, vec.normalized, KeywordTransformFactor.rotation,vec.magnitude, colisionCheckLayer, QueryTriggerInteraction.Ignore);
        for (int i = 0; i< hits.Length; ++i) 
        {
            if(hits[i] != col) 
            {
                var castHits = Physics.BoxCastAll(pos, boxSize, vec.normalized, KeywordTransformFactor.rotation, vec.magnitude, colisionCheckLayer, QueryTriggerInteraction.Ignore);
                
                foreach (var cast in castHits)
                {
                    if (cast.collider == hits[i])
                    {
                        Debug.Log(cast.distance + " " + vec.magnitude);

                        if ((cast.distance - MINMUM_DIR_AMOUNT) > 0.001f)
                        {
                            var dis = cast.distance - MINMUM_DIR_AMOUNT;
                            KeywordTransformFactor.position += dis * vec.normalized;
                            return true;
                        }
                        return false;
                    }
                }

                //if ((castHits[i].distance - MINMUM_DIR_AMOUNT) > 0.001f)
                //{
                //    var dis = castHits[i].distance - MINMUM_DIR_AMOUNT;
                //    KeywordTransformFactor.position += dis * vec.normalized;
                //    return true;
                //}
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

        var boxSize = Util.VectorMultipleScale(col.size / 2,transform.lossyScale);
        boxSize *= 0.98f;
        boxSize.y = 0;
        var rayDis = maxHeight + col.bounds.size.y;
#if UNITY_EDITOR
        ExtDebug.DrawBoxCastBox(pos,boxSize,KeywordTransformFactor.rotation, Vector3.down,rayDis,Color.red);
#endif
        Physics.BoxCast(pos,boxSize,Vector3.down,out hit,KeywordTransformFactor.rotation,rayDis, colisionCheckLayer);
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
                var moveAble = ColisionCheckMove(upVec);
                MoveAbleUpdate(moveAble);
               
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
                MoveAbleUpdate(true);
                return true;
            }
           
        }
        KeywordTransformFactor.position += Vector3.down * speed;
        MoveAbleUpdate(true);
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
     
#if UNITY_EDITOR
        ExtDebug.DrawBox(pos + vec, boxSize, KeywordTransformFactor.rotation, Color.blue);
#endif
        var hits = Physics.OverlapBox(pos + vec, boxSize, KeywordTransformFactor.rotation, colisionCheckLayer, QueryTriggerInteraction.Ignore);

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


    public void SaveData(GameData gameData)
    {
        var debugZoneGuid = parentDebugZone.GuId;
        if (!gameData.keywordEntityDatas.ContainsKey(debugZoneGuid)) 
        {
            gameData.keywordEntityDatas.Add(debugZoneGuid, new Dictionary<string, KeywordEntityData>());
        }

        var data = new KeywordEntityData();
        data.isEnable = gameObject.activeSelf;
      
       
        if (!gameObject.activeSelf)
        {
            gameData.keywordEntityDatas[debugZoneGuid].AddOrUpdateValue(guId, data);
            return;
        }

        //data.isEnable = gameObject.activeSelf;
        data.pos = transform.position;
        data.rot = transform.rotation;
        data.scale = transform.localScale;
        
        for(int i =0; i < keywordFrames.Count; ++i) 
        {
            var frameData = new KeywordEntityData.KeywordFrameData();
            data.keywordFrameDatas.Add(frameData);
            if (keywordFrames[i].HasKeyword) 
            {
                frameData.isLock = keywordFrames[i].CurFrameInnerKeyword.IsLock;
                frameData.id = keywordFrames[i].CurFrameInnerKeyword.KewordId;
            }
        }
        gameData.keywordEntityDatas[debugZoneGuid].AddOrUpdateValue(guId, data);
    }
    public void LoadData(GameData gameData)
    {
        var debugZoneGuid = parentDebugZone.GuId;
        if (!gameData.keywordEntityDatas.ContainsKey(debugZoneGuid)) 
        {
            return;
        }
        if (!gameData.keywordEntityDatas[debugZoneGuid].ContainsKey(GuId))
        {
            return;
        }
        var data = gameData.keywordEntityDatas[debugZoneGuid][GuId];
        if (!data.isEnable)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = data.pos;
        transform.rotation = data.rot;
        transform.localScale = data.scale;
        var optionList = new List<CreateKeywordOption>();
        for (int i = 0; i < data.keywordFrameDatas.Count; ++i)
        {
            var option = new CreateKeywordOption();
            if (data.keywordFrameDatas[i].id != "")
            {
                option.isLock = data.keywordFrameDatas[i].isLock;
                option.keywordGo = Managers.Resource.Load<GameObject>($"Prefabs/UI/SubItem/KeywordPrefabs/{data.keywordFrameDatas[i].id}");
            }
            optionList.Add(option);
        }
        keywords = optionList.ToArray();
    }

}
