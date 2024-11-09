using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField] StageGroupController stageGroup;
    [SerializeField] CameraGroupController cameraGroup;

    [SerializeField, ReadOnly] Player leftPlayer;
    [SerializeField, ReadOnly] Player rightPlayer;

    const int STAGE_DISTANCE = 10;

    protected virtual void Reset()
    {
        stageGroup = Util.FindChild<StageGroupController>(gameObject);
        cameraGroup = Util.FindChild<CameraGroupController>(gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;

        return true;
    }

    public void StartStage(EStageType stageType)
    {
        stageGroup.SetInfo(stageType, STAGE_DISTANCE);
        cameraGroup.SetInfo(stageType, STAGE_DISTANCE);

        // 맘에 안드는 상태 (임시)
        leftPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/LeftPlayer").GetComponent<Player>();
        leftPlayer.transform.position = stageGroup.GetStagePlayerStartPos(ETeamType.Left);
        leftPlayer.transform.position += Vector3.up * leftPlayer.GetColliderHeight();
        leftPlayer.SetInfo((int)stageType);

        rightPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/RightPlayer").GetComponent<Player>();
        rightPlayer.transform.position = stageGroup.GetStagePlayerStartPos(ETeamType.Right);
        rightPlayer.transform.position += Vector3.up * rightPlayer.GetColliderHeight();
        rightPlayer.SetInfo((int)stageType);

        cameraGroup.SetTarget(leftPlayer, ETeamType.Left);
        cameraGroup.SetTarget(rightPlayer, ETeamType.Right);

        if (Managers.UI.SceneUI is UI_GameScene uI_GameScene)
            uI_GameScene.StartStage(stageType);
    }

    public void EndStage(ETeamType winningTeam)
    {
        // 플레이어한테 정보 전달

        // 
    }
    
    public override void Clear()
    {

    }


    #region Test

    public void ConnectInputAction(bool isConnect)
    {
        Managers.Input.OnVKeyEntered -= StageClear;
        Managers.Input.OnCKeyEntered -= EndGame;

        if (isConnect)
        {
            Managers.Input.OnVKeyEntered += StageClear;
            Managers.Input.OnCKeyEntered += EndGame;
        }
    }

    public void StageClear()
    {
        // 
    }

    public void EndGame()
    {
        // 결과 UI를 띄울까?
    }
    #endregion
}
