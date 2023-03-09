using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatKeyword : KeywordController
{
    [SerializeField]
    private float speed = 10;
    public override void KeywordAction(KeywordEntity entity)
    {
        entity.SetGravity(false);
        entity.SetKinematic(true);

        entity.FloatMove(Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER) * speed);
    

    }
    public override void OnRemove(KeywordEntity entity)
    {
        entity.SetGravity(true);
        entity.SetKinematic(false);
     
    }

}
