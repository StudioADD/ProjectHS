using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public enum EItemType
{
    None, // 필요없으면 뺄 것
    RedCandy,
    GreenCandy,
    BlueCandy,
    Bomb,
    Star,

    Max
}

public class BaseItem : BaseObject
{
    protected EItemType itemType = EItemType.None;
    protected float score = 0f;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        this.gameObject.tag = ETag.Item.ToString();
        this.ObjectType = EObjectType.Item;
        return true;
    }
}
