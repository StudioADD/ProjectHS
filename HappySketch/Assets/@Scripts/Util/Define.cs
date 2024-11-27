//using System.Collections;
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

    public enum ETeamType
    {
        Left,
        Right,
    }
}