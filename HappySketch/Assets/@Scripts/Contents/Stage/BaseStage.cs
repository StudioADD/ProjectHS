using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public enum EStageType
{
    None = 0,
    SharkAvoidance, // 상어 피하기
    CollectingCandy, // 사탕 모으기
    CrossingBridge, // 다리 건니기

    Max
}

public abstract class BaseStage : InitBase
{
    [field: SerializeField, ReadOnly] protected Transform playerStartPoint;
    [field: SerializeField, ReadOnly] public EStageType StageType { get; protected set; }

    protected virtual void Reset()
    {
        playerStartPoint = Util.FindChild<Transform>(this.gameObject, "PlayerStartPoint", true);
        playerStartPoint ??= Util.Editor_InstantiateObject(this.transform).transform;
        playerStartPoint.gameObject.name = "PlayerStartPoint";
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public abstract void StartStage();
    public abstract void EndStage(ETeamType winnerTeam);
}
