//using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    /// <summary>
    /// 씬 이름과 같아야 함
    /// </summary>
    public enum EScene
    {
        Unknown,
        TitleScene,
        ResultScene,
        GameScene,
    }

    /// <summary>
    /// 유니티 에디터의 레이어와 같아야 함
    /// </summary>
    public enum ELayer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        OverUI = 3,
        Water = 4,
        UI = 5,

        Player = 6,
        Monster = 7,
        Item = 8,
    }

    /// <summary>
    /// 유니티 에디터에 태그가 있어야 함
    /// </summary>
    public enum ETag
    {
        Untagged,
        
        MainCamera,
        Player,
        GameController,
        
        Monster,
        CollisionTrigger,
        Ground,
        Booster,
        Item,
        DestroyObj,
    }

    /// <summary>
    /// "Resources/Sounds/BGM/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    [Serializable]
    public enum EBgmSoundType
    {
        None = 0,

        // 전부 임시로 깔아둠
        SharkAvoidance,
        CollectingCandy,
        CrossingBridge,

        Max,
    }

    /// <summary>
    /// "Resources/Sounds/SFX/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    [Serializable]
    public enum ESfxSoundType
    {
        None = 0,

        EndStage,
        EndGame,

        // UI_OutGame
        UI_StartGame,

        // UI_InGamePublic
        

        // Stage1 - SharkAvoidanceStage
        UseBooster,

        // Stage2 - CollectingCandyStage


        // Stage3 - CrossingBridgeStage
        SavePoint,


        Max,
    }

    public enum ETeamType
    {
        Left,
        Right,
    }
    public enum EDirection
    {
        Left,
        Right,
    }

    public static readonly string STRING_EFFECT = "Effect";
    public static readonly string STRING_ATTACK = "Attack";
    public static readonly string STRING_Hit = "Hit";
}