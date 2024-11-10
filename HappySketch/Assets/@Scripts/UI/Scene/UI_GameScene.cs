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

    private Dictionary<EStageType, Action<UITeamData>> stageHandle;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        stageHandle = new Dictionary<EStageType, Action<UITeamData>>
        {
            {EStageType.SharkAvoidance, HandleSharkAvoidance },
            {EStageType.CollectingCandy, HandleCollectingCandy },
            {EStageType.CrossingBridge, HandleCrossingBridge }
        };

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

        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                currPresentLeft = new SharkAvoidancePresenter(leftView, new SharkAvoidanceModel(ETeamType.Left));
                currPresentRight = new SharkAvoidancePresenter(rightView, new SharkAvoidanceModel(ETeamType.Right));
                break;

            case EStageType.CollectingCandy:
                // 캔디 모델로 바꿔야 함
                currPresentLeft = new CollectCandyPresenter(leftView, new SharkAvoidanceModel(ETeamType.Left));
                currPresentRight = new CollectCandyPresenter(rightView, new SharkAvoidanceModel(ETeamType.Left));
                break;

            case EStageType.CrossingBridge:
                // BrigePresenter 설정
                //currPresentLeft = new SharkAvoidancePresenter(leftView);
                //currPresentRight = new SharkAvoidancePresenter(rightView);
                break;
        }
    }

    public void ReceiveData(UIDataBase data)
    {
        switch(data)
        {
            case UITeamData teamData:
                stageHandle[data.StageType](teamData);
                break;

            case UICommonData commonData:
                HandleCommonData(commonData);
                break;
        }
    }

    private void HandleSharkAvoidance(UITeamData teamData)
    {
        switch (teamData.TeamType)
        {
            case ETeamType.Left:
                {
                    if (teamData is UIBoosterCountData boosterCountData)
                    {
                        (currPresentLeft as SharkAvoidancePresenter).SetItemCount(boosterCountData.BoosterCount);
                    }
                }
                break;

            case ETeamType.Right:
                {
                    if (teamData is UIBoosterCountData boosterCountData)
                    {
                        (currPresentRight as SharkAvoidancePresenter).SetItemCount(boosterCountData.BoosterCount);
                    }
                }
                break;
        }
    }

    private void HandleCollectingCandy(UITeamData teamData)
    {

    }

    private void HandleCrossingBridge(UITeamData teamData)
    {

    }

    private void HandleCommonData(UICommonData commonData)
    {

    }
}
