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

    public ETeamType TeamType { get; protected set; }
    public EStageType StageType { get; protected set; }

    public event Action<StageParam> OnReceiveStageParam;

    protected StageParam stageParam = null; // 임시

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

    public virtual void SetInfo(Player player = null) { }
}
