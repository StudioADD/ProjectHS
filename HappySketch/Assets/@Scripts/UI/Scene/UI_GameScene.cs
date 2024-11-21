using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        SetStageUI(stageType);
    }

    private void SetStageUI(EStageType stageType)
    {
        string name = $"UI_{stageType}";
        string prefabPath = $"{PrefabPath.UI_OBJECT_PATH}/{name}";

        GameObject leftObject = Managers.Resource.Instantiate(prefabPath, transform);
        GameObject rightObject = Managers.Resource.Instantiate(prefabPath, transform);

        leftObject.name = $"{name}_Left";
        rightObject.name = $"{name}_Right";

        RectTransform rectTransform = rightObject.GetComponent<RectTransform>();
        Vector2 anchor = new Vector2(1f, 0.5f);
        rectTransform.anchorMin = anchor;
        rectTransform.anchorMax = anchor;
        rectTransform.pivot = anchor;
        rectTransform.anchoredPosition = Vector3.zero;

        ViewBase leftView = leftObject.GetComponent<ViewBase>();
        ViewBase rightView = rightObject.GetComponent<ViewBase>();

        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                currPresentLeft = new SharkAvoidancePresenter(leftView, new SharkAvoidanceModel(ETeamType.Left));
                currPresentRight = new SharkAvoidancePresenter(rightView, new SharkAvoidanceModel(ETeamType.Right));
                break;

            case EStageType.CollectingCandy:
                currPresentLeft = new CollectCandyPresenter(leftView, new CollectCandyModel(ETeamType.Left));
                currPresentRight = new CollectCandyPresenter(rightView, new CollectCandyModel(ETeamType.Right));
                break;

            case EStageType.CrossingBridge:
                currPresentLeft = new CrossingBridgePresenter(leftView, new CrossingBridgeModel(ETeamType.Left));
                currPresentRight = new CrossingBridgePresenter(rightView, new CrossingBridgeModel(ETeamType.Right));
                break;
        }
    }

    public void ConnectStageEvents(BaseStage stage)
    {
        if (stage.TeamType == ETeamType.Left)
        {
            currPresentLeft.ConnectStageEvents(stage);
        }
        else // Right
        {
            currPresentRight.ConnectStageEvents(stage);
        }
    }
}
