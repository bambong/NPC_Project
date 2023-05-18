using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "SoundLists", menuName = "Scriptable Event/Sound/SoundLists", order = 0)]
public class SoundLists : ScriptableObject
{
    public List<SoundEvent> SoundEvents;
}
