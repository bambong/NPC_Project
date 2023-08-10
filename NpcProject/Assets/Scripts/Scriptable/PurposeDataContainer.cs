using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PurposeDataContainer", menuName = "Scriptable Data/PurposeData", order = 3)]
public class PurposeDataContainer : ScriptableObject
{
    public List<PurposeData> progressPurposes;
}

