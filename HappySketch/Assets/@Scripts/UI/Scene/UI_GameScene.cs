using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MomDra;
using static Define;
using UnityEditor;
using System;
using UnityEngine.UI;

public class UI_GameScene : UI_BaseScene
{
    private GameScenePresenterBase currPresentLeft;
    private GameScenePresenterBase currPresentRight;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void StartStage(EStageType stageType)
    {
        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                SetStageUI<GameStage1Presenter>(stageType);
                break;

            case EStageType.Stage2:
                SetStageUI<GameStage2Presenter>(stageType);
                break;

            case EStageType.Stage3:
                SetStageUI<GameStage2Presenter>(stageType);
                break;
        }
    }

    private void SetStageUI<T> (EStageType stageType) where T : GameScenePresenterBase
    {
        // 여기서 UI Prefab을 생성하야 함!
        // View 정보 가저와야 함

        // Prefab을 생성해야 함!
        string prefabPath = $"{PrefabPath.UI_GAME_SCENE_PATH}/UI_{stageType}";

        GameObject leftObject = Managers.Resource.Instantiate(prefabPath, transform);
        GameObject rightObject = Managers.Resource.Instantiate(prefabPath, transform);

        leftObject.name = $"{leftObject.name}_Left";
        rightObject.name = $"{rightObject.name}_Right";

        RectTransform rectTransform = rightObject.GetComponent<RectTransform>();
        Vector2 anchor = new Vector2(1f, 0.5f);
        rectTransform.anchorMin = anchor;
        rectTransform.anchorMax = anchor;
        rectTransform.pivot = anchor;
        rectTransform.anchoredPosition = Vector3.zero;
        
        GameSceneViewBase leftView = leftObject.GetComponent<GameSceneViewBase>();
        GameSceneViewBase rightView = rightObject.GetComponent<GameSceneViewBase>();

        // Model은 어떻게 가져올까?
        // 여기서 생성하면 안됨, 그냥 한번 넣어본 코드임
        GameSceneModelBase leftModel = new GameSceneModelBase();
        GameSceneModelBase rightModel = new GameSceneModelBase();

        currPresentLeft = Activator.CreateInstance(typeof(T), leftView, leftModel) as T;
        currPresentRight = Activator.CreateInstance(typeof(T), rightView, rightModel) as T;
    }
}
