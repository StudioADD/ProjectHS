using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 하나의 스테이지로 두 명의 플레이어가 플레이하는 스테이지
/// </summary>
public abstract class SingleStage : BaseStage
{
    [SerializeField, ReadOnly] protected Player leftPlayer;
    [SerializeField, ReadOnly] protected Player rightPlayer;

    public event Action<StageParam> OnLeftReceiveStageParam;
    public event Action<StageParam> OnRightReceiveStageParam;

    protected void OnLeftReceiveStageParamCallBack(StageParam stageParam)
    {
        if (Managers.Game.IsGamePlay == false)
            return;

        OnLeftReceiveStageParam?.Invoke(stageParam);
    }

    protected void OnRightReceiveStageParamCallBack(StageParam stageParam)
    {
        if (Managers.Game.IsGamePlay == false)
            return;

        OnRightReceiveStageParam?.Invoke(stageParam);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(Player leftPlayer, Player rightPlayer)
    {
        StageType = Managers.Game.GetCurrStageType();

        this.leftPlayer = leftPlayer;
        this.rightPlayer = rightPlayer;
    }

    public override void StartStage()
    {
        leftPlayer.SetInfo((int)StageType);
        rightPlayer.SetInfo((int)StageType);
    }

    public override void EndStage(ETeamType winnerTeam)
    {
        leftPlayer.OnEndStage(leftPlayer.TeamType == winnerTeam);
        rightPlayer.OnEndStage(rightPlayer.TeamType == winnerTeam);
    }

    public abstract void ConnectEvents(Action<ETeamType> onEndGameCallBack);

    public abstract Vector3 GetStartPoint(ETeamType teamType);
}
