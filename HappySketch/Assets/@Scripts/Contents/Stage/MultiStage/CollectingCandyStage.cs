using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CollectingCandyStage : MultiStage
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.CollectingCandy;

        return true;
    }

    public override void SetInfo(Player player = null)
    {
        base.SetInfo(player);
         
    }

    public override void ConnectEvents(Action<ETeamType> onEndGameCallBack)
    {
        
    }

    public void OnGetCandyItem()
    {

    }
}
