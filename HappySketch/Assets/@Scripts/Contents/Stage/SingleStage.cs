using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 하나의 스테이지로 두 명의 플레이어가 플레이하는 스테이지
/// </summary>
public class SingleStage : BaseStage
{
    [field: SerializeField, ReadOnly]
    public Transform LeftPlayerSpawnPoint { get; protected set; } = null;
    
    [field: SerializeField, ReadOnly] 
    public Transform RightPlayerSpawnPoint { get; protected set; } = null;

    protected virtual void Reset()
    {
        LeftPlayerSpawnPoint = Util.FindChild<Transform>(this.gameObject, "LeftPlayerSpawnPoint", false);
        RightPlayerSpawnPoint = Util.FindChild<Transform>(this.gameObject, "RightPlayerSpawnPoint", false);

        LeftPlayerSpawnPoint ??= Util.Editor_InstantiateObject(this.transform).transform;
        RightPlayerSpawnPoint ??= Util.Editor_InstantiateObject(this.transform).transform;

        LeftPlayerSpawnPoint.gameObject.name = "LeftPlayerSpawnPoint";
        RightPlayerSpawnPoint.gameObject.name = "RightPlayerSpawnPoint";
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }
}
