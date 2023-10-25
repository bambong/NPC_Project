using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
//using UnityEditor.Searcher;
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
    private const float BOX_DIFF_FACTOR = 0.99f;
    private const float BOX_DIFF_FACTOR_FOR_FLOAT = 0.98f;
    private const float FLOAT_ARRIVE_DIS = 0.1f;
    private const float ATTACH_AMOUNT = 0.001f;
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
        StartCoroutine(CheckInitDebugMod());
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
           
            // Ű���尡 �̸� �����Ǿ� �ִ� �������� Ȯ��
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
    public virtual void AddAction(KeywordController controller,KeywordAction action) // 키워드 기능 추가
    {
        action.OnEnter.Invoke(this); // 키워드 추가시 액션  실행
        fixedUpdateAction += action.OnFixecUpdate; // FixedUpdate 액션 추가 
        updateAction += action.OnUpdate; // Update 액션 추가
        currentRegisterKeyword[controller] = action; // 현재 키워드를 등록 키워드 목록에 추가
    }

    public void RemoveAction(KeywordController registerkeyword , KeywordController newKeyword) // 등록된 키워드를 삭제시 실행하는 함수 
    {
        if(registerkeyword == null) // 삭제 시도 키워드가 Null 인지 체크 
        {
            return;
        }

        if(!currentRegisterKeyword.ContainsKey(registerkeyword)) // 등록되지않은 키워드 삭제 시도 체크
        {
            return;
        }

        for(int i = 0; i < keywordFrames.Count; ++i)  // 삭제 시도 키워드가 수신 오브젝트의 다른 프레임에 존재하는지 체크 (같은 수신 오브젝트 프레임내 에서 이동하는 경우)
        {
            if(keywordFrames[i].CurFrameInnerKeyword == registerkeyword) 
            {
                return;
            }
        }
        var action = currentRegisterKeyword[registerkeyword];

        fixedUpdateAction -= action.OnFixecUpdate; //현재 등록된 FixedUpdate 이벤트 삭제
        updateAction -= action.OnUpdate; //현재 등록된 Update 이벤트 삭제

        if (newKeyword == null || registerkeyword.KeywordId != newKeyword.KeywordId) // 똑같은 타입의 키워드 끼리 전환일 경우 Remove 이벤트를 발생시키지 않음
        {
            currentRegisterKeyword[registerkeyword]?.OnRemove(this); // 키워드 Remove 이벤트 호출
        }
        currentRegisterKeyword.Remove(registerkeyword); // 현재 등록된 키워드 목록에서 키워드 삭제
    }
    public void DecisionKeyword(KeywordFrameController keywordFrame , KeywordController newKeyword) // 키워드가 등록될 때 호출되는 함수 
    {
  
        if (newKeyword == null)  // 새로운 키워드가 없다면
        {
            keywordFrame.KeywordWorldSlot.UpdateUI(false); // 키워드 보유 표시 UI 업데이트 
            return;
        }
     
        keywordFrame.KeywordWorldSlot.UpdateUI(true);   // 키워드 보유 표시 UI 업데이트 

        if (currentRegisterKeyword.ContainsKey(newKeyword))
        {
            return;
        }

        var keywordId = newKeyword.KeywordId;
        var keywordAciton = new KeywordAction(newKeyword);
        
        KeywordAction overAction;
      
        if (keywrodOverrideTable.TryGetValue(keywordId, out overAction)) // 현재 키워드에 대해 오버라이드된 기능이 있는지 확인
        {
            keywordAciton.OverrideKeywordAction(overAction); // 오버라이드된 기능이 있다면 덮어씌운다.
        }

        AddAction(newKeyword, keywordAciton); // 키워드가 가지고 있는 기능을 추가
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
            if(item.Key.KeywordId == id) 
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
    public bool ColisionCheckMove(Vector3 vec) // 움직인 경우 True 아닌 경우 false 반환 
    {
        var pos = col.transform.position;

        vec.x = math.abs(vec.x) < MINMUM_DIR_AMOUNT ? 0 : vec.x; //최소 움직임 값보다 작다면 초기화
        vec.z = math.abs(vec.z) < MINMUM_DIR_AMOUNT ? 0 : vec.z; 
        if (vec.magnitude == 0)  // 움직임이 0 이라면 Return
        {
            return false;
        }
    
        var boxSize = Util.VectorMultipleScale(col.size / 2, transform.lossyScale);
        boxSize.y = boxSize.y * BOX_DIFF_FACTOR; // 과도한 검출을 피하기위해 Factor 값을 곱하여 줌  

#if UNITY_EDITOR
        ExtDebug.DrawBox(pos + vec, boxSize, KeywordTransformFactor.rotation, Color.blue); // Editor 상에 체크하는 박스를 그려줌
#endif
        var hits = Physics.OverlapBox(pos + vec, boxSize, KeywordTransformFactor.rotation, colisionCheckLayer, QueryTriggerInteraction.Ignore); // 장애물로 인식할 레이어를 따로 지정하고 해당 레이어에대해 Ray 체크함 
        for (int i = 0; i< hits.Length; ++i)  // 장애물이 있는 경우
        {
            if(hits[i] != col) 
            {
                var castHits = Physics.BoxCastAll(pos, boxSize, vec.normalized, KeywordTransformFactor.rotation, vec.magnitude, colisionCheckLayer, QueryTriggerInteraction.Ignore);
                
                foreach (var cast in castHits)
                {
                    if (cast.collider == hits[i])
                    {
                        Debug.Log(cast.distance + " " + vec.magnitude);

                        if ((cast.distance - MINMUM_DIR_AMOUNT) > ATTACH_AMOUNT) // 장애물까지 남은 거리가 붙어있다고 판정하는 거리보다 크다면   
                        {
                            var dis = cast.distance - MINMUM_DIR_AMOUNT;
                            KeywordTransformFactor.position += dis * vec.normalized; //장애물까지 남은 거리 만큼 붙도록 이동
                            return true;
                        }

                        return false;
                    }
                }

                return false;
            }
        }
        KeywordTransformFactor.position += vec; // 장애물이 없는 경우 바로 이동
        return true;
    }
    public bool FloatMove(float speed)
    {
        var pos = col.transform.position;
        RaycastHit hit;

        var boxSize = Util.VectorMultipleScale(col.size / 2,transform.lossyScale);
        boxSize *= BOX_DIFF_FACTOR_FOR_FLOAT; // 과도한 검출을 피하기위해 박스크기를 조정
        boxSize.y = 0; 
        var rayDis = maxHeight + col.bounds.size.y; 
#if UNITY_EDITOR
        ExtDebug.DrawBoxCastBox(pos,boxSize,KeywordTransformFactor.rotation, Vector3.down,rayDis,Color.red); // Editor 상에 박스를 그려줌
#endif
        Physics.BoxCast(pos,boxSize,Vector3.down,out hit,KeywordTransformFactor.rotation,rayDis, colisionCheckLayer);
        if (hit.collider != null)
        {
            var realDis = hit.distance - col.bounds.extents.y;
            if(realDis <= maxHeight)  // 위로 상승하는 움직임
            {
                var remainDis = maxHeight - realDis; // 남은 거리를 계산
                if(remainDis < FLOAT_ARRIVE_DIS) // 남은 거리가 도착으로 판정할 거리보다 작다면 Return 
                {
                    return true;
                }
                if(remainDis < speed) // 현재 움직일려는 거리가 남은거리보다 큰지 체크
                {
                    speed =  remainDis;
                }
                var upVec = Vector3.up * speed;
                var moveAble = ColisionCheckMove(upVec);
                MoveAbleUpdate(moveAble);
               
                return true;
            }
            else  // 아래로 내려오는 움직임
            {
                var remainDis = realDis - maxHeight;
                if (remainDis < FLOAT_ARRIVE_DIS)
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
        KeywordTransformFactor.position += Vector3.down * speed; // 바닥으로 레이가 닿지 않는다면 아래로 움직임
        MoveAbleUpdate(true);
        return false;
    }
    public bool ColisionCheckScale(Vector3 desireScale, GameObject dummyParent) 
    {
        var desireBoxSize = Util.VectorMultipleScale(col.size / 2, desireScale); // 변하고자하는 BoxScale
        var curBoxSize = Util.VectorMultipleScale(col.size / 2, transform.lossyScale); // 현재 BoxScale
        var boxScaleDiff = (desireBoxSize - curBoxSize); // BoxScale 차이

        Vector3 parentPos = Vector3.zero; 
        Vector3 rayBox = curBoxSize * BOX_DIFF_FACTOR; // 과도한 검출을 피하기 위한 Factor 값

        if (CheckAxis(transform.right * desireBoxSize.x, new Vector3(0, rayBox.y, rayBox.z), boxScaleDiff.x, curBoxSize.x, ref parentPos)
        || CheckAxis(transform.up * desireBoxSize.y, new Vector3(rayBox.x, 0, rayBox.z), boxScaleDiff.y, curBoxSize.y, ref parentPos)
        || CheckAxis(transform.forward * desireBoxSize.z, new Vector3(rayBox.x, rayBox.y, 0), boxScaleDiff.z, curBoxSize.z, ref parentPos)) // 박스의 X , Y , Z 축으로 커질 수 있는지 체크   
        {
            return false; // 한 축 이라도 Scale 이 불가능할 경우 균등 Scale 이기 때문에 불가능
        }
        // 피봇을 옮겨서 Scale 조정을 위해 Dummy 를 만들고 부모로 만듬
        dummyParent.transform.localScale = transform.lossyScale; 
        dummyParent.transform.rotation = transform.rotation;
        dummyParent.transform.position = transform.position + parentPos; // 매핑해 놓았던 피봇 Pos 를 넣어줌
        transform.SetParent(dummyParent.transform); // Dummy 를 부모로 만들고
        dummyParent.transform.localScale = desireScale;  // Scale 변환
        transform.SetParent(null); // Dummy 부모를 해제 
        return true;
    }
    private bool CheckAxis(Vector3 rayDir, Vector3 rayBox, float boxScaleDiff, float parentPosFactor, ref Vector3 parentPos)
    {
        // 축을 중심으로 한 뱡항으로라도 커질 수 있다면 False 반환
        // 어느 쪽도 장애물이 없다면 피봇 매핑을 하지않고 양쪽으로 균등하게 커짐 

        if (ColisionCheckBox(rayDir, rayBox)) // 한쪽 방향으로 장애물이 있을 경우
        {
            if (ColisionCheckBox(AddDis(-rayDir, boxScaleDiff), rayBox)) // 반대쪽 방향도 장애물이 있을 경우
            {
                return true; // 커질 수 없음
            }
            parentPos += rayDir.normalized * parentPosFactor; // 커질 수 있는 방향을 피봇에 매핑 
        }
        else if (ColisionCheckBox(-rayDir, rayBox))
        {
            if (ColisionCheckBox(AddDis(rayDir, boxScaleDiff), rayBox))
            {
                return true;
            }
            parentPos -= rayDir.normalized * parentPosFactor; // 커질 수 있는 방향을 피봇에 매핑 
        }
        return false; //커질 수 있는 방향이 한 곳 이라도 존재 
    }
    public bool ColisionCheckBox(Vector3 vec , Vector3 boxSize) 
    {
        var pos = col.transform.position;

        RaycastHit hit;
     
#if UNITY_EDITOR
        ExtDebug.DrawBox(pos + vec, boxSize, KeywordTransformFactor.rotation, Color.blue); // Editor 상에서 박스를 그려줌 
#endif
        var hits = Physics.OverlapBox(pos + vec, boxSize, KeywordTransformFactor.rotation, colisionCheckLayer, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i] != col) // 이 Colider 가 아닌 Colider 중 부딪히는게 하나라도 있다면 True
            {
                return true; 
            }
        }
        return false; // 부딪히는게 없다면 False 
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
                frameData.id = keywordFrames[i].CurFrameInnerKeyword.KeywordId;
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
