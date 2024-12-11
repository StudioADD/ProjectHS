using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class UI_PresenterBase
{
    protected ETeamType TeamType { get; private set; }

    protected UI_ViewBase view;
    protected UI_ModelBase model;

    public UI_PresenterBase(UI_ViewBase view, UI_ModelBase model, ETeamType teamType)
    {
        this.view = view;
        this.model = model;
        TeamType = teamType;
    }

    public UI_ModelBase GetModel() { return model; }

    public abstract void OnStageInfoUpdate(StageParam param);

    public abstract void ConnectStageEvents(BaseStage stage);

    public virtual void Clear()
    {
        model.Clear();
    }
}
