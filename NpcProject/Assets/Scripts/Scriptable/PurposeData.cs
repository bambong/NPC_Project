using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurposeData", menuName = "Scriptable Data/PurposeData", order = 3)]
public class PurposeData : ScriptableObject
{
    [TextArea]
    public List<string> progressPurposes;
}

