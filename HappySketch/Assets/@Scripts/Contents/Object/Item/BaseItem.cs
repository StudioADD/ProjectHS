using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public enum EItemType
{
    // SharkAvoidanceStage
    BoosterItem,

    // CollectingCandyStage



    Max
}

public abstract class BaseItem : BaseObject
{
    protected EItemType itemType = EItemType.Max;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        this.gameObject.tag = Util.EnumToString(Define.ETag.Item);
        this.gameObject.layer = (int)Define.ELayer.Item;
        this.ObjectType = EObjectType.Item;
        return true;
    }

    public abstract void SetInfo(EItemType itemType);
}
