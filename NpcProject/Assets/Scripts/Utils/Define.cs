using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Define 
{ 
    public enum Scene
    {
        Unknown,
        DataPuzzle,
        Chapter_01_Office,
        Chapter_01_Serverroom_Corridor,
        Chapter_01_Puzzle01, 
        Chapter_01_Puzzle02,
        Chapter_01_Puzzle03,
        Chapter_01_Puzzle04,
        Chapter_01_Puzzle05,
        Chapter_01_Puzzle06,
        Chapter_01_Puzzle07,
        Chapter_01_Puzzle08,
        Chapter_01_Puzzle09,
        Chapter_01_Puzzle10,
        Chapter_01_Puzzle11,
        Chapter_01_Puzzle12,
        Chapter_01_Puzzle13,
        Chapter_02_Forest,
        TestLogo
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

    public enum LaserColor
    {
        Blue,
        Green,
        Purple,
        Red,
        Yellow
    }

    [Flags]
    public enum LaserLayer
    {
        Default = 1 << 0,
        TransparentFX = 1 << 1,
        IgnoreRaycast = 1 << 2,
        Player = 1 << 3,
        Water = 1 << 4,
        UI = 1 << 5,
        keywordFrame = 1 << 6,
        Cam = 1 << 7,
        Interaction = 1 << 8,
        Puzzle = 1 << 9,
        InteractionDetector = 1 << 10,
        Wall = 1 << 11,
        EventTrigger = 1 << 12,
        Slope = 1 << 13,
        PlayerDeath = 1 << 15,
        DontPlayerShadow = 1 << 16
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
        ButtonHover,
        ResetButton,
        DataPuzzleEnter,
        Sign,
        ClickButton,
        Door,
        TextSound,
        DataPuzzleDigital,
        DataPuzzleButtonHover,
        StartTextSound,
        DataPuzzleClear
    }
}