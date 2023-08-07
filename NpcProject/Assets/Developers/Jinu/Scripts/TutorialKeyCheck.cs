using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialKeyCheck : MonoBehaviour
{
    [SerializeField]
    private KEY_TYPE key;
    [SerializeField]
    private UnityEvent talkEvent;

    public void Start()
    {
        if(Managers.Data.Progress == 5)
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
                talkEvent?.Invoke();
                yield break;
            }
            yield return null;
        }
    }
}
