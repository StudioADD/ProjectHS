using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEditor;
using System;
using UnityEngine.UI;

public class UI_GameScene : UI_BaseScene
{
    [SerializeField, ReadOnly] UI_StageInstructionWindow stageInstructionWindow;

    private PresenterBase currPresentLeft;
    private PresenterBase currPresentRight;

    private EStageType currStageType = EStageType.None;

    private void Reset()
    {
        stageInstructionWindow = Util.FindChild<UI_StageInstructionWindow>(this.gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(EStageType stageType)
    {
        currStageType = stageType;
        
    }

    public void StartStage()
    {
        SetStageUI();
    }

    private void SetStageUI()
    {
        if(currStageType == EStageType.None)
        {
            Debug.LogError("CurrStageType is None");
            return;
        }

        string name = $"UI_{currStageType}";
        string prefabPath = $"{PrefabPath.UI_STAGE_PATH}/{name}";

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

        switch (currStageType)
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
        switch(stage)
        {
            case MultiStage multiStage:
                {
                    if (multiStage.TeamType == ETeamType.Left)
                        currPresentLeft.ConnectStageEvents(stage);
                    else // Right
                        currPresentRight.ConnectStageEvents(stage);
                    break;
                }
            case SingleStage singleStage:
                {
                    currPresentLeft.ConnectStageEvents(stage);
                    currPresentRight.ConnectStageEvents(stage);
                    break;
                }
        }

       
    }
}
