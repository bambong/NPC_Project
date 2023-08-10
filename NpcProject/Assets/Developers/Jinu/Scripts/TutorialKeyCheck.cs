using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialKeyCheck : GuIdBehaviour
{
    [SerializeField]
    private KEY_TYPE key;
    [SerializeField]
    private GuIdBehaviour preEvent;
    [SerializeField]
    private UnityEvent talkEvent;

    protected override void Start()
    {
        base.Start();

        if(Managers.Data.IsClearEvent(preEvent.GuId) && !Managers.Data.IsClearEvent(guId))
        {
            CheckKey();
        }
    }

    public void CheckKey()
    {
        StartCoroutine(Check(key));
    }

    IEnumerator Check(KEY_TYPE key)
    {
        while(true)
        {
            if (Input.GetKeyDown(Managers.Game.Key.ReturnKey(key)))
            {
                Managers.Data.ClearEvent(guId);
                talkEvent?.Invoke();
                yield break;
            }
            yield return null;
        }
    }
}
