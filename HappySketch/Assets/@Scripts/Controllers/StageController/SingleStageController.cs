using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SingleStageController : BaseStageController
{
    [SerializeField, ReadOnly] SingleStage singleStage = null;

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
        singleStage = Managers.Resource.Instantiate(prefabPath, this.transform).GetComponent<SingleStage>();
        singleStage.transform.position = Vector3.zero;

        SetPlayerPosition(leftPlayer, rightPlayer);
        singleStage.SetInfo(leftPlayer, rightPlayer);
    }

    public override void StartStage()
    {
        singleStage.StartStage();
    }

    public override void EndStage(ETeamType winnerTeam)
    {
        base.EndStage(winnerTeam);

        singleStage.EndStage(winnerTeam);
    }

    public override void ConnectEvents()
    {
        singleStage.ConnectEvents(EndStage);
    }

    public BaseStage GetStage()
    {
        return singleStage;
    }

    public override Vector3 GetStagePlayerStartPos(ETeamType teamType)
    {
        return singleStage.GetStartPoint(teamType);
    }

    public void Clear()
    {

    }
}
