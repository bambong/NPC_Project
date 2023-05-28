using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{ 
    public enum Scene
    {
        Unknown,
        DataPuzzle,
        Chapter_01_Office_Slave,
        Chapter_01_Serverroom,
        Chapter_01_Puzzle01, 
        Chapter_01_Puzzle02,
        Chapter_01_Puzzle03,
        Chapter_01_Puzzle04,
        Chapter_01_Puzzle05,
        Chapter_01_Puzzle06,
        Chapter_01_Puzzle07,
        Chapter_01_Puzzle08,
        Chapter_01_Puzzle09,
        Chapter_01_Puzzle10
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
        PairKeyword,
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
        WalkPlayer,
        OpenDoor,
        NextChapter,
        DebugModeEnter,
        DebugModeExit,
        DataPuzzleSuccess,
        DataPuzzleFail,
        DataPuzzleGood,
        DataPuzzleBad,
        DataPuzzleButtonHover,
        ResetButton,
        DataPuzzleEnter,
        Sign,
        ClickButton
    }
}