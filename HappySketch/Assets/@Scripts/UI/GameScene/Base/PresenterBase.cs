using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PresenterBase
{
    protected ViewBase view;
    protected ModelBase model;

    public PresenterBase(ViewBase view, ModelBase model)
    {
        this.view = view;
        this.model = model;

        view.SetPresenter(this);
    }
}

