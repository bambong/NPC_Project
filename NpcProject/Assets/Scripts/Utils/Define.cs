using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{ 
    public enum Scene
    {
        Unknown,
        Clear,
        Game
    }
    public enum WorldObject
    {
        Player
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
    public enum ColiiderMask 
    {
        //Player,
        Interaction
    }
}
