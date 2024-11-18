using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CollectCandyPresenter : PresenterBase
{
    public CollectCandyPresenter(ViewBase view, ModelBase model) : base(view, model)
    {
        // 여기서 모델의 이벤트 등록이 필요하다
        // 모델에서는 이벤트를 발생 시켜야 한다

        if(model is CollectCandyModel candyModel)
        {
            candyModel.TimeChangedEvent += SetTime;
            candyModel.StartTimer();
        }

        view.SetPresenter(this);
    }

    public override void OnStageInfoUpdate(StageParam param)
    {
        if (param is CollectingCandyParam collectingCandyParam)
        {
            foreach(ECandyItemType itemType in collectingCandyParam.CandyItemList)
            {
                // 수정 필요
            }
        }
    }

    public void SetItemCount(EItemType itemType, int itemCount)
    {
        if(view is CollectCandyView candyView)
        {
            candyView.UpdateItemCount(itemType, itemCount);
        }
    }

    public void SetScore(int score)
    {
        if (view is CollectCandyView candyView)
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
}
