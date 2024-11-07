using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UIDataBase
{
    EStageType stageType;
    ETeamType teamType;

    public UIDataBase(EStageType stageType, ETeamType teamType)
    {
        this.stageType = stageType;
        this.teamType = teamType;
    }
}

public class BoosterCountData : UIDataBase
{
    public int BoosterCount { get; set; }

    public BoosterCountData(EStageType stageType, ETeamType teamType, int boosterCount) : base(stageType, teamType)
    {
        this.BoosterCount = boosterCount;
    }
}
