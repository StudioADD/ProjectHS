using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SharkAvoidanceStage : MultiStage
{
    [SerializeField, ReadOnly]
    protected FinishLineObject finishLineObject;

    [field: SerializeField, ReadOnly]
    List<SpawnPointObject> spawnPointList = new List<SpawnPointObject>();
    
    protected override void Reset()
    {
        base.Reset();

        finishLineObject = Util.FindChild<FinishLineObject>(gameObject, "FinishLineObject", false);

        Transform[] children = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.TryGetComponent<SpawnPointObject>(out SpawnPointObject spawnPoint))
                spawnPointList.Add(spawnPoint);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.SharkAvoidance;

        return true;
    }

    public override void SetInfo(Player player = null)
    {
        base.SetInfo();


    }

    public override void ConnectEvents(Action<Define.ETeamType> onEndGameCallBack)
    {
        if (finishLineObject != null)
        {
            finishLineObject.OnArriveFinishLine -= onEndGameCallBack;
            finishLineObject.OnArriveFinishLine += onEndGameCallBack;
        }
        else
            Debug.LogWarning($"FinishLineObject is Null!!");
    }

    // Coroutine coStage
}
