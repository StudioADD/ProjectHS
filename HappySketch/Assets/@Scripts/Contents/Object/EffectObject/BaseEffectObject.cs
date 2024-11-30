using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public enum EEffectType
{
    ItemBurstEffect = 0,
    CandyItemBurstEffect, 

    StunEffect,
    UseBoosterEffect,
}

public abstract class BaseEffectObject : BaseObject
{
   

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.ObjectType = EObjectType.Effect;

        return true;
    }

    public virtual void SetInfo() { }

}
