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
    private PresenterBase currPresentLeft;
    private PresenterBase currPresentRight;

    private EStageType currStageType;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void StartStage(EStageType stageType)
    {
        currStageType = stageType;

        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                SetStageUI<GameStage1Presenter>(stageType);
                break;

            case EStageType.CollectingCandy:
                SetStageUI<GameStage2Presenter>(stageType);
                break;

            case EStageType.CrossingBridge:
                SetStageUI<GameStage2Presenter>(stageType);
                break;
        }
    }

    private void SetStageUI<T> (EStageType stageType) where T : PresenterBase
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
        
        ViewBase leftView = leftObject.GetComponent<ViewBase>();
        ViewBase rightView = rightObject.GetComponent<ViewBase>();

        // currPresentLeft = Activator.CreateInstance(typeof(T), leftView) as T;
        // currPresentRight = Activator.CreateInstance(typeof(T), rightView) as T;
    }

    public void ReceiveData(UIDataBase data)
    {
            
    }

    public void ReceiveCommonData()
    {

    }
}
