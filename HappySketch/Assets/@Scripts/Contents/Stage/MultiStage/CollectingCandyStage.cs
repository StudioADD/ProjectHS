using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CollectingCandyStage : MultiStage
{
    

    protected override void Reset()
    {
        base.Reset();

    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void ConnectEvents(Action<ETeamType> onEndGameCallBack)
    {
        
    }
}
