using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairKeyword : KeywordController
{
    public static Dictionary<DebugZone, List<PairKeyword>> PairKeywords = new Dictionary<DebugZone,List<PairKeyword>>();

    public KeywordEntity MasterEntity { get; protected set; }
    [SerializeField]
    private LineRenderer lineRenderer;
    private bool isLineOn = false;
    private KeywordEntity lineEntity;


    private void Start()
    {
        Managers.Keyword.OnEnterDebugModEvent += () => 
        {
            if (isLineOn) 
            {
                lineRenderer.enabled = true;
            }
        };
        Managers.Keyword.OnExitDebugModEvent += () => 
        {
            lineRenderer.enabled = false;
        };
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
        otherEntity = pairKeyword.GetOtherPair().MasterEntity;

        if (otherEntity == null || pairKeyword.MasterEntity == null || otherEntity == pairKeyword.MasterEntity)
        {
            return false;
        }
        return true;
    }
    private bool IsAvailablePair(out KeywordEntity otherEntity)
    {

        otherEntity = GetOtherPair().MasterEntity;

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

    public override void OnEnter(KeywordEntity entity)
    {
        MasterEntity = entity;

        KeywordEntity otherEntity = null;
        if (IsAvailablePair(out otherEntity)) 
        {
            OpenLineRender(otherEntity);
            lineRenderer.SetPosition(0, MasterEntity.transform.position);
            lineRenderer.SetPosition(1, MasterEntity.transform.position);
        }
        else 
        {
            CloseLineRender();
        }
    }

    private void OpenLineRender(KeywordEntity entity)
    {
        isLineOn = true;
        lineEntity = entity;
        lineRenderer.enabled = true;
    }
    private void CloseLineRender()
    {
        isLineOn = false;
        lineRenderer.enabled = false;
        lineEntity = null;
    }
    public override void OnFixedUpdate(KeywordEntity entity)
    {
        KeywordEntity otherEntity = null;
        if (IsAvailablePair(entity, out otherEntity))
        {
            if (lineEntity != otherEntity) 
            {
                CloseLineRender();
                return;
            }
            //lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, MasterEntity.transform.position);
            lineRenderer.SetPosition(1,otherEntity.transform.position);
        }
        else
        {
            CloseLineRender();
        }
    }

    public override void OnRemove(KeywordEntity entity)
    {
        //if(entity != MasterEntity) 
        //{
        //    return;
        //}
        MasterEntity = null;
        lineRenderer.enabled=false;
    }
    
    private void OnDestroy()
    {
        if(parentDebugZone == null) 
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
