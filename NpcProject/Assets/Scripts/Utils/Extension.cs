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
    //public static List<T> RandomShupple<T>(this List<T> list)
    //{
    //    int index = Random.Range(0, list.Count);
    //    return index;
    //}
    public static void Shuffle<T>(this IList<T> list)
    {
        var rgn = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rgn.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static bool IsValid(this GameObject go)
	{
		return go != null && go.activeSelf;
	}
    public static void AddOrUpdateValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            // 키가 이미 존재하는 경우 값 덮어쓰기
            dictionary[key] = value;
        }
        else
        {
            // 키가 존재하지 않는 경우 새로운 키와 값을 추가
            dictionary.Add(key, value);
        }
    }
}
