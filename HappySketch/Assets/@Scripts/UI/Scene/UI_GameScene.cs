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

    private UI_PresenterBase currPresentLeft;
    private UI_PresenterBase currPresentRight;

    private UI_WinLoseController winLoseController;
    private UI_ScorePool scorePool;

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
        stageInstructionWindow.SetInfo(stageType);
    }

    public void StartStage()
    {
        SetStageUI();
    }

    public void EndStage(ETeamType winTeam, int leftWinCount, int rightWinCount, Action onEnd)
    {
        UI_WinLoseController winLoseController = Managers.UI.SpawnObjectUI<UI_WinLoseController>();

        switch(currStageType)
        {
            case EStageType.CollectingCandy:
                winLoseController.EndStageNoGoalImg(winTeam, leftWinCount, rightWinCount, onEnd);
                break;

            default:
                winLoseController.EndStage(winTeam, leftWinCount, rightWinCount, onEnd);
                break;
        }

        currPresentLeft?.Clear();
        currPresentRight?.Clear();
    }

    public UI_ModelBase GetStageUI()
    {
        return currPresentLeft.GetModel();
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

        UI_ViewBase leftView = leftObject.GetComponent<UI_ViewBase>();
        UI_ViewBase rightView = rightObject.GetComponent<UI_ViewBase>();

        switch (currStageType)
        {
            case EStageType.SharkAvoidance:
                UI_SharkAvoidanceModel sharkModel = new UI_SharkAvoidanceModel();
                currPresentLeft = new UI_SharkAvoidancePresenter(leftView, sharkModel, ETeamType.Left);
                currPresentRight = new UI_SharkAvoidancePresenter(rightView, sharkModel, ETeamType.Right);
                break;

            case EStageType.CollectingCandy:
                UI_CollectCandyModel candyModel = new UI_CollectCandyModel();
                currPresentLeft = new UI_CollectCandyPresenter(leftView, candyModel, ETeamType.Left);
                currPresentRight = new UI_CollectCandyPresenter(rightView, candyModel, ETeamType.Right);
                break;

            case EStageType.CrossingBridge:
                currPresentLeft = new UI_CrossingBridgePresenter(leftView, new UI_CrossingBridgeModel(), ETeamType.Left);
                currPresentRight = new UI_CrossingBridgePresenter(rightView, new UI_CrossingBridgeModel(), ETeamType.Right);
                break;
        }
    }

    public void ConnectStageEvents(BaseStage stage)
    {
        switch(stage)
        {
            case MultiStage multiStage:
                {
                    switch(multiStage.TeamType)
                    {
                        case ETeamType.Left:
                            currPresentLeft.ConnectStageEvents(stage);
                            break;

                        case ETeamType.Right:
                            currPresentRight.ConnectStageEvents(stage);
                            break;
                    }
                }
                break;
            case SingleStage singleStage:
                {
                    currPresentLeft.ConnectStageEvents(stage);
                    currPresentRight.ConnectStageEvents(stage);
                }
                break;
        }
    }
}
