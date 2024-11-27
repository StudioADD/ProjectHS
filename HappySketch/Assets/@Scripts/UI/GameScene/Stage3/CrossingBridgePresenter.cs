using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CrossingBridgePresenter : PresenterBase
{
    public CrossingBridgePresenter(ViewBase view, ModelBase model) : base(view, model)
    {

    }

    public override void ConnectStageEvents(BaseStage stage)
    {
        if (stage is SingleStage singleStage)
        {
            // 얘랑 라이트랑 연결해주어야 함.
            // 내 팀타입 필요 (희용)
            // singleStage.OnLeftReceiveStageParam
        }
    }

    public override void OnStageInfoUpdate(StageParam param)
    {
        if(param is  CrossingBridgeParam crossingParam)
        {
            SetIsActive(crossingParam.isHaveGoggle);
        }
    }

    public void SetIsActive(bool isActive)
    {
        if(view is CrossingBridgeView crossingBridgeView)
        {
            crossingBridgeView.SetActiveGoogleImage(isActive);
        }
    }
}
