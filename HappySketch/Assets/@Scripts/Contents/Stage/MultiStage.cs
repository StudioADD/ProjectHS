using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 두 개의 스테이지에 각각의 플레이어가 플레이하는 스테이지
/// </summary>
public abstract class MultiStage : BaseStage
{
    [field: SerializeField, ReadOnly] public ETeamType TeamType { get; protected set; }
    [SerializeField, ReadOnly] protected Player player;

    public event Action<StageParam> OnReceiveStageParam;

    protected void OnReceiveStageParamCallBack(StageParam stageParam)
    {
        if (Managers.Game.IsGamePlay == false)
            return;

        OnReceiveStageParam?.Invoke(stageParam);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(Player player)
    {
        StageType = Managers.Game.GetCurrStageType();
        this.player = player;

        TeamType = player.TeamType;
    }

    public override void StartStage()
    {
        player.SetInfo((int)StageType);
    }

    public override void EndStage(ETeamType winnerTeam)
    {
        player.OnEndStage(player.TeamType == winnerTeam);
    }

    public virtual void ConnectEvents(Action<ETeamType> onEndGameCallBack)
    {

    }

    public Vector3 GetStartPoint() => playerStartPoint.position;

    protected Coroutine coReceiveStageParam = null;
    protected virtual IEnumerator CoReceiveStageParam() 
    {     
        yield return null;
    }
}
