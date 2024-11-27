using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SharkAvoidancePresenter : PresenterBase
{
    public SharkAvoidancePresenter(ViewBase view, ModelBase model) : base(view, model)
    {
        if(model is SharkAvoidanceModel sharkModel)
        {
            sharkModel.progressEvent -= SetProgressRatio;
            sharkModel.progressEvent += SetProgressRatio;
        }
    }

    public override void ConnectStageEvents(BaseStage stage)
    {
        if (stage is MultiStage multiStage)
        {
            multiStage.OnReceiveStageParam -= OnStageInfoUpdate;
            multiStage.OnReceiveStageParam += OnStageInfoUpdate;
        }
    }

    public override void OnStageInfoUpdate(StageParam param)
    {
        if(param is SharkAvoidanceParam sharkAvoidanceParam)
        {
            SetProgressRatio(sharkAvoidanceParam.TeamType, sharkAvoidanceParam.CurrDisRatio);
            SetItemCount(sharkAvoidanceParam.BoosterCount);
        }
    }

    /// <summary>
    /// 아이템 수를 설정하는 함수
    /// </summary>
    /// <param name="count"> 아이템 수: 0 ~ 3 </param>
    private void SetItemCount(int count)
    {
        if (model is SharkAvoidanceModel sharkModel)
            sharkModel.SetItemCount(count);

        if (view is SharkAvoidanceView sharkView)
            sharkView.UpdateItemCount(count);
    }

    /// <summary>
    /// 총 이동경로 대비 현재 이동 경로 비율
    /// </summary>
    /// <param name="ratio">비율: 0 ~ 1 </param>
    private void SetProgressRatio(ETeamType teamType, float ratio)
    {
        if (model is SharkAvoidanceModel sharkModel)
        {
            if (view is SharkAvoidanceView sharkView)
            {
                switch (teamType)
                {
                    case ETeamType.Left:
                        sharkModel.SetLeftProgressRatio(sharkView.GetProgressRatio(), ratio);
                        sharkView.UpdateLeftProgressRatio(ratio);

                        break;

                    case ETeamType.Right:
                        sharkModel.SetRightProgressRatio(sharkView.GetProgressRatio(), ratio);
                        sharkView.UpdateRightProgressRatio(ratio);

                        break;
                }
            }
        }
    }

    private void SetProgressRatio(float ratio)
    {
        if (view is SharkAvoidanceView sharkView)
            sharkView.UpdateProgressingBar(ratio);
    }
}
