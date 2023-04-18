using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;



public class PairKeyword : KeywordController
{
    public static Dictionary<DebugZone, List<PairKeyword>> PairKeywords = new Dictionary<DebugZone,List<PairKeyword>>();

    [SerializeField]
    private LineRenderer lineRenderer;

    private E_PAIRCOLOR_MODE curPairMod;
    private bool isLineOn = false;
    private KeywordEntity lineEntity;
    public KeywordEntity MasterEntity { get; protected set; }


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

  

    private void OpenLineRender(KeywordEntity entity)
    {
        entity.WireColorController.Open();
        MasterEntity.WireColorController.Open();
        //entity.MRenderer.sharedMaterial = pairEffectMat;
        //MasterEntity.MRenderer.sharedMaterial = pairEffectMat;

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

       // var originMat = MasterEntity.OriginMat;
        lineEntity.WireColorController.Close();
        MasterEntity.WireColorController.Close();
        //lineEntity.MRenderer.sharedMaterial = originMat;
       // MasterEntity.MRenderer.sharedMaterial = originMat;
        isLineOn = false;
        lineRenderer.enabled = false;
        lineEntity = null;
    }
    public void ChangeLineState(E_PAIRCOLOR_MODE mod) 
    {
        if(curPairMod == mod) 
        {
            return;
        }
        curPairMod = mod;
        //lineRenderer.material.color = 


    
    }
    public void ClearLineState() 
    {
        ChangeLineState(E_PAIRCOLOR_MODE.Default);
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
        var other = GetOtherPair();
        other.CloseLineRender();
        CloseLineRender();
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
