using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public enum EItemType
{
    // 지울 예정ㅋ
    RedCandy,
    GreenCandy,
    BlueCandy,
    Bomb,
    Star,
    // 일단 다 지우장
        
    Max
}

public enum ECandyItemType
{
    RedCandy,
    GreenCandy,
    BlueCandy,
    Max
}

public class BaseItem : BaseObject
{
    protected EItemType itemType = EItemType.Max;
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
