using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class MultiStageController : BaseStageController
{
    [SerializeField, ReadOnly] MultiStage leftStage = null;
    [SerializeField, ReadOnly] MultiStage rightStage = null;

    const int STAGE_DISTANCE = 5000;
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        return true;
    }

    public override void SetInfo(EStageType stageType, Player leftPlayer, Player rightPlayer)
    {
        base.SetInfo(stageType, leftPlayer, rightPlayer);

        string prefabPath = $"{PrefabPath.STAGE_PATH}/{stageType}";

        leftStage = Managers.Resource.Instantiate(prefabPath, this.transform).GetComponent<MultiStage>();
        rightStage = Managers.Resource.Instantiate(prefabPath, this.transform).GetComponent<MultiStage>();

        leftStage.transform.position = Vector3.left * STAGE_DISTANCE;
        rightStage.transform.position = Vector3.right * STAGE_DISTANCE;

        SetPlayerPosition(leftPlayer, rightPlayer);

        leftStage.SetInfo(leftPlayer);
        rightStage.SetInfo(rightPlayer);
    }

    public override void EndStage(ETeamType winnerTeam)
    {
        base.EndStage(winnerTeam);


    }
    
    public override void StartStage()
    {
        leftStage.StartStage();
        rightStage.StartStage();
    }

    public override void ConnectEvents()
    {  
        leftStage.ConnectEvents(EndStage);
        rightStage.ConnectEvents(EndStage);
    }

    public BaseStage GetStage(ETeamType teamType)
    {
        return (teamType == ETeamType.Left) ? leftStage : rightStage;
    }

    public override Vector3 GetStagePlayerStartPos(ETeamType teamType)
    {
        if (teamType == ETeamType.Left)
            return leftStage.GetStartPoint();
        
        if (teamType == ETeamType.Right)
            return rightStage.GetStartPoint();

        Debug.LogWarning("알 수 없는 팀이 들어옴");
        return Vector3.zero;
    }

    public void Clear()
    {

    }

}
