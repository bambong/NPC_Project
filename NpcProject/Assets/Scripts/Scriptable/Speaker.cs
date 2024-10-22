using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speaker",menuName = "Scriptable Event/Talk/Speaker",order = 0)]
public class Speaker : ScriptableObject
{
    public int Id;
    public string charName;
    public Sprite sprite;
}
