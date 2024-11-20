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

    //private Dictionary<EStageType, Action<UITeamData>> stageHandle;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //stageHandle = new Dictionary<EStageType, Action<UITeamData>>
        //{
        //    {EStageType.SharkAvoidance, HandleSharkAvoidance },
        //    {EStageType.CollectingCandy, HandleCollectingCandy },
        //    {EStageType.CrossingBridge, HandleCrossingBridge }
        //};

        return true;
    }

    public void StartStage(EStageType stageType)
    {
        currStageType = stageType;
        SetStageUI(stageType);
    }

    private void SetStageUI(EStageType stageType)
    {
        // 여기서 UI Prefab을 생성하야 함!
        // View 정보 가저와야 함

        // Prefab을 생성해야 함!
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

        ModelBase leftModel = leftObject.GetComponent<ModelBase>();
        ModelBase rightModel = rightObject.GetComponent<ModelBase>();

        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                if(leftModel is SharkAvoidanceModel leftSharkModel)
                {
                    leftSharkModel.SetTeamType(ETeamType.Left);
                    currPresentLeft = new SharkAvoidancePresenter(leftView, leftSharkModel);
                }

                if(rightModel is SharkAvoidanceModel rightSharkModel)
                {
                    rightSharkModel.SetTeamType(ETeamType.Right);
                    currPresentRight = new SharkAvoidancePresenter(rightView, rightSharkModel);
                }

                break;

            case EStageType.CollectingCandy:
                if(leftModel is CollectCandyModel leftCandyModel)
                {
                    leftCandyModel.SetTeamType(ETeamType.Left);
                    currPresentLeft = new CollectCandyPresenter(leftView, leftCandyModel);
                }

                if(rightModel is CollectCandyModel rightCandyModel)
                {
                    rightCandyModel.SetTeamType(ETeamType.Right);
                    currPresentRight = new CollectCandyPresenter(rightView, rightCandyModel);
                }
                
                break;

            case EStageType.CrossingBridge:
                if (leftModel is CrossingBridgeModel leftBridgeModel)
                {
                    currPresentLeft = new CollectCandyPresenter(leftView, leftBridgeModel);
                }

                if (rightModel is CrossingBridgeModel rightBridgeModel)
                {
                    currPresentRight = new CollectCandyPresenter(rightView, rightBridgeModel);
                }
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

    
    // 여기부터 아래 제거 해도 될 듯, 이벤트 방식으로 바뀌었음
    //public void ReceiveData(UIDataBase data)
    //{
    //    switch(data)
    //    {
    //        case UITeamData teamData:
    //            stageHandle[data.StageType](teamData);
    //            break;

    //        case UICommonData commonData:
    //            HandleCommonData(commonData);
    //            break;
    //    }
    //}

    //private void HandleSharkAvoidance(UITeamData teamData)
    //{
    //    switch (teamData.TeamType)
    //    {
    //        case ETeamType.Left:
    //            {
    //                if (teamData is UIBoosterCountData boosterCountData)
    //                {
    //                    (currPresentLeft as SharkAvoidancePresenter).SetItemCount(boosterCountData.BoosterCount);
    //                }
    //                else if(teamData is UIRatioData ratioData)
    //                {
    //                    (currPresentLeft as SharkAvoidancePresenter).SetLeftProgressRatio(ratioData.Ratio);
    //                }
    //            }
    //            break;

    //        case ETeamType.Right:
    //            {
    //                if (teamData is UIBoosterCountData boosterCountData)
    //                {
    //                    (currPresentRight as SharkAvoidancePresenter).SetItemCount(boosterCountData.BoosterCount);
    //                }
    //                else if (teamData is UIRatioData ratioData)
    //                {
    //                    (currPresentLeft as SharkAvoidancePresenter).SetLeftProgressRatio(ratioData.Ratio);
    //                }
    //            }
    //            break;
    //    }
    //}

    //private void HandleCollectingCandy(UITeamData teamData)
    //{

    //}

    //private void HandleCrossingBridge(UITeamData teamData)
    //{

    //}

    //private void HandleCommonData(UICommonData commonData)
    //{

    //}
}
