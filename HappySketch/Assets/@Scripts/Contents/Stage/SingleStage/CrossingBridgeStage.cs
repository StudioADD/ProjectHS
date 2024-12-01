using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using CrossingBridge;

public class CrossingBridgeStage : SingleStage
{
    [SerializeField, ReadOnly] PlatformGroupController platformGroupController;

    [field: SerializeField, ReadOnly] protected Transform playerSavePoint;
    [field: SerializeField, ReadOnly] protected Transform playerEndPoint;

    [SerializeField, ReadOnly] int leftPlayerPosNum = 0;
    [SerializeField, ReadOnly] int rightPlayerPosNum = 0;

    // 팀에 따라 좌우로 벌어져있는 정도 ( Left = -f, Right = f )
    // 플랫폼 좌우 1,2 스타트, 세이브, 앤드 포인트는 Offset 값
    readonly float TeamOffSetPosX = 3.0f;

    Action<ETeamType> onEndGameCallBack;

    protected override void Reset()
    {
        base.Reset();

        playerEndPoint = Util.FindChild<Transform>(this.gameObject, "PlayerEndPoint", true);
        playerSavePoint = Util.FindChild<Transform>(this.gameObject, "PlayerSavePoint", true);
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

        platformGroupController.SetInfo(OnLandPlayerCallBack);
    }

    public override void StartStage()
    {
        base.StartStage();


    }

    public override void ConnectEvents(Action<ETeamType> onEndGameCallBack)
    {
        // leftPlayer.ConnectCrossingBridgeStage(GetJumpTargetPos, GetSpawnPoint, OnUseGoggleItem, OnChangeTarget);
        // rightPlayer.ConnectCrossingBridgeStage(GetJumpTargetPos, GetSpawnPoint, OnUseGoggleItem, OnChangeTarget);

        this.onEndGameCallBack = onEndGameCallBack;
    }

    public void OnLandPlayerCallBack(int platformId, ETeamType teamType)
    {
        if(platformId == (int)EPlatformType.EndPoint)
        {
            onEndGameCallBack?.Invoke(teamType);
            return;
        }

        if(teamType == ETeamType.Left)
            leftPlayerPosNum = platformId;
        else if(teamType == ETeamType.Right)
            rightPlayerPosNum = platformId;
    }

    public Vector3 GetJumpTargetPos(ETeamType teamType, EDirection dir = EDirection.Left)
    {
        return GetPlatformPosition(leftPlayerPosNum + 1, teamType, dir);
    }

    private Vector3 GetPlatformPosition(int jumpTargetPosNum, ETeamType teamType, EDirection dir)
    {
        if(jumpTargetPosNum == (int)EPlatformType.SavePoint)
        {
            return playerSavePoint.position + new Vector3(TeamOffSetPosX * -1, 0, 0);
        }
        else if (jumpTargetPosNum == (int)EPlatformType.EndPoint)
        {
            return playerEndPoint.position + new Vector3(TeamOffSetPosX, 0, 0);
        }

        return platformGroupController.GetPlatformPos(jumpTargetPosNum, teamType, dir);
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
