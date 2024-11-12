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
    [SerializeField, ReadOnly]
    protected FinishLineObject finishLineObject;

    protected virtual void Reset()
    {
        finishLineObject = Util.FindChild<FinishLineObject>(gameObject, "FinishLineObject", false);
    }

    public EStageType StageType { get; protected set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo() { }

    public void ConnectEvents(Action<ETeamType> onArriveFinishLine)
    {
        finishLineObject.OnArriveFinishLine -= onArriveFinishLine;
        finishLineObject.OnArriveFinishLine += onArriveFinishLine;
    }
}
