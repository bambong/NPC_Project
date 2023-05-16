using MoreMountains.FeedbacksForThirdParty;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackOnceKeyword : KeywordController
{
    [SerializeField]
    private int damage = 1;
    public override void OnEnter(KeywordEntity entity)
    {
        if(entity is MonsterController)
        {
           var monster = entity as MonsterController;
            monster.GetDamaged();
            DestroyKeyword();
        }
    }

}
