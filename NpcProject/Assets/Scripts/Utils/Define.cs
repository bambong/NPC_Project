using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{ 
    public enum Scene
    {
        Unknown,
        Clear,
        DataPuzzle,
        Chapter_01,
        Chapter_01_Puzzle_01, 
        Chapter_01_Puzzle_02,
        Chapter_01_Puzzle_03,
        Chapter_01_Puzzle_04,
        Chapter_01_Tutorial_01,
        Chapter_01_Tutorial_02,
        Chapter_01_Tutorial_03,
        Chapter_01_Tutorial_04,
        Serverroom
    }
    public enum EventIdType
    {
        DataPuzzle = 90,
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
        Interaction,
        Slope
    }
    public enum EFFECT
    {
        EnergeItemEffect,
        BombEffect,
        MonsterDeathEffect
    }
    public enum BGM
    {
        Start,
        ReStart,
        Stop,
        Pause
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