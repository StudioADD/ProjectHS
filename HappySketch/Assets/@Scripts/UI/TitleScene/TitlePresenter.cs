using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitlePresenter : PresenterBase
{

    public TitlePresenter(ViewBase view, ModelBase model) : base(view, model)
    {
        view.SetPresenter(this);
    }

    public override void OnStageInfoUpdate(StageParam param)
    {
        // 이새기 확인 요망 (희용)
    }
}
