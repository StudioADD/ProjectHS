using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField] StageGroupController stageGroup;
    [SerializeField] CameraGroupController cameraGroup;

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
        // 스테이지 로드
        stageGroup.SetInfo(stageType);
        
        // 스테이지 정보를 가지고 각 카메라 타겟 세팅
        cameraGroup.SetInfo();
    }
    
    public override void Clear()
    {

    }
}
