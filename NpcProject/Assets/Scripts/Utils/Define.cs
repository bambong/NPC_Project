using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{ 
    public enum Scene
    {
        Unknown,
        Clear,
        Chapter_01,
        Chapter_01_Puzzle_01, 
        Chapter_01_Puzzle_02,
        Chapter_01_Puzzle_03,
        Serverroom
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
    public enum EFFECT
    {
        EnergeItemEffect,
        BombEffect,
        MonsterDeathEffect
    }

    public enum SOUND
    {
        AssignmentKeyword,
        AttackMonster,
        ClickKeyword,
        DeathMonster,
        DeathPlayer,
        EnlargementKeyword,
        ErrorEffectKeyword,
        FairKeyword,
        FindMonster,
        FloatingKeyword,
        HitMonster,
        HitPlayer,
        Item,
        MoveKeyword,
        RevolutionKeyword,
        RotatingKeyword,
        RunPlayer,
        ToBounceKeyword,
        ToDropKeyword,
        WalkPlayer
    }
}
