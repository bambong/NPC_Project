using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Talk_ID",menuName = "Scriptable Event/TalkEvent",order = int.MaxValue)]
public class Talk : ScriptableObject
{
    public int Id;
    public List<Speak> speaks;
}
