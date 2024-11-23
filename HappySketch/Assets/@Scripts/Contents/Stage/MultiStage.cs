using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 두 개의 스테이지에 각각의 플레이어가 플레이하는 스테이지
/// </summary>
public abstract class MultiStage : BaseStage
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        

        return true;
    }

    public abstract void ConnectEvents(Action<ETeamType> onEndGameCallBack);

    public Vector3 GetStartPoint() => playerStartPoint.position;
}
