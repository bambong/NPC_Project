using MoreMountains.FeedbacksForThirdParty;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackKeyword : KeywordController
{
    [SerializeField]
    private Material bombMat;
    [SerializeField]
    private Material bombSpriteMat;
    private readonly float BOMB_TIME = 3f;
    public override void OnEnter(KeywordEntity entity)
    {
        if( CurFrame is KeywordFrameController) 
        {
            var frame =  CurFrame as KeywordFrameController;
            frame.SetLockFrame(true);
            StartCoroutine(CountAttack(entity));
        }
    }
    public IEnumerator CountAttack(KeywordEntity entity)
    {
        var render = entity.GetComponent<Renderer>();
        var originMat = render.material;
        float curTime = 0;
        float blinkFactorTime = BOMB_TIME/6;
        float curBlinkTime = 0;
        float curNormalTime = 0;
        bool blinkOn = true;
        var effectMat = render is SpriteRenderer ? bombSpriteMat : bombMat;
        render.material = effectMat;
       

        while(curTime < BOMB_TIME) 
        {
            var delTime = Managers.Time.GetDeltaTime(TIME_TYPE.NONE_PLAYER);
            curTime += delTime;

            if (blinkOn) 
            {
                curBlinkTime += delTime;

                if (curBlinkTime >= (blinkFactorTime * 0.4)) 
                {
                    blinkOn = false;
                    curBlinkTime = 0;
                    render.material = originMat;
                    blinkFactorTime = 0.1f + ((BOMB_TIME - curTime)/10) ;
                }
            }
            else 
            {
                curNormalTime += delTime;
                if( curNormalTime >= blinkFactorTime) 
                {
                    blinkOn = true;
                    curNormalTime = 0;
                    render.material = effectMat;
                }
            }
            yield return null;
        }
        render.material = originMat;
        Managers.Effect.PlayEffect(Define.EFFECT.BombEffect,entity.transform);
        entity.DestroyKeywordEntity();
    }
}
