using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MomDra;
using static Define;

public class UI_GameScene : UI_BaseScene
{
    private GameScenePresenter currPresentLeft;
    private GameScenePresenter currPresentRight;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void StartStage(EStageType stageType)
    {
        SetStage(stageType);
    }

    private void SetStage(EStageType stageType)
    {
        // 여기서 UI Prefab을 생성하야 함!
        // View 정보 가저와야 함

        // Prefab을 생성해야 함!
        string prefabPath = $"{PrefabPath.UI_GAME_SCENE_PATH}/UI_{stageType.ToString()}";
        GameSceneView leftView = Managers.Resource.Instantiate(prefabPath).GetComponent<GameSceneView>();
        GameSceneView rightView = Managers.Resource.Instantiate(prefabPath).GetComponent<GameSceneView>();


        // Model은 어떻게 가져올까?
        // 여기서 생성하면 안됨, 그냥 한번 넣어본 코드임
        GameSceneModel leftModel = new GameSceneModel();
        GameSceneModel rightModel = new GameSceneModel();
            
        switch(stageType)
        {
            case EStageType.SharkAvoidance:
                currPresentLeft = new GameSceneStage1Presenter(leftView, leftModel);
                currPresentRight = new GameSceneStage1Presenter(leftView, rightModel);
                break;

            case EStageType.Stage2:

                break;

            case EStageType.Stage3:

                break;
        }
    }
}
