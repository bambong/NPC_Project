using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyKeyword : KeywordController
{
    public override void OnEnter(KeywordEntity entity)
    {
        if(entity.IsDestroy)
        {
            Managers.Effect.PlayEffect(Define.EFFECT.MonsterDeathEffectBig, entity.transform);
            Managers.Sound.PlaySFX(Define.SOUND.ErrorEffectKeyword);
            entity.DestroyKeywordEntity();
         }
    }
    public override void OnUpdate(KeywordEntity entity)
    {
        if (entity.IsDestroy)
        {
            Managers.Effect.PlayEffect(Define.EFFECT.MonsterDeathEffectBig, entity.transform);
            Managers.Sound.PlaySFX(Define.SOUND.ErrorEffectKeyword);
            entity.DestroyKeywordEntity(); 
         }
    }
    public override void OnRemove(KeywordEntity entity)
    {
        
    }
}
