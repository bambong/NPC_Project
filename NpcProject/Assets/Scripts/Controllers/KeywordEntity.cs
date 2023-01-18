using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordEntity : MonoBehaviour
{
    [SerializeField]
    private int keywordSlot = 1;
   
    private Dictionary<string, Action<KeywordEntity>> keywrodOverrideTable = new Dictionary<string, Action<KeywordEntity>>();
    private List<KeywordFrameController> keywordSlotUI = new List<KeywordFrameController>();
    private List<KeywordWorldSlotUIController> keywordSlotWorldUI = new List<KeywordWorldSlotUIController>();

    private Action<KeywordEntity> updateAction = null;
    private Collider col;

    private void Start()
    {
        Managers.Keyword.AddSceneEntity(this);

        CreateKeywordFrame();
        CreateKeywordWorldSlotUI();

        col = Util.GetOrAddComponent<Collider>(gameObject);
    }

    private void CreateKeywordFrame() 
    {
        var slot = Managers.UI.MakeSubItem<KeywordFrameController>(Managers.Keyword.PlayerKeywordPanel.transform, "KeywordSlotUI");
        keywordSlotUI.Add(slot);
        slot.SetScale(Vector3.one);
    }
    private void CreateKeywordWorldSlotUI() 
    {
        keywordSlotWorldUI.Add(Managers.UI.MakeWorldSpaceUI<KeywordWorldSlotUIController>(transform, "KeywordSlotWorldSpace"));
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
        foreach(var slot in keywordSlotWorldUI) 
        {
            slot.Close();
        }
    }
    public void OpenWorldSlotUI() 
    {
        foreach (var slot in keywordSlotWorldUI)
        {
            slot.Open(transform);
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

    public void AddAction(Action<KeywordEntity> action) 
    {
        updateAction -= action;
        updateAction += action;
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
            AddAction(keywordSlotUI[i].KeywordController.KeywordUpdateAction);
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
