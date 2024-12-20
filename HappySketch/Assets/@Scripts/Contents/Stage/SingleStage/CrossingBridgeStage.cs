using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using CrossingBridge;

public class CrossingBridgeStage : SingleStage
{
    [field: SerializeField, ReadOnly]
    List<(ParticleEffectObject, int)> GoggleItemEffectList = new List<(ParticleEffectObject, int)>();

    [SerializeField, ReadOnly] PlatformGroupController platformGroupController;

    [field: SerializeField, ReadOnly] protected Transform playerSavePoint;
    [field: SerializeField, ReadOnly] protected Transform playerEndPoint;

    [SerializeField, ReadOnly] int leftPlayerPosNum = 0;
    [SerializeField, ReadOnly] int rightPlayerPosNum = 0;  

    [SerializeField, ReadOnly] bool isLeftPlayerSaved = false;
    [SerializeField, ReadOnly] bool isRightPlayerSaved = false;

    [field: SerializeField, ReadOnly] CrossingBridgeParam leftStageParam = null;
    [field: SerializeField, ReadOnly] CrossingBridgeParam rightStageParam = null;

    readonly float offSetPosX = 3f;
    readonly float offSetPosY = 1f;

    Action<ETeamType> onEndGameCallBack = null;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

        playerEndPoint = Util.FindChild<Transform>(this.gameObject, "PlayerEndPoint", true);
        playerSavePoint = Util.FindChild<Transform>(this.gameObject, "PlayerSavePoint", true);
        platformGroupController = Util.FindChild<PlatformGroupController>(gameObject);
    }
#endif

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

    public override void EndStage(ETeamType winnerTeam)
    {
        base.EndStage(winnerTeam);
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

        if (platformId == (int)EPlatformType.StartPoint)
        {
            if(teamType == ETeamType.Left)
                leftPlayerPosNum = 0;
            else if (teamType == ETeamType.Right)
                rightPlayerPosNum = 0;
            return;
        }

        for(int i = 0; i < GoggleItemEffectList.Count; i++)
        {
            if (GoggleItemEffectList[i].Item2 == platformId)
            {
                Managers.Resource.Destroy(GoggleItemEffectList[i].Item1.gameObject);
                GoggleItemEffectList.RemoveAt(i);
            }
        }

        if (teamType == ETeamType.Left)
        {
            leftPlayerPosNum = platformId;

            if (leftPlayerPosNum == (int)EPlatformType.SavePoint && isLeftPlayerSaved == false)
            {
                Managers.Sound.PlaySfx(ESfxSoundType.SavePoint);
                isLeftPlayerSaved = true;
                leftStageParam.isHaveGoggle = true;
                OnLeftReceiveStageParamCallBack(leftStageParam);
            }
        }
        else if(teamType == ETeamType.Right)
        {
            rightPlayerPosNum = platformId;

            if(rightPlayerPosNum == (int)EPlatformType.SavePoint && isRightPlayerSaved == false)
            {
                Managers.Sound.PlaySfx(ESfxSoundType.SavePoint);
                isRightPlayerSaved = true;
                rightStageParam.isHaveGoggle = true;
                OnRightReceiveStageParamCallBack(rightStageParam);
            }
        }
    }

    public Vector3 GetJumpTargetPos(ETeamType teamType)
    {
        EDirection dir = (teamType == ETeamType.Left) ? leftStageParam.LookAtDir : rightStageParam.LookAtDir;
        return GetPlatformPosition(((teamType == ETeamType.Left) ? leftPlayerPosNum : rightPlayerPosNum) + 1, teamType, dir);
    }

    private Vector3 GetPlatformPosition(int jumpTargetPosNum, ETeamType teamType, EDirection dir)
    {
        if(jumpTargetPosNum == (int)EPlatformType.SavePoint)
        {
            return playerSavePoint.position + new Vector3(
                (teamType == ETeamType.Left) ? offSetPosX * -1f : offSetPosX, offSetPosY, 0);
        }
        else if (jumpTargetPosNum == (int)EPlatformType.EndPoint)
        {
            return playerEndPoint.position + new Vector3(
                (teamType == ETeamType.Left) ? offSetPosX * -1f : offSetPosX, offSetPosY, 0);
        }

        return platformGroupController.GetPlatformPos(jumpTargetPosNum, teamType, dir) + new Vector3(0, offSetPosY, 0);
    }

    public Vector3 GetSpawnPoint(ETeamType teamType)
    {
        if (teamType == ETeamType.Left)
        {
            Vector3 offSetVec = new Vector3(offSetPosX * -1, offSetPosY, 0);
            if (leftPlayerPosNum >= (int)EPlatformType.SavePoint)
                return playerSavePoint.position + offSetVec;
            return playerStartPoint.position + offSetVec;
        }
        else if (teamType == ETeamType.Right)
        {
            Vector3 offSetVec = new Vector3(offSetPosX, offSetPosY, 0);
            if (rightPlayerPosNum >= (int)EPlatformType.SavePoint)
                return playerSavePoint.position + offSetVec;
            return playerStartPoint.position + offSetVec;

        }
#if DEBUG
        Debug.LogError("없는 타입");
#endif
        return Vector3.zero;
    }

    public void OnUseGoggleItem(ETeamType teamType)
    {
        if (teamType == ETeamType.Left && leftStageParam.isHaveGoggle
            && !IsCheckSpecialPlatform(leftPlayerPosNum + 1))
        {
            SpawnGoggleItemEffect(leftPlayerPosNum + 1);
            leftStageParam.isHaveGoggle = false;
            OnLeftReceiveStageParamCallBack(leftStageParam);
        }
        else if (teamType == ETeamType.Right && rightStageParam.isHaveGoggle
            && !IsCheckSpecialPlatform(rightPlayerPosNum + 1))
        {
            SpawnGoggleItemEffect(rightPlayerPosNum + 1);
            rightStageParam.isHaveGoggle = false;
            OnRightReceiveStageParamCallBack(rightStageParam);
        }
    }

    private bool IsCheckSpecialPlatform(int platformId)
    {
        return (platformId == (int)EPlatformType.SavePoint || platformId == (int)EPlatformType.EndPoint);
    }

    private void SpawnGoggleItemEffect(int platformId)
    {
        Vector3 spawnPos = platformGroupController.IsLandablePosition(platformId);
        ParticleEffectObject goggleItemEffect = ObjectCreator.SpawnEffect<ParticleEffectObject>(EEffectType.GoggleItemEffect, spawnPos);
        GoggleItemEffectList.Add((goggleItemEffect, platformId));
    }

    public void OnChangeTarget(ETeamType teamType, EDirection dir)
    {
        if (teamType == ETeamType.Left)
        {
            leftStageParam.LookAtDir = dir;
            OnLeftReceiveStageParamCallBack(leftStageParam);
        }
        else if (teamType == ETeamType.Right) 
        {
            rightStageParam.LookAtDir = dir;
            OnRightReceiveStageParamCallBack(rightStageParam);
        }
    }

    public override Vector3 GetStartPoint(ETeamType teamType)
    {
        Vector3 offsetVec = new Vector3((teamType == ETeamType.Left) ? -offSetPosX : offSetPosX, 0, 0);
        return playerStartPoint.transform.position + offsetVec;
    }
}
