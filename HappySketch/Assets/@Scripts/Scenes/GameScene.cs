using System;
using UnityEditor;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField, ReadOnly] BaseStageController stageController;
    [SerializeField] CameraGroupController cameraGroup;

    [SerializeField, ReadOnly] Player leftPlayer;
    [SerializeField, ReadOnly] Player rightPlayer;


    protected virtual void Reset()
    {
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
        // StageController
        Type type = Type.GetType($"{stageType}Stage");
        BaseStage tempStage = Activator.CreateInstance(type) as BaseStage;
        GameObject stageControllerObj = new GameObject("StageController");
        stageControllerObj.transform.SetParent(transform, false);

        if (tempStage is SingleStage singleStage)
        {
            stageController = stageControllerObj.AddComponent<SingleStageController>();
        }
        else if(tempStage is MultiStage multiStage)
        {
            stageController = stageControllerObj.AddComponent<MultiStageController>();
        }
        else
        {
            Debug.LogWarning($"없는 타입 : {tempStage.GetType().Name}");
            return;
        }
        stageController.SetInfo(stageType);

        // Player
        leftPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/LeftPlayer").GetComponent<Player>();
        leftPlayer.transform.position = stageController.GetStagePlayerStartPos(ETeamType.Left);
        leftPlayer.transform.position += Vector3.up * leftPlayer.GetColliderHeight();
        leftPlayer.SetInfo((int)stageType);

        rightPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/RightPlayer").GetComponent<Player>();
        rightPlayer.transform.position = stageController.GetStagePlayerStartPos(ETeamType.Right);
        rightPlayer.transform.position += Vector3.up * rightPlayer.GetColliderHeight();
        rightPlayer.SetInfo((int)stageType);

        // Camera
        cameraGroup.SetInfo(stageType);
        cameraGroup.SetTarget(leftPlayer, ETeamType.Left);
        cameraGroup.SetTarget(rightPlayer, ETeamType.Right);

        // StartStage
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
