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

    [SerializeField, ReadOnly] bool isLeftPlayerSaved = false;
    [SerializeField, ReadOnly] bool isRightPlayerSaved = false;

    [field: SerializeField, ReadOnly] CrossingBridgeParam leftStageParam = null;
    [field: SerializeField, ReadOnly] CrossingBridgeParam rightStageParam = null;

    // 팀에 따라 좌우로 벌어져있는 정도 ( Left = -f, Right = f )
    // 플랫폼 좌우 1,2 스타트, 세이브, 앤드 포인트는 Offset 값
    readonly float TeamOffSetPosX = 3.0f;
    readonly float offSetPosY = 1f;

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
        leftPlayer.ConnectCrossingBridgeStage(GetJumpTargetPos, GetSpawnPoint, OnUseGoggleItem, OnChangeTarget);
        rightPlayer.ConnectCrossingBridgeStage(GetJumpTargetPos, GetSpawnPoint, OnUseGoggleItem, OnChangeTarget);

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
        {
            leftPlayerPosNum = platformId;

            if (leftPlayerPosNum == (int)EPlatformType.SavePoint && isLeftPlayerSaved == false)
            {
                isLeftPlayerSaved = true;
                leftStageParam.isHaveGoggle = true;
                // 이벤트 뿌려주기
            }
        }
        else if(teamType == ETeamType.Right)
        {
            rightPlayerPosNum = platformId;

            if(rightPlayerPosNum == (int)EPlatformType.SavePoint && isRightPlayerSaved == false)
            {
                isRightPlayerSaved = true;
                rightStageParam.isHaveGoggle = true;
                // 이벤트 뿌려주기
            }
        }
    }

    public Vector3 GetJumpTargetPos(ETeamType teamType)
    {
        EDirection dir = (teamType == ETeamType.Left) ? leftStageParam.LookAtDir : rightStageParam.LookAtDir;
        return GetPlatformPosition(leftPlayerPosNum + 1, teamType, dir);
    }

    private Vector3 GetPlatformPosition(int jumpTargetPosNum, ETeamType teamType, EDirection dir)
    {
        if(jumpTargetPosNum == (int)EPlatformType.SavePoint)
        {
            return playerSavePoint.position + new Vector3(
                (dir == EDirection.Left) ? TeamOffSetPosX * -1f : TeamOffSetPosX, offSetPosY, 0);
        }
        else if (jumpTargetPosNum == (int)EPlatformType.EndPoint)
        {
            return playerEndPoint.position + new Vector3(
                (dir == EDirection.Left) ? TeamOffSetPosX * -1f : TeamOffSetPosX, offSetPosY, 0);
        }

        return platformGroupController.GetPlatformPos(jumpTargetPosNum, teamType, dir) + new Vector3(0, offSetPosY, 0);
    }

    public Vector3 GetSpawnPoint(ETeamType teamType)
    {
        if (teamType == ETeamType.Left)
        {
            if (leftPlayerPosNum >= (int)EPlatformType.SavePoint)
                return playerSavePoint.position + new Vector3(TeamOffSetPosX * -1, offSetPosY, 0);

        }
        else if (teamType == ETeamType.Right)
        {
            if (rightPlayerPosNum >= (int)EPlatformType.SavePoint)
                return playerSavePoint.position + new Vector3(TeamOffSetPosX, offSetPosY, 0);

        }
#if DEBUG
        Debug.LogError("없는 타입");
#endif
        return Vector3.zero;
    }

    public bool OnUseGoggleItem(ETeamType teamType)
    {
        if (teamType == ETeamType.Left && leftStageParam.isHaveGoggle)
        {
            leftStageParam.isHaveGoggle = false;

            return true;
        }
        else if (teamType == ETeamType.Right && rightStageParam.isHaveGoggle)
        {
            rightStageParam.isHaveGoggle = false;

            return true;
        }

        return false;
    }

    public void OnChangeTarget(ETeamType teamType, EDirection dir)
    {
        if (teamType == ETeamType.Left)
        {
            leftStageParam.LookAtDir = EDirection.Left;

        }
        else if (teamType == ETeamType.Right) 
        {
            rightStageParam.LookAtDir= EDirection.Right;

        }
    }

    public override Vector3 GetStartPoint(ETeamType teamType)
    {
        Vector3 offsetVec = new Vector3((teamType == ETeamType.Left) ? -TeamOffSetPosX : TeamOffSetPosX, 0, 0);
        return playerStartPoint.transform.position + offsetVec;
    }
}
