using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField] CameraGroupController cameraGroup;
    [SerializeField] StageGroupController stageGroup;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;

        return true;
    }

    public void SetInfo()
    {
        // 스테이지 로드
        // 

        // 스테이지 정보를 가지고 각 카메라 타겟 세팅
        cameraGroup.SetInfo();
    }
    
    public override void Clear()
    {

    }
}
