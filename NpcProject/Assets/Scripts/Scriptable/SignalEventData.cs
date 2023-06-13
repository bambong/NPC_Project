using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SignalTalk
{
    public int eventId;
    [TextArea]
    public List<string> texts;
}


[CreateAssetMenu(fileName = "SignalData", menuName = "Scriptable Data/SignalData", order = 3)]
public class SignalEventData : ScriptableObject
{
    public List<SignalTalk> SignalTalks;
}
