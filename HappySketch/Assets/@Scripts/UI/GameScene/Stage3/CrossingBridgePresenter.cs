using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CrossingBridgePresenter : PresenterBase
{
    public CrossingBridgePresenter(ViewBase view, ModelBase model, ETeamType teamType) : base(view, model, teamType)
    {

    }

    public override void ConnectStageEvents(BaseStage stage)
    {
        if (stage is SingleStage singleStage)
        {
            // 얘랑 라이트랑 연결해주어야 함.
            // 내 팀타입 필요 (희용)
            // singleStage.OnLeftReceiveStageParam

            switch (teamType)
            {
                case ETeamType.Left:
                    singleStage.OnLeftReceiveStageParam -= OnStageInfoUpdate;
                    singleStage.OnLeftReceiveStageParam += OnStageInfoUpdate;
                    break;

                case ETeamType.Right:
                    singleStage.OnRightReceiveStageParam -= OnStageInfoUpdate;
                    singleStage.OnRightReceiveStageParam += OnStageInfoUpdate;
                    break;
            }
        }
    }

    public override void OnStageInfoUpdate(StageParam param)
    {
        if(param is CrossingBridgeParam crossingParam)
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
