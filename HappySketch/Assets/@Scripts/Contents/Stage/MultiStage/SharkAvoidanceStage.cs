using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAvoidanceStage : MultiStage
{
    [field: SerializeField, ReadOnly]
    List<SpawnPointObject> spawnPointList = new List<SpawnPointObject>();
    
    protected override void Reset()
    {
        base.Reset();

        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.TryGetComponent<SpawnPointObject>(out SpawnPointObject spawnPoint))
                spawnPointList.Add(spawnPoint);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo()
    {
        base.SetInfo();


    }



    #region Test Input
    protected void ConnectInputActions(bool isConnect)
    {


        if(isConnect)
        {

        }
    }
    #endregion
}
