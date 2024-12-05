using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static Define;

public class CollectCandyPresenter : PresenterBase
{
    public CollectCandyPresenter(ViewBase view, ModelBase model, ETeamType teamType) : base(view, model, teamType)
    {
        if(model is CollectCandyModel candyModel)
        {
            candyModel.OnTimeChanged -= SetTime;
            candyModel.OnTimeChanged += SetTime;

            switch (teamType)
            {
                case ETeamType.Left:
                    candyModel.OnLeftScoreChanged -= SetScoreView;
                    candyModel.OnLeftScoreChanged += SetScoreView;
                    break;

                case ETeamType.Right:
                    candyModel.OnRightScoreChanged -= SetScoreView;
                    candyModel.OnRightScoreChanged += SetScoreView;
                    break;
            }
        }

        view.SetPresenter(this);
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
        if (param is CollectingCandyParam collectingCandyParam)
        {
            SetItemCount(collectingCandyParam.CandyItems);
            SetScore(collectingCandyParam.CurrScore);
        }
    }

    public void SetItemCount(int[] itemCounts)
    {
        if (view is CollectCandyView candyView)
        {
            candyView.UpdateItemCount(itemCounts);
        }
    }

    public void SetScore(int score)
    {
        if (model is CollectCandyModel candyModel)
        {
            switch(teamType)
            {
                case ETeamType.Left:
                    candyModel.SetLeftScore(score);
                    break;

                case ETeamType.Right:
                    candyModel.SetRightScore(score);
                    break;
            }
        }
    }

    public void SetScoreView(int score)
    {
        if(view is CollectCandyView candyView)
        {
            candyView.UpdateScore(score);
        }
    }

    public void SetTime()
    {
        if(view is CollectCandyView candyView)
        {
            if (model is CollectCandyModel candyModel)
            {
                candyView.UpdateTime(candyModel.GetFormattedTime());
            }
        }
    }

    public override void Clear()
    {
        base.Clear();

        if (model is CollectCandyModel candyModel)
        {
            candyModel.OnTimeChanged -= SetTime;

            switch (teamType)
            {
                case ETeamType.Left:
                    candyModel.OnLeftScoreChanged -= SetScoreView;
                    break;

                case ETeamType.Right:
                    candyModel.OnRightScoreChanged -= SetScoreView;
                    break;
            }
        }
    }
}
