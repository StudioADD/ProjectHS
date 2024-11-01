using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMosnterType
{
    SmallShark = 0,
    BigShark,

    Max
}

public abstract class BaseMonster : BaseObject
{
    public EMosnterType MonsterType { get; protected set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;
        this.gameObject.tag = Define.ETag.Monster.ToString();

        return true;
    }

    
}
