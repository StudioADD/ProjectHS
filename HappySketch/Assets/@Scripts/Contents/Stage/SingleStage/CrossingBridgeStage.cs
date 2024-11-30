using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using CrossingBridge;

public class CrossingBridgeStage : SingleStage
{
    [SerializeField, ReadOnly] PlatformGroupController platformGroupController;

    // 팀에 따라 좌우로 벌어져있는 정도 ( Left = -f, Right = f )
    // 플랫폼 좌우 1,2 스타트, 세이브, 앤드 포인트는 Offset 값
    readonly float TeamOffSetPosX = 5.0f;

    [SerializeField, ReadOnly]
    protected FinishLineObject finishLineObject;

    [SerializeField, ReadOnly] int leftPlayerPosNum = 0;
    [SerializeField, ReadOnly] int rightPlayerPosNum = 0;

    protected override void Reset()
    {
        base.Reset();

        finishLineObject = Util.FindChild<FinishLineObject>(gameObject, "FinishLineObject", false);
        platformGroupController = Util.FindChild<PlatformGroupController>(gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.CrossingBridge;

        return true;
    }

    public override void SetInfo(Player leftPlayer, Player rightPlayer)
    {
        base.SetInfo(leftPlayer, rightPlayer);


    }

    public override void StartStage()
    {
        base.StartStage();


    }

    public override void ConnectEvents(Action<ETeamType> onEndGameCallBack)
    {
        leftPlayer.ConnectCrossingBridgeStage(GetJumpTargetPos, GetSpawnPoint, OnUseGoggleItem, OnChangeTarget);
        rightPlayer.ConnectCrossingBridgeStage(GetJumpTargetPos, GetSpawnPoint, OnUseGoggleItem, OnChangeTarget);

        if (finishLineObject != null)
        {
            finishLineObject.OnArriveFinishLine -= onEndGameCallBack;
            finishLineObject.OnArriveFinishLine += onEndGameCallBack;
        }
        else
            Debug.LogWarning($"FinishLineObject is Null!!");
    }

    public Vector3 GetJumpTargetPos(ETeamType teamType)
    {
        if (teamType == ETeamType.Left)
            leftPlayerPosNum++;
        else
            rightPlayerPosNum++;

        return Vector3.zero;
    }

    public Vector3 GetSpawnPoint(ETeamType teamType)
    {


        return Vector3.zero;
    }

    public bool OnUseGoggleItem(ETeamType teamType)
    {

        return false;
    }

    public void OnChangeTarget(ETeamType teamType, EDirection dir)
    {

    }

    public override Vector3 GetStartPoint(ETeamType teamType)
    {
        Vector3 offsetVec = new Vector3((teamType == ETeamType.Left) ? -TeamOffSetPosX : TeamOffSetPosX, 0, 0);
        return playerStartPoint.transform.position + offsetVec;
    }
}
