using System;
using UnityEditor;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField, ReadOnly] BaseStageController stageController;
    [SerializeField] CameraGroupController cameraGroupController;

    [SerializeField, ReadOnly] Player leftPlayer;
    [SerializeField, ReadOnly] Player rightPlayer;

    protected virtual void Reset()
    {
        cameraGroupController = Util.FindChild<CameraGroupController>(gameObject);
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
        // UI ( GameStart )
        if (Managers.UI.SceneUI is UI_GameScene uI_GameScene)
            uI_GameScene.StartStage(stageType);
        
        // StageController
        Type type = Type.GetType($"{stageType}Stage");
        GameObject stageControllerObj = new GameObject("StageController");
        stageControllerObj.transform.SetParent(transform, false);

        if (type.BaseType == typeof(SingleStage))
        {
            stageController = stageControllerObj.AddComponent<SingleStageController>();
        }
        else if(type.BaseType == typeof(MultiStage))
        {
            stageController = stageControllerObj.AddComponent<MultiStageController>();
        }
        else
        {
            Debug.LogWarning($"없는 타입 : {type.Name}");
            return;
        }
        stageController.SetInfo(stageType);

        // 스테이지 컨트롤러에서 할까?
        leftPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/LeftPlayer").GetComponent<Player>();
        leftPlayer.transform.position = stageController.GetStagePlayerStartPos(ETeamType.Left);
        leftPlayer.transform.position += Vector3.up * leftPlayer.GetColliderHeight();
       //  leftPlayer.SetInfo((int)stageType);

        rightPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/RightPlayer").GetComponent<Player>();
        rightPlayer.transform.position = stageController.GetStagePlayerStartPos(ETeamType.Right);
        rightPlayer.transform.position += Vector3.up * rightPlayer.GetColliderHeight();
        // rightPlayer.SetInfo((int)stageType);

        // Camera
        cameraGroupController.SetInfo(stageType);
        cameraGroupController.SetTarget(leftPlayer, ETeamType.Left); 
        cameraGroupController.SetTarget(rightPlayer, ETeamType.Right);

        // Connect Events
        stageController.ConnectEvents(leftPlayer, rightPlayer);

        // UI 테스트
        if (Managers.UI.SceneUI is UI_GameScene uiGameScene)
        {
            // uiGameScene.ConnectStageEvents()
        }
    }

    public override void Clear()
    {
        
    }
}
