#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEditor : InitBase
{
    [SerializeField, ReadOnly] BaseStage currStage = null;
    
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }
    // 현재 소환된 스테이지를 하나로 보장해줘야 됨.

    public void SummonStage()
    {

    }
}
#endif