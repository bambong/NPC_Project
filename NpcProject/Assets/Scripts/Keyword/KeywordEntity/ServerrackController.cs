using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerrackController : KeywordEntity
{
    private void Awake()
    {
        AddOverrideTable(typeof(RotateKeyword).ToString(),new KeywordAction(XrotateKeywordAction,KeywordActionType.OneShot));
    }
    private void XrotateKeywordAction(KeywordEntity entity)
    {
        Debug.Log("�������̵� ����");
    }
}
