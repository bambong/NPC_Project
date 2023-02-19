using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatKeyword : KeywordController
{
    [SerializeField]
    private float speed = 10;
    private Dictionary<KeywordEntity,Coroutine> removeDic = new Dictionary<KeywordEntity,Coroutine>();
    public override void KeywordAction(KeywordEntity entity)
    {

        if(removeDic.ContainsKey(entity)) 
        {
            StopCoroutine(removeDic[entity]);
            removeDic.Remove(entity);
        }

        if(!entity.FloatMove(Vector3.up * Managers.Time.GetDeltaTime(TIME_TYPE.PLAYER)* speed)) 
        {
            MoveToGround(entity);
        }

    }
    public override void OnRemove(KeywordEntity entity)
    {
        removeDic.Add(entity,entity.StartCoroutine(FloatRemove(entity)));
    }
    private bool MoveToGround(KeywordEntity entity) 
    {
        return entity.ColisionCheckMove(Vector3.up * Managers.Time.GetDeltaTime(TIME_TYPE.PLAYER) * Physics.gravity.y);
    }
    private IEnumerator FloatRemove(KeywordEntity entity) 
    {
        bool isOn = true;
        while(isOn)
        {
            isOn = MoveToGround(entity);
           yield return null;
        }
        removeDic.Remove(entity);
    }

}
