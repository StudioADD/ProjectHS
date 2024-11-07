using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UIDataBase
{
    public EStageType StageType { get; }

    public UIDataBase(EStageType stageType)
    {
        this.StageType = stageType;
    }
}

public class UICommonData : UIDataBase
{
    public UICommonData(EStageType stageType) : base(stageType)
    {

    }
}

public class UITeamData : UIDataBase
{
    public ETeamType TeamType { get; }

    public UITeamData(EStageType stageType, ETeamType teamType) : base(stageType)
    {
        this.TeamType = teamType;
    }
}

public class UIBoosterCountData : UITeamData
{
    public int BoosterCount { get; set; }

    public UIBoosterCountData(EStageType stageType, ETeamType teamType, int boosterCount) : base(stageType, teamType)
    {
        this.BoosterCount = boosterCount;
    }
}
