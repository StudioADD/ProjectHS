using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGroupController : InitBase
{
    [SerializeField, ReadOnly] BaseStage leftStage;
    [SerializeField, ReadOnly] BaseStage rightStage;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    

    public void Clear()
    {

    }

}
