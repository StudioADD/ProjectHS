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

public class Scene1Data : UIDataBase
{
    public int BoosterCount { get; set; }

    public Scene1Data(EStageType stageType, ETeamType teamType, int boosterCount) : base(stageType, teamType)
    {
        this.BoosterCount = boosterCount;
    }
}
