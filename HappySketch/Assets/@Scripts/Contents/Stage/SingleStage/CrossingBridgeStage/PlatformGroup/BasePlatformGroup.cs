using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;

public abstract class BasePlatformGroup : InitBase
{
    protected Action<int, ETeamType, EDirection> onLandPlayer = null;

    [SerializeField, ReadOnly] protected int platformId;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(Action<int, ETeamType, EDirection> onLandPlayer, int platformId)
    {
        this.onLandPlayer = onLandPlayer;
        this.platformId = platformId;
    }

    public void OnLandPlayerCallBack(ETeamType teamType, EDirection dir)
    {
        onLandPlayer?.Invoke(platformId, teamType, dir);
    }
}
