using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvasNotOverlay(gameObject, false);
        Managers.UI.SetSceneUI(this);

        return true;
    }
}
