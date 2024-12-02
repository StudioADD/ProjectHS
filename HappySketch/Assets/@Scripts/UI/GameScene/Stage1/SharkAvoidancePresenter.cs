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
            sharkModel.OnLeftRatioChanged -= SetLeftRatio;
            sharkModel.OnLeftRatioChanged += SetLeftRatio;

            sharkModel.OnRightRatioChanged -= SetRightRatio;
            sharkModel.OnRightRatioChanged += SetRightRatio;

            switch(teamType)
            {
                case ETeamType.Left:
                    sharkModel.OnLeftRatioChanged -= SetProgressRatio;
                    sharkModel.OnLeftRatioChanged += SetProgressRatio;

                    sharkModel.OnLeftItemRatioChanged -= SetItemRatio;
                    sharkModel.OnLeftItemRatioChanged += SetItemRatio;
                    break;

                case ETeamType.Right:
                    sharkModel.OnRightRatioChanged -= SetProgressRatio;
                    sharkModel.OnRightRatioChanged += SetProgressRatio;

                    sharkModel.OnRightItemRatioChanged -= SetItemRatio;
                    sharkModel.OnRightItemRatioChanged += SetItemRatio;
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

    private void SetItemCount(int itemCount)
    {
        if (model is SharkAvoidanceModel sharkModel)
        {
            switch(teamType)
            {
                case ETeamType.Left:
                    sharkModel.SetLeftItemCount(itemCount);
                    break;

                case ETeamType.Right:
                    sharkModel.SetRightItemCount(itemCount);
                    break;
            }
        }

        if (view is SharkAvoidanceView sharkView)
            sharkView.UpdateItemCount(itemCount);
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

    private void SetItemRatio(float ratio)
    {
        if (view is SharkAvoidanceView sharkView)
            sharkView.UpdateItemRatio(ratio);
    }

    public override void EndStage()
    {
        if (model is SharkAvoidanceModel sharkModel)
            sharkModel.Clear();
    }
}
