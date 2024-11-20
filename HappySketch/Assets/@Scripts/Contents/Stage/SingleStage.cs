using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 하나의 스테이지로 두 명의 플레이어가 플레이하는 스테이지
/// </summary>
public abstract class SingleStage : BaseStage
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public abstract void ConnectEvents(Action<ETeamType> onEndGameCallBack);
    public abstract Vector3 GetStartPoint(ETeamType teamType);
}
