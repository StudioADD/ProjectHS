using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAvoidancePresenter : PresenterBase
{
    public SharkAvoidancePresenter(ViewBase view, ModelBase model) : base(view, model)
    {

    }

    /// <summary>
    /// 아이템 수를 설정하는 함수
    /// </summary>
    /// <param name="count"> 아이템 수: 0 ~ 3 </param>
    public void SetItemCount(int count)
    {
        if (model is SharkAvoidanceModel model1)
            model1.SetItemCount(count);

        if (view is SharkAvoidanceView view1)
            view1.UpdateItemCount(count);
    }

    /// <summary>
    /// 총 이동경로 대비 현재 이동 경로 비율
    /// </summary>
    /// <param name="ratio">비율: 0 ~ 1 </param>
    public void SetLeftProgressRatio(float ratio)
    {
        if (model is SharkAvoidanceModel model1)
        {
            model1.SetLeftProgressRatio(ratio);

            if (view is SharkAvoidanceView view1)
                view1.UpdateLeftProgressRatio(model1.TeamType, ratio);
        }
    }

    public void SetRightProgressRatio(float ratio)
    {
        if (model is SharkAvoidanceModel model1)
        {
            model1.SetRightProgressRatio(ratio);

            if (view is SharkAvoidanceView view1)
                view1.UpdateRightProgressRatio(model1.TeamType, ratio);
        }
    }
}
