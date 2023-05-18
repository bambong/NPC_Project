using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class Extension
{
	public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(go);
	}

    //public static void BindEvent(this GameObject go,Action<PointerEventData> action,Define.UIEvent type = Define.UIEvent.Click)
    //{
    //	UI_Base.BindEvent(go,action,type);
    //}
    public static T RemoveRandom<T>(this List<T> list)
    {
        int index = Random.Range(0, list.Count);
        T removedItem = list[index];
        list.RemoveAt(index);
        return removedItem;
    }
    public static int GetRandomIndex<T>(this List<T> list)
    {
        int index = Random.Range(0, list.Count);
        return index;
    }
    public static bool IsValid(this GameObject go)
	{
		return go != null && go.activeSelf;
	}
}
