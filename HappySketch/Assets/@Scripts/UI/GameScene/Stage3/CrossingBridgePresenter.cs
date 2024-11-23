using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingBridgePresenter : PresenterBase
{
    public CrossingBridgePresenter(ViewBase view, ModelBase model) : base(view, model)
    {

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
