using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorStateData", menuName = "Scriptable Data/ColorStateData", order = 0)]
public class ColorStateData : ScriptableObject
{
    public List<ColorState> colorStates;
}

