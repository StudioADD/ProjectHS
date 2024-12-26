using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_CrossingBridgePresenter : UI_PresenterBase
{
    public UI_CrossingBridgePresenter(UI_ViewBase view, UI_ModelBase model, ETeamType teamType) : base(view, model, teamType)
    {
        if(model is UI_CrossingBridgeModel crossingBridgeModel)
        {
            crossingBridgeModel.OnColorChangedEvent -= SetColor;
            crossingBridgeModel.OnColorChangedEvent += SetColor;
        }
    }

    public override void ConnectStageEvents(BaseStage stage)
    {
        if (stage is SingleStage singleStage)
        {
            // 얘랑 라이트랑 연결해주어야 함.
            // 내 팀타입 필요 (희용)
            // singleStage.OnLeftReceiveStageParam

            switch (TeamType)
            {
                case ETeamType.Left:
                    singleStage.OnLeftReceiveStageParam -= OnStageInfo;
                    singleStage.OnLeftReceiveStageParam += OnStageInfo;
                    break;

                case ETeamType.Right:
                    singleStage.OnRightReceiveStageParam -= OnStageInfo;
                    singleStage.OnRightReceiveStageParam += OnStageInfo;
                    break;
            }
        }
    }

    public override void OnStageInfo(StageParam param)
    {
        if(param is CrossingBridgeParam crossingParam)
        {
            SetIsActive(crossingParam.isHaveGoggle);
        }
    }

    private void SetIsActive(bool isActive)
    {
        if(model is UI_CrossingBridgeModel crossingBridgeModel)
        {
            if(view is UI_CrossingBridgeView crossingBridgeView)
            {
                Color targetColor = Color.white;

                if(!isActive)
                {
                    targetColor.a = 0.5f;
                }

                crossingBridgeModel.SetColor(crossingBridgeView.GetColor(), targetColor);

                if (isActive) crossingBridgeView.RotateEffect();
                else crossingBridgeView.StopEffect();
            }
        }
    }

    private void SetColor(Color color)
    {
        if(view is UI_CrossingBridgeView crossingBridgeView)
        {
            crossingBridgeView.SetColor(color);
        }
    }

    public override void Clear()
    {
        if (model is UI_CrossingBridgeModel crossingBridgeModel)
        {
            crossingBridgeModel.OnColorChangedEvent -= SetColor;
        }
    }
}
