using CrossingBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;

public abstract class BasePlatformGroup : InitBase
{
    [SerializeField, ReadOnly] protected int platformId;
    [SerializeField, ReadOnly] protected EPlatformType platformType = EPlatformType.Normal;

    protected Action<int, ETeamType> onLandPlayer = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        return true;
    }

    public virtual void SetInfo(Action<int, ETeamType> onLandPlayer, int platformId)
    {
        this.onLandPlayer = onLandPlayer;
        this.platformId = platformId;
    }

    public abstract Vector3 IsLandablePosition();

    public abstract Vector3 GetPlatformPosition(ETeamType teamType, EDirection jumpDir = EDirection.Left);
    
    public void OnLandPlayerCallBack(ETeamType teamType)
    {
        onLandPlayer?.Invoke(platformId, teamType);
    }
}
