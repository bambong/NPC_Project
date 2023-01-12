using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordEntity : MonoBehaviour
{
    [SerializeField]
    private string entityId;
    [SerializeField]
    private int keywordSlot = 1;

    private Action<KeywordEntity> updateAction = null;
    private KeywordFrameController keywordSlotUI;
    private KeywordSlotUIWorldSpaceController keywordSlotWorldUI;
    private Collider col;

    private void Start()
    {
        Managers.Keyword.AddSceneEntity(this);

        keywordSlotUI = Managers.UI.MakeSubItem<KeywordFrameController>(Managers.Keyword.PlayerKeywordPanel.transform,"KeywordSlotUI");
        keywordSlotUI.SetScale(Vector3.one);

        keywordSlotWorldUI = Managers.UI.MakeWorldSpaceUI<KeywordSlotUIWorldSpaceController>(transform,"KeywordSlotWorldSpace");
        col = Util.GetOrAddComponent<Collider>(gameObject);
    }

    public virtual void EnterDebugMod()
    {
        OpenWorldSlotUI();
    }
    public virtual void ExitDebugMod() 
    {
        CloseWorldSlotUI();
    }

    public void CloseWorldSlotUI() => keywordSlotWorldUI.Close();
    public void OpenWorldSlotUI() => keywordSlotWorldUI.Open(transform);

    public void OpenKeywordSlot() 
    {
        keywordSlotUI.Open();
    }
    public void CloseKeywordSlot()
    { 
        keywordSlotUI.Close();
    }
    public void AddAction(Action<KeywordEntity> action) 
    {
        updateAction -= action;
        updateAction += action;
    }
    public void DecisionKeyword()
    {
        ClearAction();
        if(keywordSlotUI.KeywordController == null) 
        {
            keywordSlotWorldUI.ResetSlotUI();
            return;
        }

        keywordSlotWorldUI.SetSlotUI(keywordSlotUI.KeywordController.Image.color,keywordSlotUI.KeywordController.KeywordText.text);
        AddAction(keywordSlotUI.KeywordController.KeywordUpdateAction);
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
