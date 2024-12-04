using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class PresenterBase
{
    protected ETeamType teamType { get; private set; }

    protected ViewBase view;
    protected ModelBase model;

    public PresenterBase(ViewBase view, ModelBase model, ETeamType teamType)
    {
        this.view = view;
        this.model = model;
        this.teamType = teamType;

        view.SetPresenter(this);
    }

    public ModelBase GetModel() { return model; }

    public abstract void OnStageInfoUpdate(StageParam param);

    public abstract void ConnectStageEvents(BaseStage stage);

    public virtual void Clear()
    {
        model.Clear();
    }
}
