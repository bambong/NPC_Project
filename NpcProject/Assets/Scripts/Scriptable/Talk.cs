using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Talk_ID",menuName = "Scriptable Event/Talk/TalkEvent",order = 0)]
public class Talk : ScriptableObject
{
    public int Id;
    public List<Dialogue> speaks;
}

