using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECandyItemType
{
    RedCandy,
    GreenCandy,
    BlueCandy,
    Max
}

public class CandyItem : BaseItem
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        return true;
    }

    public override void SetInfo(EItemType itemType)
    {


    }
}
