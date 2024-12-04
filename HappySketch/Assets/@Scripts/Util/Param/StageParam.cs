using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[Serializable]
public class StageParam 
{
    public ETeamType TeamType;

    public StageParam(ETeamType teamType)
    {
        TeamType = teamType;
    }
}

[Serializable]
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

[Serializable]
public class CollectingCandyParam : StageParam
{
    public int[] CandyItems;
    public int CurrScore;

    public CollectingCandyParam(ETeamType teamType, int[] CandyItems, int CurrScore) 
        : base(teamType)
    {
        this.CandyItems = new int[(int)ECandyItemType.Max];
        this.CurrScore = CurrScore;
    }
}

[Serializable]
public class CrossingBridgeParam : StageParam
{
    public EDirection LookAtDir;
    public bool isHaveGoggle;

    public CrossingBridgeParam(ETeamType teamType, EDirection lookAtDir, bool isHaveGoggle)
        : base(teamType)
    {
        this.LookAtDir = lookAtDir;
        this.isHaveGoggle = isHaveGoggle;
    }
}