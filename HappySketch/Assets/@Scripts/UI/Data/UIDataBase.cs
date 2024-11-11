using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UIDataBase
{
    public EStageType StageType { get; }

    public UIDataBase(EStageType stageType)
    {
        StageType = stageType;
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
        TeamType = teamType;
    }
}

public class UIBoosterCountData : UITeamData
{
    public int BoosterCount { get; }

    public UIBoosterCountData(EStageType stageType, ETeamType teamType, int boosterCount) : base(stageType, teamType)
    {
        BoosterCount = boosterCount;
    }
}

public class UIRatioData : UITeamData
{
    public float Ratio { get; }

    public UIRatioData(EStageType stageType, ETeamType teamType, float ratio) : base(stageType, teamType)
    {
        Ratio = ratio;
    }
}