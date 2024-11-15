using System.Collections;
using System.Collections.Generic;
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

    public virtual void EndStage(ETeamType winnerTeam)
    {
        Managers.Game.EndStage(winnerTeam);
    }

    public virtual void SetInfo(EStageType stageType)
    {
        StageType = stageType;
        LightingController.SetStageLighting(stageType);
    }

    public abstract void ConnectEvents(Player leftPlayer, Player rightPlayer);
    public abstract Vector3 GetStagePlayerStartPos(ETeamType teamType);
}
