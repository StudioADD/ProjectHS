using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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

        leftStage = Managers.Resource.Instantiate(prefabPath, this.transform).GetComponent<BaseStage>();
        rightStage = Managers.Resource.Instantiate(prefabPath, this.transform).GetComponent<BaseStage>();

        leftStage.transform.position = Vector3.left * 10f;
        rightStage.transform.position = Vector3.right * 10f;
    }

    public Vector3 GetStagePlayerStartPos(ETeamType teamType)
    {
        if (teamType == ETeamType.Left)
            return leftStage.PlayerStartTr.position;
        else if (teamType == ETeamType.Right)
            return rightStage.PlayerStartTr.position;
        else
            Debug.LogWarning("알 수 없는 팀이 들어옴");

        return Vector3.zero;
    }

    public void Clear()
    {

    }

}
