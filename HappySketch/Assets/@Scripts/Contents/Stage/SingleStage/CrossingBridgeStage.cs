using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using CrossingBridge;

public class CrossingBridgeStage : SingleStage
{
    [SerializeField, ReadOnly] PlatformGroup platformGroup;

    // 팀에 따라 좌우로 벌어져있는 정도 ( Left = -f, Right = f )
    // 플랫폼 좌우 1,2 스타트, 세이브, 앤드 포인트는 Offset 값
    readonly float TeamOffSetPosX = 5.0f;

    [SerializeField, ReadOnly]
    protected FinishLineObject finishLineObject;

    [SerializeField, ReadOnly] int currLeftPlayerId = 0;
    [SerializeField, ReadOnly] int currRightPlayerId = 0;

    [SerializeField, ReadOnly] bool isLeftSavePoint = false;
    [SerializeField, ReadOnly] bool isRightSavePoint = false;

    protected override void Reset()
    {
        base.Reset();

        finishLineObject = Util.FindChild<FinishLineObject>(gameObject, "FinishLineObject", false);
        platformGroup = Util.FindChild<PlatformGroup>(gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.CrossingBridge;

        return true;
    }

    public override void SetInfo(Player player = null)
    {
        base.SetInfo(player);

        /*
        CrossingBridgeParam param = new CrossingBridgeParam(GetJumpTargetPos);
        player.SetStageInfo(param);
        */
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

    public override Vector3 GetStartPoint(ETeamType teamType)
    {
        Vector3 offsetVec = new Vector3((teamType == ETeamType.Left) ? -TeamOffSetPosX : TeamOffSetPosX, 0, 0);
        return playerStartPoint.transform.position + offsetVec;
    }
}
