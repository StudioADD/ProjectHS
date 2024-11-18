using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class StageParam 
{
    public ETeamType TeamType;

    public StageParam(ETeamType teamType)
    {
        TeamType = teamType;
    }
}

public class SharkAvoidanceParam : StageParam
{
    public float CurrDisRatio; // 0 ~ 1
    public int BoosterCount; // 0 ~ 3

    public SharkAvoidanceParam(ETeamType teamType, float currDisRatio, int boosterCount)
        : base(teamType)
    {
        CurrDisRatio = currDisRatio;
        BoosterCount = boosterCount;
    }
}

public class CollectingCandyParam : StageParam
{
    public List<ECandyItemType> CandyItemList;
    public int CurrScore;

    public CollectingCandyParam(ETeamType teamType, List<ECandyItemType> CandyItemList, int CurrScore) 
        : base(teamType)
    {
        this.CandyItemList = CandyItemList;
        this.CurrScore = CurrScore;
    }
}

public class CrossingBridgeParam : StageParam
{
    public bool isHaveGoggle;

    public CrossingBridgeParam(ETeamType teamType, bool isHaveGoggle)
        : base(teamType)
    {
        this.isHaveGoggle = isHaveGoggle;
    }
}