using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static Define;

public class CollectCandyPresenter : PresenterBase
{
    private int score = 0;

    public CollectCandyPresenter(ViewBase view, ModelBase model, ETeamType teamType) : base(view, model, teamType)
    {
        // 여기서 모델의 이벤트 등록이 필요하다
        // 모델에서는 이벤트를 발생 시켜야 한다

        if(model is CollectCandyModel candyModel)
        {
            candyModel.OnTimeChangedEvent -= SetTime;
            candyModel.OnTimeChangedEvent += SetTime;
            candyModel.StartTimer();

            candyModel.OnScoreChangedEvent -= SetScoreView;
            candyModel.OnScoreChangedEvent += SetScoreView;
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

    public void SetItemCount(EItemType itemType, int itemCount)
    {
        if(view is CollectCandyView candyView)
        {
            candyView.UpdateItemCount(itemType, itemCount);
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
            candyModel.SetScore(score);
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
            candyModel.OnTimeChangedEvent -= SetTime;
            candyModel.OnScoreChangedEvent -= SetScoreView;
        }
    }
}
