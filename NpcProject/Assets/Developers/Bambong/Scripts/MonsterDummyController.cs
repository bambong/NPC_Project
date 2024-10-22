using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDummyController : KeywordEntity
{
    public override void ClearForPool()
    {
        Managers.Scene.CurrentScene.StartCoroutine(Test(transform.position));
        base.ClearForPool();
    }
    IEnumerator Test(Vector3 pos )
    {
        yield return new WaitForSeconds(3f);
        var a =  Managers.Resource.Instantiate("MonsterDummy");
        a.transform.position = pos;
    }
}
