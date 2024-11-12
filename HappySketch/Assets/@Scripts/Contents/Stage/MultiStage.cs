using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 두 개의 스테이지에 각각의 플레이어가 플레이하는 스테이지
/// </summary>
public class MultiStage : BaseStage
{
    [field: SerializeField, ReadOnly]
    public Transform PlayerSpawnPoint { get; protected set; } = null;


    protected override void Reset()
    {
        base.Reset();

        PlayerSpawnPoint = Util.FindChild<Transform>(this.gameObject, "PlayerSpawnPoint", false);
        PlayerSpawnPoint ??= Util.Editor_InstantiateObject(this.transform).transform;
        PlayerSpawnPoint.gameObject.name = "PlayerSpawnPoint";
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        

        return true;
    }
}
