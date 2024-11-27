using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;

public abstract class BaseStageController : InitBase
{
    public EStageType StageType { get; protected set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(EStageType stageType, Player leftPlayer, Player rightPlayer)
    {
        StageType = stageType;

        LightingController.SetStageLighting(stageType);
    }

    public virtual void EndStage(ETeamType winnerTeam)
    {
        Managers.Game.EndStage(winnerTeam);
    }

    protected void SetPlayerPosition(Player leftPlayer, Player rightPlayer)
    {
        leftPlayer.transform.position = GetStagePlayerStartPos(ETeamType.Left);
        leftPlayer.transform.position += Vector3.up * leftPlayer.GetColliderHeight();

        rightPlayer.transform.position = GetStagePlayerStartPos(ETeamType.Right);
        rightPlayer.transform.position += Vector3.up * rightPlayer.GetColliderHeight();
    }

    public abstract void StartStage();
    public abstract void ConnectEvents();
    public abstract Vector3 GetStagePlayerStartPos(ETeamType teamType);
}
