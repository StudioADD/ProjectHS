using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectType
{
    Player,
    Monster,

    Max
}

public abstract class BaseObject : InitBase
{
    public EObjectType ObjectType { get; protected set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    
}
