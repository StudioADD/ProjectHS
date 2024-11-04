using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField] StageGroupController stageGroup;
    [SerializeField] CameraGroupController cameraGroup;

    // 임시
    [SerializeField, ReadOnly] TestPlayer leftPlayer;
    [SerializeField, ReadOnly] TestPlayer rightPlayer;

    private void Start()
    {
        Managers.Game.StartGame();
    }

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

    public void SetInfo(EStageType stageType)
    {
        stageGroup.SetInfo(stageType);
        // cameraGroup.SetInfo();

        // 플레이어 소환 (임시 코드)
        string prefabPath = $"{PrefabPath.OBJECT_PLAYER_PATH}/TestPlayer";
        leftPlayer = Managers.Resource.Instantiate(prefabPath).GetComponent<TestPlayer>();
        leftPlayer.transform.position = stageGroup.GetStagePlayerStartPos(ETeamType.Left);
        leftPlayer.transform.position += Vector3.up * leftPlayer.GetColliderHeight();

        rightPlayer = Managers.Resource.Instantiate(prefabPath).GetComponent<TestPlayer>();
        rightPlayer.transform.position = stageGroup.GetStagePlayerStartPos(ETeamType.Right);
        rightPlayer.transform.position += Vector3.up * rightPlayer.GetColliderHeight();

        cameraGroup.SetTarget(leftPlayer, ETeamType.Left);
        cameraGroup.SetTarget(rightPlayer, ETeamType.Right);
    }
    
    public override void Clear()
    {

    }
}
