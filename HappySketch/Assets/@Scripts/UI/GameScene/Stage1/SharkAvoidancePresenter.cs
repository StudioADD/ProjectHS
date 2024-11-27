using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SharkAvoidancePresenter : PresenterBase
{
    public SharkAvoidancePresenter(ViewBase view, ModelBase model, ETeamType teamType) : base(view, model, teamType)
    {
        if(model is SharkAvoidanceModel sharkModel)
        {
            sharkModel.leftRatioEvent -= SetLeftRatio;
            sharkModel.leftRatioEvent += SetLeftRatio;

            sharkModel.rightRatioEvent -= SetRightRatio;
            sharkModel.rightRatioEvent += SetRightRatio;

            switch(teamType)
            {
                case ETeamType.Left:
                    sharkModel.leftRatioEvent -= SetProgressRatio;
                    sharkModel.leftRatioEvent += SetProgressRatio;
                    break;

                case ETeamType.Right:
                    sharkModel.rightRatioEvent -= SetProgressRatio;
                    sharkModel.rightRatioEvent += SetProgressRatio;
                    break;
            }
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
        {
            switch(teamType)
            {
                case ETeamType.Left:
                    sharkModel.SetLeftItemCount(count);
                    break;

                case ETeamType.Right:
                    sharkModel.SetRightItemCount(count);
                    break;
            }
        }

        // ItemCount에도 에니메이션 구현할 거면
        // Model 쪽으로 빼는 것도 고려해야 함!
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
                        sharkModel.SetLeftRatio(ratio);
                        break;

                    case ETeamType.Right:
                        sharkModel.SetRightRatio(ratio);
                        break;
                }
            }
        }
    }

    private void SetProgressRatio(float ratio)
    {
        if (view is SharkAvoidanceView sharkView)
            sharkView.UpdateProgressingBarRatio(ratio);
    }

    private void SetLeftRatio(float ratio)
    {
        if (view is SharkAvoidanceView sharkView)
            sharkView.UpdateLeftRatio(ratio);
    }

    private void SetRightRatio(float ratio)
    {
        if (view is SharkAvoidanceView sharkView)
            sharkView.UpdateRightRatio(ratio);
    }
}
