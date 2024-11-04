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

    public void SetInfo(EStageType stageType)
    {
        string prefabPath = $"{PrefabPath.STAGE_PATH}/{stageType}";

        Debug.Log(prefabPath);

        leftStage = Managers.Resource.Instantiate(prefabPath, this.transform).GetComponent<BaseStage>();
        rightStage = Managers.Resource.Instantiate(prefabPath, this.transform).GetComponent<BaseStage>();

        leftStage.transform.position = Vector3.left * 50f;
        rightStage.transform.position = Vector3.right * 50f;
    }

    public void Clear()
    {

    }

}
