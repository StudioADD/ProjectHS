using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Define;

public class SharkAvoidanceModel : ModelBase
{
    private int itemCount;
    private float leftProgressRatio;
    private float rightProgressRatio;

    public SharkAvoidanceModel(ETeamType teamType) : base(teamType)
    {

    }

    public void SetItemCount(int itemCount)
    {
        this.itemCount = itemCount;
    }

    public void SetLeftProgressRatio(float ratio)
    {
        leftProgressRatio = ratio;
    }

    public void SetRightProgressRatio(float ratio)
    {
        rightProgressRatio = ratio;
    }
}
