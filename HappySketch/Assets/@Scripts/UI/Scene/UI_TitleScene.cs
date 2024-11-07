using MomDra;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_BaseScene
{
    private PresenterBase presenter;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void Foo()
    {

    }
}
