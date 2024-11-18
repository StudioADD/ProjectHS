using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : BaseItem
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        itemType = EItemType.RedCandy;
        score = 100;
        return true;
    }
}
