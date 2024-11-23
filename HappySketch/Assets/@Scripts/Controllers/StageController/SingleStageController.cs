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

    public override void SetInfo(EStageType stageType)
    {
        base.SetInfo(stageType);

        string prefabPath = $"{PrefabPath.STAGE_PATH}/{stageType}";
        singleStage = Managers.Resource.Instantiate(prefabPath, this.transform).GetComponent<SingleStage>();
        singleStage.transform.position = Vector3.zero;
    }

    public override void ConnectEvents(Player leftPlayer, Player rightPlayer)
    {

    }

    public override Vector3 GetStagePlayerStartPos(ETeamType teamType)
    {
        return singleStage.GetStartPoint(teamType);
    }

    public void Clear()
    {

    }
}
