using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CrossingBridgeStage : SingleStage
{
    [SerializeField, ReadOnly]
    protected FinishLineObject finishLineObject;

    protected override void Reset()
    {
        base.Reset();

        finishLineObject = Util.FindChild<FinishLineObject>(gameObject, "FinishLineObject", false);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo(Player player = null)
    {
        base.SetInfo(player);

        CrossingBridgeParam param = new CrossingBridgeParam(GetJumpTargetPos);
        player.SetStageInfo(param);
    }

    public override void ConnectEvents(Action<ETeamType> onEndGameCallBack)
    {
        if (finishLineObject != null)
        {
            finishLineObject.OnArriveFinishLine -= onEndGameCallBack;
            finishLineObject.OnArriveFinishLine += onEndGameCallBack;
        }
        else
            Debug.LogWarning($"FinishLineObject is Null!!");
    }

    public Vector3 GetJumpTargetPos(int id, bool isLeft)
    {


        return Vector3.zero;
    }
}
