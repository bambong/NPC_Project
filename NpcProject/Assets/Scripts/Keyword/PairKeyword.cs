using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;



public class PairKeyword : KeywordController
{
    public static Dictionary<DebugZone, List<PairKeyword>> PairKeywords = new Dictionary<DebugZone,List<PairKeyword>>();

    [SerializeField]
    private LineRenderer lineRenderer;

    private E_WIRE_COLOR_MODE curPairMod;
    private bool isLineOn = false;
    private KeywordEntity lineEntity;
    public KeywordEntity MasterEntity { get; protected set; }

    private const string MAT_FLOW_PROPERTY_NAME = "_BoardTill";
    private const int FLOW_TILL_COUNT = 20;
    private static readonly string[] FLOW_KEYWORD_IDS = new string[]{ "AttachKeyword", "ApartKeyword",};
 
    private void Start()
    {
        Managers.Keyword.OnEnterDebugModEvent += LineEnable;
        Managers.Keyword.OnExitDebugModEvent += LineDisable;
    }
    
    public static bool IsAvailablePair(KeywordEntity entity ,out KeywordEntity otherEntity) 
    {
        PairKeyword pairKeyword = null;
        otherEntity = null;
        foreach (var keyword in entity.CurrentRegisterKeyword)
        {
            if (keyword.Key is PairKeyword)
            {
                pairKeyword = keyword.Key as PairKeyword;
                break;
            }
        }
        if (pairKeyword == null)
        {
            return false;
        }
        var pair = pairKeyword.GetOtherPair();
        if (pair == null)
        {
            return false;
        }
        otherEntity = pair.MasterEntity;

        if (otherEntity == null || pairKeyword.MasterEntity == null || otherEntity == pairKeyword.MasterEntity)
        {
            return false;
        }
        return true;
    }
    private bool IsAvailablePair(out KeywordEntity otherEntity)
    {
        var pair = GetOtherPair();
        otherEntity = null;
        if (pair == null) 
        {
            return false;
        }

        otherEntity = pair.MasterEntity;

        if (otherEntity == null || otherEntity == MasterEntity)
        {
            return false;
        }
        return true;
    }


    public PairKeyword GetOtherPair() 
    {
        for(int i = 0;i < PairKeywords[parentDebugZone].Count; ++i) 
        {
            if (PairKeywords[parentDebugZone][i] == this) 
            {
                continue;
            }
            return PairKeywords[parentDebugZone][i];
        }
        return null;
    }
    public override void SetDebugZone(DebugZone zone)
    {
        base.SetDebugZone(zone);
        if(!PairKeywords.ContainsKey(zone)) 
        {
            PairKeywords.Add(zone, new List<PairKeyword>());
        }
        PairKeywords[zone].Add(this);
    }

  

    private void OpenLineRender(KeywordEntity entity)
    {
        entity.WireColorController.Open();
        MasterEntity.WireColorController.Open();

        isLineOn = true;
        lineEntity = entity;
        lineRenderer.enabled = true;
    }
    private void CloseLineRender()
    {
        if (!isLineOn) 
        {
           return;
        }
        lineEntity.WireColorController.Close();
        MasterEntity.WireColorController.Close();

        isLineOn = false;
        lineRenderer.enabled = false;
        lineEntity = null;
    }
    private void LineEnable()
    {
        if (isLineOn)
        {
            lineRenderer.enabled = true;
        }
    }
    private void LineDisable()
    {
        lineRenderer.enabled = false;
    }
    public bool ChangeLineState(KeywordEntity first , KeywordEntity second) 
    {
       for(int i = 0; i < FLOW_KEYWORD_IDS.Length; ++i) 
       {
            if (first.HasKeyword(FLOW_KEYWORD_IDS[i])) 
            {
                lineRenderer.material.SetInt(MAT_FLOW_PROPERTY_NAME, FLOW_TILL_COUNT);
                switch (FLOW_KEYWORD_IDS[i]) 
                {
                    case "AttachKeyword":
                        LineSetPos(second, first);
                        return true;
                    case "ApartKeyword":
                        LineSetPos(first, second);
                        return true;
                }
            }
       }
       return false;
    }
 
    public override void OnEnter(KeywordEntity entity)
    {
        MasterEntity = entity;
        KeywordEntity otherEntity = null;
        if (IsAvailablePair(out otherEntity))
        {
            OpenLineRender(otherEntity);
            LinePosUpdate(MasterEntity, otherEntity);
        }
        else
        {
            CloseLineRender();
        }
    }
    public override void OnFixedUpdate(KeywordEntity entity)
    {
        if (!isLineOn)
        {
            return;
        }

        KeywordEntity otherEntity = null;
        if (IsAvailablePair(out otherEntity))
        {
            LinePosUpdate(MasterEntity, otherEntity);
        }
        else
        {
            CloseLineRender();
        }
    }
    private void LineSetPos(KeywordEntity first, KeywordEntity second) 
    {
        lineRenderer.SetPosition(0, first.transform.position);
        lineRenderer.SetPosition(1, second.transform.position);
    }
    private void LinePosUpdate(KeywordEntity master, KeywordEntity other) 
    {
        if(ChangeLineState(master, other)) 
        {
            return;        
        }
        if(ChangeLineState(other, master)) 
        {
            return;
        }
        lineRenderer.material.SetInt(MAT_FLOW_PROPERTY_NAME, 0);
        LineSetPos(master, other);
    }
    public override void OnRemove(KeywordEntity entity)
    {
        //if(entity != MasterEntity) 
        //{
        //    return;
        //}
        var other = GetOtherPair();
        if(other != null) 
        {
            other.CloseLineRender();
        }
        CloseLineRender();
        MasterEntity = null;
        lineRenderer.enabled=false;
    }
    
    private void OnDestroy()
    {
        Managers.Keyword.OnEnterDebugModEvent -= LineEnable;
        Managers.Keyword.OnExitDebugModEvent -= LineDisable;
        if (parentDebugZone == null) 
        {
            return;
        }
        PairKeywords[parentDebugZone].Remove(this);
        if(PairKeywords[parentDebugZone].Count == 0) 
        {
            PairKeywords.Remove(parentDebugZone);
        }
    }
}
