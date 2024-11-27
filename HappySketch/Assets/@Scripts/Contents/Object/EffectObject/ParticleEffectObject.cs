using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectObject : BaseObject
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.ObjectType = EObjectType.Effect;

        return true;
    }

    // 구현해야 함 ㅋ
}
